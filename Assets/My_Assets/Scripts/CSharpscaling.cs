using UnityEngine;
using System.Collections;

public class CSharpscaling : MonoBehaviour
{


    public float initialFingersDistance;
    public Vector3 initialScale;
    public Material initialMaterial;

    //static variable representing the transform of the object currently available for scaling; since it is static, only one object scaled at a time
    public static Transform ScaleTransform;


    void Update()
    {
        int fingersOnScreen = 0;

        foreach (Touch touch in Input.touches)
        {
            fingersOnScreen++; //Count fingers (or rather touches) on screen as you iterate through all screen touches.

		}

        if (fingersOnScreen == 0){

            if (!ScaleTransform.gameObject.GetComponent<SelectTracker>().isBeingTranslated && ScaleTransform.gameObject.GetComponent<SelectTracker>().isBeingScaled)
            {
                ScaleTransform.gameObject.GetComponent<SelectTracker>().deactivateHighlight();
            }

            ScaleTransform.gameObject.GetComponent<SelectTracker>().isBeingScaled = false;
            
        }
        //You need two fingers on screen to pinch.
        if (fingersOnScreen == 2)
        {

            //First set the initial distance between fingers so you can compare.
            if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                initialScale = ScaleTransform.localScale;

                ScaleTransform.gameObject.GetComponent<SelectTracker>().activateHighlight();

                ScaleTransform.gameObject.GetComponent<SelectTracker>().isBeingScaled = true;

            }
            else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
            {
                float currentFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

                float scaleFactor = currentFingersDistance / initialFingersDistance;

                ScaleTransform.localScale = initialScale * scaleFactor;

                ScaleTransform.gameObject.GetComponent<SelectTracker>().isBeingScaled = true;
            }
        }
    }
}
