using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Private variables for spawning cars
    private float delayStart = 2.0f;
    private float repeatingTime = 5.0f;
    private int randomCarStart;
    private int numPositions = 4;
    private Vector3[] startPositions;
    private Quaternion[] startRotations;
    private Vector3 carPosition;
    private Quaternion carRotation;

    //Public variable for car prefab
    public GameObject carPrefab;

    //Private variables for standard positions
    private float xSide = 8.0f;
    private float zSide = 4.0f;

    //Private variables for standard rotations
    private float faceRight = 90.0f;
    private float faceLeft = -90.0f;
    private float faceUp = 0.0f;
    private float faceDown = 180.0f;

    //Private variable for width
    private MeshRenderer carRenderer;
    private float carWidth;

    // Start is called before the first frame update
    void Start()
    {
        //Get car width using renderer size
        carRenderer = carPrefab.GetComponent<MeshRenderer>();
        carWidth = carRenderer.bounds.size.x;

        //Create start positions and start rotations
        startPositions = new Vector3[numPositions];
        startRotations = new Quaternion[numPositions];

        startPositions[0] = new Vector3(-xSide, 1, -carWidth / 2);
        startPositions[1] = new Vector3(xSide, 1, carWidth / 2);
        startPositions[2] = new Vector3(-carWidth / 2, 1, zSide);
        startPositions[3] = new Vector3(-carWidth / 2, 1, -zSide);

        startRotations[0] = Quaternion.Euler(0,faceRight, 0);
        startRotations[1] = Quaternion.Euler(0, faceLeft, 0);
        startRotations[2] = Quaternion.Euler(0, faceDown, 0);
        startRotations[3] = Quaternion.Euler(0, faceUp, 0);

        //Start spawning car every couple of seconds
        InvokeRepeating("SpawnCarsRandom", delayStart, repeatingTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Method to spawn cars at random positions
    private void SpawnCarsRandom()
    {
        randomCarStart = Random.Range(0, startPositions.Length);
        carPosition = startPositions[randomCarStart];
        carRotation = startRotations[randomCarStart];

        Instantiate(carPrefab, carPosition, carRotation);
    }
}
