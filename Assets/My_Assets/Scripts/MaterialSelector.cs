﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelector : MonoBehaviour {

    public SpawnRadioButtons.SpawnProperty[] OrderedProperties;
    public Sprite[] OrderedPropertyIcons; //must be in the same order corresponding to the order of properties specified in OrderedProperties
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
        for (int i = 0; i < OrderedProperties.Length; i += 1){
            float minSliderVal = (float)i / (float)OrderedProperties.Length;
            float maxSliderVal = (float)(i + 1) / (float)OrderedProperties.Length;
            if(minSliderVal<=materialSlider.value && materialSlider.value <=maxSliderVal){ //if the slider's value is currently set in this property's portion/zone
                if(SpawnRadioButtons.CurrentProperty != OrderedProperties[i]){ //if the current property is not equal to the one the slider is currently placed on
                    SpawnRadioButtons.CurrentProperty = OrderedProperties[i]; //set the current property equal to the one the slider is placed on
                    MaterialIcon.GetComponent<Image>().sprite = OrderedPropertyIcons[i]; 
                }
            }
        }



        if(System.Math.Abs(previousSliderValue - materialSlider.value) < 0.000001f)
        {
            timeSinceLastMove += Time.deltaTime;
        }
        else{
            timeSinceLastMove = 0f;
            MaterialIconBackground.SetActive(true);
        }

        if(timeSinceLastMove >= maxTimeBeforeIconFadeOut){
            MaterialIconBackground.SetActive(false);
        }

        previousSliderValue = materialSlider.value;


		
	}
}
