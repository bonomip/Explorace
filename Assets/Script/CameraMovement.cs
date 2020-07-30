using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public int direction = 0;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        if (direction == 0)
        {
            target = new Vector3(transform.position.x + (float)1.5, transform.position.y, transform.position.z);
        }

        if (direction == 1)
        {
            target = new Vector3(transform.position.x, transform.position.y, transform.position.z + (float)1.5);
        }

        if (direction == 2)
        {
            target = new Vector3(transform.position.x - (float)1.5, transform.position.y, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime / 3);
    }
}
