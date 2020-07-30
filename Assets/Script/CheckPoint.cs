using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
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
        if (col.gameObject.layer == 9)
        {
            	col.transform.parent.gameObject.GetComponent<RVP.BasicInput>().setCheckPoint(this.transform);
        }     
    }
}
