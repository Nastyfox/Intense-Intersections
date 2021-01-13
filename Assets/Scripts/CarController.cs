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
    private List<GameObject> activeCars;

    private GameObject lights;
    public int carPosition;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        carSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        GetCarPosition();

        activeCars = GameObject.Find("SpawnManager").GetComponent<ObjectPooling>().GetActiveCars();

        RedLights();
        GreenLights();

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

    private void RedLights()
    {
        if(lights.GetComponent<TrafficLightController>().isRed)
        {
            float distanceCars;
            float angleCars;
            Vector3 targetDirectionCars;
            
            float distanceLights = Vector3.Distance(lights.transform.position, transform.position);

            if (distanceLights < 2.5f)
            {
                carSpeed = 0;
            }
            else
            {
                for (int i = 0; i < activeCars.Count; i++)
                {
                    if(activeCars[i].transform.position != transform.position)
                    {
                        targetDirectionCars = activeCars[i].transform.position - transform.position;
                        distanceCars = Vector3.Distance(activeCars[i].transform.position, transform.position);
                        angleCars = Vector3.Angle(targetDirectionCars, transform.forward);

                        if (distanceCars < 2 && angleCars < 90)
                        {
                            carSpeed = 0;
                        }
                    }
                }
            }
        }
    }

    private void GreenLights()
    {
        if (lights.GetComponent<TrafficLightController>().isGreen)
        {
            if(carSpeed == 0)
            {
                carSpeed = speed;
            }
            
        }
    }

    private void GetCarPosition()
    {
        switch (carPosition)
        {
            case (int)SpawnManager.carPositions.Left:
                lights = GameObject.Find("TrafficLightLeft").transform.Find("Lights").gameObject;
                break;
            case (int)SpawnManager.carPositions.Right:
                lights = GameObject.Find("TrafficLightRight").transform.Find("Lights").gameObject;
                break;
            case (int)SpawnManager.carPositions.Up:
                lights = GameObject.Find("TrafficLightUp").transform.Find("Lights").gameObject;
                break;
            case (int)SpawnManager.carPositions.Bot:
                lights = GameObject.Find("TrafficLightBot").transform.Find("Lights").gameObject;
                break;
        }
    }
}
