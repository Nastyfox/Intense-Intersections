using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Private variables
    [SerializeField] private float speed = 5.0f;
    private float carSpeed;

    //Private variables for positions outside map
    [SerializeField] private float horizontalOutside = 16.0f;
    [SerializeField] private float verticalOutside = 11.0f;

    //GameManager to set Game Over when collision
    private GameManager gameManager;

    //List of all active cars in the scene
    private List<GameObject> activeCars;

    //Light associated to the lane of the car based on position
    private GameObject lights;
    public int carPosition;

    //Distance to stop the car
    [SerializeField] private float stopDistanceLights = 2.5f;
    [SerializeField] private float stopDistanceCar = 2;
    [SerializeField] private float goDistanceCar = 3;
    [SerializeField] private float stopAngleCars = 20;
    [SerializeField] private float stopAngleLights = 1;
    private bool lightsPassed;
    private bool canGo;
    private bool mustStop;
    [SerializeField] private float angleLightsPassed = 50;
    private bool mouseClicked;

    // Start is called before the first frame update
    void Awake()
    {
        //Get game manager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Set speed
        carSpeed = speed;

        //Lights is not passed
        lightsPassed = false;

        //Mouse has not been clicked at first
        mouseClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if mouse has not been clicked
        if(!mouseClicked)
        {
            //Get car position in order to set lights
            GetCarPosition();

            //Get all active cars to check position
            activeCars = GameObject.Find("SpawnManager").GetComponent<ObjectPooling>().GetActiveCars();

            //Launch green and red lights to stop car based on traffic lights
            if (lights.GetComponent<TrafficLightController>().isRed)
            {
                RedLights();
            }
            else if (lights.GetComponent<TrafficLightController>().isGreen)
            {
                GreenLights();
            }
        }

        //Make car move forwar
        transform.Translate(Vector3.forward * Time.deltaTime * carSpeed);

        //Deactivate cars when they pass a certain point
        DeactivateOutsideRoad();
    }

    private void DeactivateOutsideRoad()
    {
        //If they cross a certain point on map, car becomes inactive
        if(transform.position.x <= -horizontalOutside  || transform.position.x >= horizontalOutside || transform.position.z <= -verticalOutside || transform.position.z >= verticalOutside)
        {
            lightsPassed = false;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If there is a collisio between 2 cars or car and pedestrian, game is over
        if (collision.gameObject.CompareTag("Pedestrian") || collision.gameObject.CompareTag("Car"))
        {
            print("Game Over");
            gameManager.GameOver();
            collision.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        mouseClicked = true;
        //If car is moving when clicked, stop it
        if(carSpeed > 0)
        {
            carSpeed = 0;
        }
        //If car is stopped when clicked, move it
        else if(carSpeed == 0)
        {
            carSpeed = speed;
        }
    }

    private void RedLights()
    {
        //Car doesn't have to stop as default
        mustStop = false;

        //Variables for distance and angle with other cars
        float distanceCars;
        float angleCars;
        Vector3 targetDirectionCars;

        //Distance with traffic lights
        float distanceLights = Vector3.Distance(lights.transform.position, transform.position);
        Vector3 targetDirectionLights = lights.transform.position - transform.position;
        float angleLigths = Vector3.Angle(targetDirectionLights, transform.up);
        
        //Check if lights has been passed
        if(angleLigths < angleLightsPassed)
        {
            lightsPassed = true;
        }

        //Check position with other cars and stop it if it is close to the previous one in front
        for (int i = 0; i < activeCars.Count; i++)
        {
            if (activeCars[i].transform.position != transform.position)
            {
                targetDirectionCars = activeCars[i].transform.position - transform.position;
                distanceCars = Vector3.Distance(activeCars[i].transform.position, transform.position);
                angleCars = Vector3.Angle(targetDirectionCars, transform.forward);

                if (distanceCars < stopDistanceCar && angleCars < stopAngleCars)
                {
                    mustStop = true;
                }
            }
        }

        //If car is close to traffic light and car has not passed the light yet, stop it
        if ((distanceLights < stopDistanceLights && angleLigths < stopAngleLights && !lightsPassed) || mustStop)
        {
            carSpeed = 0;
        }
        else
        {
            carSpeed = speed;
        }
    }

    private void GreenLights()
    {
        //Start the car again
        if (carSpeed == 0)
        {
            //Variables for distance and angle with other cars
            float distanceCars;
            float angleCars;
            Vector3 targetDirectionCars;

            //Car can go as default
            canGo = true;

            //Check position with other cars and make it move if it is far enough to the previous one in front
            for (int i = 0; i < activeCars.Count; i++)
            {
                if (activeCars[i].transform.position != transform.position)
                {
                    targetDirectionCars = activeCars[i].transform.position - transform.position;
                    distanceCars = Vector3.Distance(activeCars[i].transform.position, transform.position);
                    angleCars = Vector3.Angle(targetDirectionCars, transform.forward);

                    //If the previous car in front is not far enough, the car can't move
                    if(angleCars < stopAngleCars && distanceCars < goDistanceCar)
                    {
                        canGo = false;
                    }
                }
            }

            //If the way is clear, speed is set
            if(canGo)
            {
                carSpeed = speed;
            }
        }
    }

    private void GetCarPosition()
    {
        //Get correct light of the road line based on the car position
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
