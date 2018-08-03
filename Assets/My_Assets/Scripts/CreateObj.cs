using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class CreateObj : MonoBehaviour {

	// Use this for initialization
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer


bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes, GameObject obj)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
                    obj.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    obj.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", obj.transform.position.x, obj.transform.position.y, obj.transform.position.z));
                    return true;
                }
            }
            return false;
        }

	public void ClickEvent (GameObject CubeObj) 
	{
		Ray ray = Camera.main.ScreenPointToRay (new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0)); //this Vector3 fires a raycast at the center of the camera
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
		
		/* 
		GameObject obj = Instantiate(CubeObj, new Vector3(0F, 0F, 0F), CubeObj.transform.rotation);
		var screenPosition = Camera.main.ScreenToViewportPoint(new Vector2(0.5F, 0.5F)); // this Vector2 points at the center of the camera
				ARPoint point = new ARPoint {
					x = screenPosition.x,
					y = screenPosition.y
				};

				// prioritize reults types
				ARHitTestResultType[] resultTypes = {
					//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
					// if you want to use infinite planes use this:
					//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
					//ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
					//ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
					//ARHitTestResultType.ARHitTestResultTypeFeaturePoint
				}; 
				
				foreach (ARHitTestResultType resultType in resultTypes)
				{
					if (HitTestWithResultType (point, resultType, obj))
					{
						return;
					}
				}
				*/

				
		
		

	}

}
