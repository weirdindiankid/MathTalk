using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class SpawnBalls : MonoBehaviour {

	// Use this for initialization

	public GameObject Sphere;
	public float force = 5.0f;
	private	Rigidbody rb;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var touch = Input.GetTouch (0);
		if (touch.phase == TouchPhase.Began)
		{
			GameObject obj = Instantiate(Sphere,transform.position,transform.rotation);
			obj.GetComponent<Rigidbody>().velocity= transform.forward * force;
		}
	}
}
