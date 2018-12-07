using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelector : MonoBehaviour {

    public SpawnRadioButtons.SpawnProperty[] OrderedProperties; //specify this in the Unity interface. The order of SpawnProperties in this
    //list represents the order their regions will be on the slider
    public Sprite[] OrderedPropertyIcons; //order of material icons must correspond to the order they were listed in OrderedProperties
    // Use this for initialization
    public GameObject MaterialIconBackground;
    public GameObject MaterialIcon;
    public float maxTimeBeforeIconFadeOut;

    private Slider materialSlider;
    private float propertyRange;

    private float timeSinceLastMove;
    private float previousSliderValue;


	void Start () {
        materialSlider = GetComponent<Slider>();
        propertyRange = 1f / (float)OrderedProperties.Length; //the length (on a scale from 0 to 1) that each property occupies on the slider

        timeSinceLastMove = 0f;

        previousSliderValue = materialSlider.value;

        MaterialIcon.GetComponent<Image>().sprite = OrderedPropertyIcons[0];

	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < OrderedProperties.Length; i += 1){ //loops over each of the material properties that need to be on the slider
            float minSliderVal = (float)i / (float)OrderedProperties.Length;
            float maxSliderVal = (float)(i + 1) / (float)OrderedProperties.Length;
            if(minSliderVal<=materialSlider.value && materialSlider.value <=maxSliderVal){ //if the slider's value is currently set in this property's portion/zone
                if(SpawnRadioButtons.CurrentProperty != OrderedProperties[i]){ //if the current property is not equal to the one the slider is currently placed on
                    SpawnRadioButtons.CurrentProperty = OrderedProperties[i]; //set the current property equal to the one the slider is placed on
                    MaterialIcon.GetComponent<Image>().sprite = OrderedPropertyIcons[i]; //update the icon on the slider to this material's icon
                }
            }
        }


        //increment time since last move only if the position of slider has not changed since last frame
        if(System.Math.Abs(previousSliderValue - materialSlider.value) < 0.000001f)
        {
            timeSinceLastMove += Time.deltaTime;
        }
        //if position of slider has changed, time since last move is 0 and the icon is shown
        else{
            timeSinceLastMove = 0f;
            MaterialIconBackground.SetActive(true);
        }

        //max icon fade out represents how long it takes for the slider staying still for the icon to disappear
        if(timeSinceLastMove >= maxTimeBeforeIconFadeOut){
            MaterialIconBackground.SetActive(false);
        }

        previousSliderValue = materialSlider.value;


		
	}
}
