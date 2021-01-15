using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    //Private variables
    [SerializeField] private float speed = 2.5f;
    private float pedSpeed;

    //Private variables for positions outside map
    [SerializeField] private float horizontalOutside = 15.0f;
    [SerializeField] private float verticalOutside = 10.0f;

    //Animator to launch animation based on state for pedestrian
    private Animator pedAnim;

    //Value for scoring cars
    [SerializeField] private int scorePed = 5;
    private GameManager gameManager;

    void Awake()
    { 
        //Get necessary components when object is activated
        pedAnim = GetComponent<Animator>();
        pedSpeed = speed;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Make pedestrian move forward
        transform.Translate(Vector3.forward * Time.deltaTime * pedSpeed);
        DeactivateOutsideRoad();
    }

    private void DeactivateOutsideRoad()
    {
        //Deactivate pedestrian object if out of the road on camera
        if (transform.position.x <= -horizontalOutside || transform.position.x >= horizontalOutside || transform.position.z <= -verticalOutside || transform.position.z >= verticalOutside)
        {
            gameObject.SetActive(false);
            gameManager.UpdateScore(scorePed);
        }
    }

    private void OnMouseDown()
    {
        //Stop the pedestrian and animation
        if (pedSpeed > 0)
        {
            pedSpeed = 0;
            pedAnim.SetBool("walking", false);
        }
        //Make the pedestrian move and make it animate
        else if (pedSpeed == 0)
        {
            pedSpeed = speed;
            pedAnim.SetBool("walking", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If two pedestrian collide, make the inactive to avoid weird collisions
        if(collision.gameObject.CompareTag("Pedestrian"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
