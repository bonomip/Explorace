using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVP;

public class SlifisPP : MonoBehaviour
{


	Rigidbody rb;
	bool onAirSteering;

    // Start is called before the first frame update
    void Start()
    {
    	this.rb = GetComponent<Rigidbody>();
    	this.gameObject.tag = "Slifis";

        BoxCollider bc = this.gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider; //0.6, 0.4, 1.5
        bc.size = new Vector3(0.6f, 0.4f, 1.5f);
        bc.isTrigger = true;
    }

    public void use()
    {
    	rb.AddForce(Vector3.up * 1.5f);
    }

    public void onAir(bool value)
    {
    	 onAirSteering = value;
    }

}
