using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCurrent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
    	if(col.gameObject.tag == "Slifis")
    	{
    		col.gameObject.GetComponent<SlifisPP>().onAir(true);
    	}
    }

    void OnTriggerStay(Collider col)
    {
    	if(col.gameObject.tag == "Slifis")
    	{
    		col.gameObject.GetComponent<SlifisPP>().use();
    	}
    }

    void OnTriggerExit(Collider col)
    {
    	if(col.gameObject.tag == "Slifis")
    	{
    		col.gameObject.GetComponent<SlifisPP>().onAir(false);
    	}
    }
}
