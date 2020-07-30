using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinishLine : MonoBehaviour
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
    	if(col.gameObject.layer == 9 && col.gameObject.name == "collider")
    	{
    		Debug.Log("Collision with finish line: " + col.gameObject.name);
    		GameObject kart = col.gameObject.transform.parent.gameObject;
            
    		kart.GetComponent<RVP.BasicInput>().enabled = false;

            if (kart.GetComponent<ArtificialAgent>() != null)
                kart.GetComponent<ArtificialAgent>().enabled = false;
            else
                kart.GetComponent<RVP.BasicInput>().leaveRoom();
        }
    }
}
