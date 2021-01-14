using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public bool isGreen;
    public bool isRed;
    private MeshRenderer lightsMeshRenderer;

    private Color greenAlbedo;
    private Color greenSpecular;
    private Material greenLight;

    private Color redAlbedo;
    private Color redSpecular;
    private Material redLight;

    private Color yellowAlbedo;
    private Color yellowSpecular;
    private Material yellowLight;


    // Start is called before the first frame update
    void Start()
    {
        //Get all materials for the three color lights and there based color
        lightsMeshRenderer = gameObject.GetComponent<MeshRenderer>();

        yellowLight = lightsMeshRenderer.materials[0];
        yellowAlbedo = new Color(1, 1, 0);
        yellowSpecular = new Color(0.78f, 0.76f, 0);

        redLight = lightsMeshRenderer.materials[1];
        redAlbedo = new Color(1, 0, 0);
        redSpecular = new Color(0.78f, 0, 0);

        greenLight = lightsMeshRenderer.materials[2];
        greenAlbedo = new Color(0, 1, 0.47f);
        greenSpecular = new Color(0, 0.76f, 0.35f);

        isGreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Change colors to make traffic light appear green
        if (isGreen)
        {
            RedToGreen();
            isRed = false;
            isGreen = true;
        }
        //Change colors to make traffic light appear red after yellow
        else if (!isGreen && !isRed)
        {
            StartCoroutine(GreenToRed());
            isRed = true;
            isGreen = false;
        }
    }

    private void OnMouseDown()
    {
        //Change state of traffic light
        isGreen = !isGreen;
    }

    private void RedToGreen()
    {
        //Every material in black except green one
        greenLight.color = greenAlbedo;
        greenLight.SetColor("_SpecColor", greenSpecular);
        yellowLight.color = Color.black;
        yellowLight.SetColor("_SpecColor", Color.black);
        redLight.color = Color.black;
        redLight.SetColor("_SpecColor", Color.black);
    }

    IEnumerator GreenToRed()
    {
        //Activate only yellow material
        greenLight.color = Color.black;
        greenLight.SetColor("_SpecColor", Color.black);
        yellowLight.color = yellowAlbedo;
        yellowLight.SetColor("_SpecColor", yellowSpecular);

        yield return new WaitForSeconds(1);

        //Then activate only red material
        yellowLight.color = Color.black;
        yellowLight.SetColor("_SpecColor", Color.black);
        redLight.color = redAlbedo;
        redLight.SetColor("_SpecColor", redSpecular);
    }
}
