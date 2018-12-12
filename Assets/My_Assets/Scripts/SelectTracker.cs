using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTracker : MonoBehaviour {

	public bool isSelected;
    public bool isBeingScaled;
    public bool isBeingTranslated;

    public Color normalColor; //specified in editor; color of this object when it is not being translated or scaled
    public Color selectedColor; //specified in editor; color of this object when it is being " "

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

    //important to create new unique material when highlighting/dehighlighting, or else altering one object's material changes all of them
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

    //called on an object when it is selected; this was to fix the bug that caused objects to rotate infinitely when knocking into something during translation
	private void FreezeRotation(){
        GetComponent<Rigidbody>().freezeRotation = true;
	}

    private void ContinueRotation(){
        GetComponent<Rigidbody>().freezeRotation = false;
        
    }

}
