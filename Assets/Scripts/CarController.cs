using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Private variables
    [SerializeField] private float speed = 5.0f;
    private float carSpeed;

    //Private variables for positions outside map
    [SerializeField] private float horizontalOutside = 15.0f;
    [SerializeField] private float verticalOutside = 10.0f;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        carSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //Make car move forward
        transform.Translate(Vector3.forward * Time.deltaTime * carSpeed);
        DeactivateOutsideRoad();
    }

    private void DeactivateOutsideRoad()
    {
        if(transform.position.x <= -horizontalOutside  || transform.position.x >= horizontalOutside || transform.position.z <= -verticalOutside || transform.position.z >= verticalOutside)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pedestrian") || collision.gameObject.CompareTag("Car"))
        {
            print("Game Over");
            gameManager.GameOver();
            collision.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if(carSpeed > 0)
        {
            carSpeed = 0;
        }
        else if(carSpeed == 0)
        {
            carSpeed = speed;
        }
    }
}
