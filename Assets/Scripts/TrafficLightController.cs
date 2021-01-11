using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    private bool isGreen;
    private bool isRed;
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
        if (isGreen)
        {
            RedToGreen();
            isRed = false;
            isGreen = true;
        }
        else if (!isGreen && !isRed)
        {
            StartCoroutine(GreenToRed());
            isRed = true;
            isGreen = false;
        }
    }

    private void OnMouseDown()
    {
        isGreen = !isGreen;
    }

    private void RedToGreen()
    {
        greenLight.color = greenAlbedo;
        greenLight.SetColor("_SpecColor", greenSpecular);
        yellowLight.color = Color.black;
        yellowLight.SetColor("_SpecColor", Color.black);
        redLight.color = Color.black;
        redLight.SetColor("_SpecColor", Color.black);
    }

    IEnumerator GreenToRed()
    {
        greenLight.color = Color.black;
        greenLight.SetColor("_SpecColor", Color.black);
        yellowLight.color = yellowAlbedo;
        yellowLight.SetColor("_SpecColor", yellowSpecular);
        yield return new WaitForSeconds(1);
        yellowLight.color = Color.black;
        yellowLight.SetColor("_SpecColor", Color.black);
        redLight.color = redAlbedo;
        redLight.SetColor("_SpecColor", redSpecular);
    }
}
