using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVP;

public class PowerUp : MonoBehaviour
{
	private int kind;
	private RVP.BasicInput bi;
	//private float rotSpeed;


    //1 is boost
    //2 is spike
    //3 is ray
    void Start()
    {
      this.kind = randomizeKind();
      //this.rotSpeed = Random.Range(20f,40f);
        //chose the type of power point random
    }

    void OnTriggerEnter(Collider col)
    {
        
    	if(col.gameObject.layer == 9)
    	{
            
    		makeMeInvisible();

    		GameObject kart = col.gameObject.transform.parent.gameObject;

            //changing the probability for the powerups in case the kart has a theme
            if (kart.GetComponent<ParticleSystem>().emission.enabled)
            {
                Debug.Log("Changing the prob");
                float rand = Random.value;
                if (rand <= 0.5f)
                {
                    this.kind = Mathf.RoundToInt(Random.Range(1f, 2f));
                }
                else
                {
                    this.kind = 3;
                }
            }
    		this.bi = kart.GetComponent<RVP.BasicInput>();
            this.bi.getPowerUp(this.kind);
            if(!this.bi.isArtificialAgent) GetComponent<AudioSource>().Play();
    	}
    }

    //void FixedUpdate()
    //{
    //	this.transform.Rotate(new Vector3(0f,1f,0f), this.rotSpeed * Time.deltaTime);
    //}

    void makeMeInvisible()
    {//start co routine that will bi re enable the object after n seconds
    	
    	GetComponent<Renderer>().enabled = false;
    	GetComponent<BoxCollider>().enabled = false;
        GetComponent<ParticleSystem>().enableEmission = false;
    	StartCoroutine(waitAndMakeMeVisible(3f));
    }

    void makeMeVisibile()
    {//chose the type of power up random
    	kind = randomizeKind();
    	GetComponent<Renderer>().enabled = true;
    	GetComponent<BoxCollider>().enabled = true;
        GetComponent<ParticleSystem>().enableEmission = true;
    }

    private IEnumerator waitAndMakeMeVisible(float waitTime)
    {
  		yield return new WaitForSeconds(waitTime);
  		makeMeVisibile();
    }

    private int randomizeKind()
    {
    	return Mathf.RoundToInt(Random.Range(1f,3f));
    }
}
