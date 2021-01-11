using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Private variables for spawning cars
    [SerializeField] private float carRepeatingTime = 2.5f;
    [SerializeField] private float pedRepeatingTime = 2.5f;
    private Coroutine spawnCars;
    private Coroutine spawnPeds;
    private GameManager gameManager;
    private int randomCarStart;
    private int randomPedStart;
    private int randomRotation;
    private int carNumPositions = 4;
    private int pedNumPositions = 8;
    private int numRotations = 4;
    private Vector3[] carStartPositions;
    private Quaternion[] startRotations;  
    private Vector3[] pedStartPositions;
    private Vector3 carPosition;
    private Quaternion carRotation;
    private Vector3 pedPosition;
    private Quaternion pedRotation;

    //Public variable for car prefab
    [SerializeField] private GameObject carPrefab;
    private GameObject pedPrefab;
    private int pedType;
    [SerializeField] private GameObject[] pedPrefabs;

    //Private variables for standard positions
    [SerializeField] private float xSide = 15.0f;
    [SerializeField] private float zSide = 10.0f;
    [SerializeField] private float yHeight = 0.5f;

    //Private variables for standard rotations
    [SerializeField] private float faceRight = 90.0f;
    [SerializeField] private float faceLeft = -90.0f;
    [SerializeField] private float faceUp = 0.0f;
    [SerializeField] private float faceDown = 180.0f;

    //Private variable for width and offset
    private float roadWidth;
    private float carOffset;
    private float pedestrianOffset1;
    private float pedestrianOffset2;

    // Start is called before the first frame update
    void Start()
    {
        //Get road with to shift cars and pedestrians
        roadWidth = GameObject.Find("RoadVertical").GetComponent<MeshRenderer>().bounds.size.x;
        carOffset = roadWidth / 4;
        pedestrianOffset1 = 3 * roadWidth / 4;
        pedestrianOffset2 = roadWidth;

        //Create start positions and start rotations
        carStartPositions = new Vector3[carNumPositions];
        startRotations = new Quaternion[numRotations];
        pedStartPositions = new Vector3[pedNumPositions];

        carStartPositions[0] = new Vector3(-xSide, yHeight, -carOffset);
        carStartPositions[1] = new Vector3(xSide, yHeight, carOffset);
        carStartPositions[2] = new Vector3(-carOffset, yHeight, zSide);
        carStartPositions[3] = new Vector3(carOffset, yHeight, -zSide);

        pedStartPositions[0] = new Vector3(-xSide, yHeight, -pedestrianOffset2);
        pedStartPositions[1] = new Vector3(xSide, yHeight, pedestrianOffset2);
        pedStartPositions[2] = new Vector3(-pedestrianOffset2, yHeight, zSide);
        pedStartPositions[3] = new Vector3(pedestrianOffset2, yHeight, -zSide);
        pedStartPositions[4] = new Vector3(-xSide, yHeight, pedestrianOffset1);
        pedStartPositions[5] = new Vector3(xSide, yHeight, -pedestrianOffset1);
        pedStartPositions[6] = new Vector3(pedestrianOffset1, yHeight, zSide);
        pedStartPositions[7] = new Vector3(-pedestrianOffset1, yHeight, -zSide);

        startRotations[0] = Quaternion.Euler(0,faceRight, 0);
        startRotations[1] = Quaternion.Euler(0, faceLeft, 0);
        startRotations[2] = Quaternion.Euler(0, faceDown, 0);
        startRotations[3] = Quaternion.Euler(0, faceUp, 0);

        //Start spawning car every couple of seconds
        spawnCars = StartCoroutine(SpawnCarsRandom());
        spawnPeds = StartCoroutine(SpawnPedestriansRandom());
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
}

    // Update is called once per frame
    void LateUpdate()
    {
        if(gameManager.isGameOver)
        {
            StopCoroutine(spawnCars);
            StopCoroutine(spawnPeds);
        }
    }

    //Method to spawn cars at random positions
    IEnumerator SpawnCarsRandom()
    {
        while(true)
        {
            yield return new WaitForSeconds(carRepeatingTime);
            randomCarStart = Random.Range(0, carNumPositions);
            randomRotation = randomCarStart;
            carPosition = carStartPositions[randomCarStart];
            carRotation = startRotations[randomRotation];

            GameObject car = ObjectPooling.SharedInstance.GetCarObject();
            if(car != null)
            {
                car.transform.position = carPosition;
                car.transform.rotation = carRotation;
                car.SetActive(true);
            }
        }
    }

    IEnumerator SpawnPedestriansRandom()
    {
        while(true)
        {
            yield return new WaitForSeconds(pedRepeatingTime);
            pedType = Random.Range(0, pedPrefabs.Length);
            pedPrefab = pedPrefabs[pedType];
            randomPedStart = Random.Range(0, pedNumPositions);
            randomRotation = randomPedStart % 4;
            pedPosition = pedStartPositions[randomPedStart];
            pedRotation = startRotations[randomRotation];

            GameObject pedestrian = ObjectPooling.SharedInstance.GetPedObject(pedType);
            if (pedestrian != null)
            {
                pedestrian.transform.position = pedPosition;
                pedestrian.transform.rotation = pedRotation;
                pedestrian.SetActive(true);
            }
        }
    }
}
