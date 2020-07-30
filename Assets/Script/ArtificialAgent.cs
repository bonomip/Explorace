using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVP;

public class ArtificialAgent : MonoBehaviour
{
    private static float NODE_DISTANCE_THR = 20.0F;
    private static float BRAKE_VELOCITY_THR = 10.0F;
    private static float BRAKE_ANGLE_THR = 50.0F;
    private static float DIRECTIONAL_CONE_THR = 5.0F;
    private static float STOP_VELOCITY_THR = 0.5F;
    private static float STOP_TIME = 1.0F;
    private static float RECOVER_TIME = 2.0F;

    public TrackPath path;

    private Transform targetPoint;
    private int currentTargetIndex;
    private RVP.BasicInput inputScript;

    private float timer;
    private bool isStill;
    private bool inRecoverRoutine;
    
    // Start is called before the first frame update
    void Start()
    {
        GameSetUp gameSetUp = GameObject.Find("GameSetUp").GetComponent<GameSetUp>();

        this.path = gameSetUp.aiPath;
        this.targetPoint = this.path.pathPoints[0];
        this.currentTargetIndex = 0;
        this.inputScript = this.GetComponent<RVP.BasicInput>();

        this.timer = 0;
        this.isStill = false;
        this.inRecoverRoutine = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromTarget = this.computeDistanceFromTarget();
        float currentTime;

        if (distanceFromTarget < NODE_DISTANCE_THR)
        {
            this.updatePathProgression();
        }

        Vector3 currentVelocity = this.GetComponent<Rigidbody>().velocity;
        currentVelocity.y = 0;

        float velocityMagnitude = Vector3.Magnitude(currentVelocity);

        if (!inRecoverRoutine)
        {
            if (velocityMagnitude < STOP_VELOCITY_THR)
            {
                currentTime = Time.time;

                if (!isStill)
                {
                    this.timer = currentTime;
                    isStill = true;
                }

                if (currentTime - this.timer >= STOP_TIME)
                {
                    this.inRecoverRoutine = true;

                    this.timer = currentTime;
                    this.isStill = false;
                }
            }
            else
            {
                this.timer = 0;
                this.isStill = false;
            }
        }

        Vector3 currentPosition = this.GetComponent<Rigidbody>().position;
        Vector3 directionToTarget = targetPoint.position - currentPosition;

        Vector3 currentDirection = this.transform.forward;
        currentDirection.y = 0;

        float angle = Vector3.SignedAngle(currentDirection, directionToTarget, Vector3.up);

        /*** Control logic ***/
        if (!inRecoverRoutine)
        {
            this.inputScript.goStraight();
            this.inputScript.accelerate();

            if (angle < -DIRECTIONAL_CONE_THR)
            {
                if (angle < -BRAKE_ANGLE_THR && velocityMagnitude > BRAKE_VELOCITY_THR)
                {
                    this.inputScript.brakeV();
                }
                this.inputScript.turnLeft();
            }
            else if (angle > DIRECTIONAL_CONE_THR)
            {
                if (angle > BRAKE_ANGLE_THR && velocityMagnitude > BRAKE_VELOCITY_THR)
                {
                    this.inputScript.brakeV();
                }
                this.inputScript.turnRight();
            }

            this.inputScript.usePowerUp();
        }
        else
        {
            currentTime = Time.time;
            this.inputScript.brakeV();
            if (currentTime - this.timer >= RECOVER_TIME)
            {
                this.inRecoverRoutine = false;
                this.timer = 0;
            } 
        }
    }

    private float computeDistanceFromTarget()
    {
        Rigidbody rigidBody = this.GetComponent<Rigidbody>();

        Vector3 currentPositionVec = new Vector3(rigidBody.position.x, rigidBody.position.y, rigidBody.position.z);
        Vector3 targetVec = new Vector3(this.targetPoint.position.x, this.targetPoint.position.y, this.targetPoint.position.z);

        return Vector3.Distance(targetVec, currentPositionVec);
    }

    private void updatePathProgression()
    {
        this.currentTargetIndex++;

        if (this.currentTargetIndex == this.path.pathPoints.Length)
        {
            this.currentTargetIndex = 0;

            if (this.path.nextPaths.Length != 0)
            {
                int nextPathIndex = (int)Mathf.Round(Random.Range(-0.5F, this.path.nextPaths.Length - 0.5F));

                this.path = this.path.nextPaths[nextPathIndex];
            }
            else
            {
                this.GetComponent<ArtificialAgent>().enabled = false;
                this.GetComponent<RVP.BasicInput>().enabled = false;
            }
        }

        this.targetPoint = this.path.pathPoints[currentTargetIndex];
    }
}
