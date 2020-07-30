using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVP;

public class Spike : MonoBehaviour
{
	private RVP.BasicInput bi;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(waitAndDestroy(10f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
    	if(col.gameObject.layer == 9)
    	{
    		Debug.Log(col.gameObject.name);
    		GameObject kart = col.gameObject;
    		this.bi = kart.GetComponent<RVP.BasicInput>();
    		this.bi.onSpikeCollision();
    		Destroy(this.gameObject);
    	}
    }

    private IEnumerator waitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
