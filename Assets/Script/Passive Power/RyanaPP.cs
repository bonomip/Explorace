using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyanaPP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	this.gameObject.tag = "Ryana";

    	GameObject[] walls = GameObject.FindGameObjectsWithTag("CrystalWall");
    	Debug.Log("CrystallWall: " + walls.Length);

    	foreach(GameObject o in walls)
    	{
    		o.GetComponent<CrystalWall>().Init();
    	}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
