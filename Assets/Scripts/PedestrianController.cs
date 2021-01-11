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

    private Animator pedAnim;

    // Start is called before the first frame update
    void Awake()
    {
        pedAnim = GetComponent<Animator>();
        pedSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //Make car move forward
        transform.Translate(Vector3.forward * Time.deltaTime * pedSpeed);
        DeactivateOutsideRoad();
    }

    private void DeactivateOutsideRoad()
    {
        if (transform.position.x <= -horizontalOutside || transform.position.x >= horizontalOutside || transform.position.z <= -verticalOutside || transform.position.z >= verticalOutside)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (pedSpeed > 0)
        {
            pedSpeed = 0;
            pedAnim.SetBool("walking", false);
        }
        else if (pedSpeed == 0)
        {
            pedSpeed = speed;
            pedAnim.SetBool("walking", true);
        }
    }
}
