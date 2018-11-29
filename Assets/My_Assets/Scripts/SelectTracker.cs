using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTracker : MonoBehaviour {

	public bool isSelected;
    public bool isBeingScaled;
    public bool isBeingTranslated;

    public Color normalColor;
    public Color selectedColor;

	// Use this for initialization
	void Start () {
		isSelected = false;
        isBeingScaled = false;
        isBeingTranslated = false;
        deactivateHighlight();
	}

    // Update is called once per frame
    void Update()
    {
        isSelected = (isBeingScaled || isBeingTranslated);
	}

    public void activateHighlight(){
        Material newMaterial = GetComponent<Renderer>().material;
        newMaterial.color = selectedColor;
        GetComponent<Renderer>().material = newMaterial;
        FreezeRotation();
    }

    public void deactivateHighlight(){
        Material newMaterial = GetComponent<Renderer>().material;
        newMaterial.color = normalColor;
        GetComponent<Renderer>().material = newMaterial;
        ContinueRotation();
    }

	private void FreezeRotation(){
        GetComponent<Rigidbody>().freezeRotation = true;
	}

    private void ContinueRotation(){
        GetComponent<Rigidbody>().freezeRotation = false;
        
    }

}
