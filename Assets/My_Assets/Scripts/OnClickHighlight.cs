using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickHighlight : MonoBehaviour {

	private float HighlightDistance = 10000;
	public Shader Default;
	public Shader Highlight;
	public Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		Default = Shader.Find("Diffuse");
		Highlight = Shader.Find("Outlined Diffuse");
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, HighlightDistance) == this)
                {
                    rend.material.shader = Highlight;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                rend.material.shader = Default;
            }

        }
	}
}
