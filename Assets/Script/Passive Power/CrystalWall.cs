using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalWall : MonoBehaviour
{
	public GameObject[] ryana;

    public void Init()
    {
    	ryana = GameObject.FindGameObjectsWithTag("Ryana"); //get all the car with ryana passive power
    	
    	foreach (GameObject o in ryana)
        {
			IgnoreCollisionRecursive(o, GetComponent<Collider>());     
        }
    }

    public void IgnoreCollisionRecursive(GameObject obj, Collider wall){
    	if(obj.GetComponent<Collider>() != null ) Physics.IgnoreCollision(obj.GetComponent<Collider>(), wall);
    	
    	if(obj.transform.GetChildCount() > 0)
         foreach(Transform t in obj.transform)
             IgnoreCollisionRecursive(t.gameObject, wall);
    }
}
