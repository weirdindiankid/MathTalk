using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class CreateObj : MonoBehaviour {

	// Use this for initialization
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer



	public void ClickEvent (GameObject CubeObj) {
		Ray ray = Camera.main.ScreenPointToRay (new Vector3(0.5F, 0.5F, 0)); //this Vector3 fires a raycast at the center of the camera
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) 
		{
			GameObject obj = Instantiate(CubeObj, hit.point, hit.transform.rotation);
			//we're going to get the position from the contact point
			obj.transform.position = hit.point;
			//Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", obj.transform.position.x, obj.transform.position.y, obj.transform.position.z));

			//and the rotation from the transform of the plane collider
			obj.transform.rotation = hit.transform.rotation;
		}		
		
		

	}

}
