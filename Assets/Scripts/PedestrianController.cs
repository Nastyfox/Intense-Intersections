using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    //Private variables
    private float speed = 2.5f;

    //Private variables for positions outside map
    private float horizontalOutside = 15.0f;
    private float verticalOutside = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Make car move forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        DestroyOutsideRoad();
    }

    private void DestroyOutsideRoad()
    {
        if (transform.position.x <= -horizontalOutside || transform.position.x >= horizontalOutside || transform.position.z <= -verticalOutside || transform.position.z >= verticalOutside)
        {
            Destroy(gameObject);
        }
    }
}
