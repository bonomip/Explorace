using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject vehicle;
	private Transform finish;
    // Start is called before the first frame update
    void Start()
    {
        this.finish = GameObject.Find("Finish").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 finishDirection = this.finish.position - this.transform.position;
        finishDirection.y = 0;
        Vector3 frontDirection = this.vehicle.transform.forward;
        frontDirection.y = 0;
        float arrowAngle = Vector3.SignedAngle(frontDirection, finishDirection, Vector3.up);
        this.transform.eulerAngles = new Vector3(this.vehicle.transform.eulerAngles.x + 20, this.vehicle.transform.eulerAngles.y, -arrowAngle);
    }

    public void setVehicle(GameObject ve)
    {
        this.vehicle = ve;
    }
}
