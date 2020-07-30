using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleRotation : MonoBehaviour
{
    Vector3 v;
    public float rotTimer;
    public Button b;

    void Start()
    {
        v = Random.onUnitSphere;
    }


    float time;

    //void FixedUpdate()
    //{
    //    time += Time.deltaTime;

    //    if (time >= rotTimer)
    //    {
    //        v = Random.onUnitSphere;
    //        time = 0;
    //    }

    //    this.transform.Rotate(v, 40f * Time.deltaTime);

    //}

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            transform.Rotate(Vector3.up * -touchDeltaPosition.x);
            transform.Rotate(Vector3.right * touchDeltaPosition.y);
        }

        time += Time.deltaTime;

        if (time >= rotTimer)
        {
            v = Random.onUnitSphere;
            time = 0;
        }

        this.transform.Rotate(v, 40f * Time.deltaTime);
    }


}
