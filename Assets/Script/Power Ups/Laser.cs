using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private RVP.BasicInput bi;

    // Start is called before the first frame update
    void Start()
    {
        //not strictly necessary, disable if framerate drops
        StartCoroutine(waitAndDestroy(10f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("collisione laser");
        if (col.gameObject.layer == 9)
        {
            Debug.Log("colpito kart");
            GameObject kart = col.gameObject;
            this.bi = kart.GetComponent<RVP.BasicInput>();
            this.bi.onLaserCollision();
        }

        Destroy(this.gameObject);
    }

    private IEnumerator waitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
