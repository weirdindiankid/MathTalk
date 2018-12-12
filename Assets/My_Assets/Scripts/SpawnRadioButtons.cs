using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class SpawnRadioButtons : MonoBehaviour
{


    //public int RadioInt = 3;

    public enum SpawnProperty { Magnet, Rubber, Paper, Default }; //have this be changed by the property slider. When a new object is created, it gets the property that this is currently assigned to
    //when object is created, have a case statement that gives it different properties depending on this enum value

    static public SpawnProperty CurrentProperty; //controlled by materialSelection script

    public MagnetManager magnetManager;
    public PhysicMaterial magneticMaterial; //defined in the editor
    public PhysicMaterial defaultMaterial;
    public PhysicMaterial rubberMaterial;
    public PhysicMaterial paperMaterial;

    private GameObject SpawnObj;
    public GameObject CubeObj;
    public GameObject SphereObj;
    public GameObject CanObj;
    public Vector3 StartPos; //coordinate on mousedown
    public Vector3 EndPos; // coordinate on mouseup
    //public string[] RadioStrings = new string[] { "Cubes", "Spheres", "Cones" };

    public float maxRayDistance = 1000.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

    private Color[] ObjColors = new Color[3];
    private List<GameObject> CreatedObjs = new List<GameObject>();

    void Start()
    {
        // Will clean this up to make it more modular
        ObjColors[0] = new Color(0.8431373F, 0.2F, 0.1215686F); // Red
        ObjColors[1] = new Color(0.2627451F, 0.7058824F, 1F); // Blue
        ObjColors[2] = new Color(0.9647059F, 0.7843137F, 0.3098039F); // Yellow

    }


    bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes, GameObject obj)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                obj.transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                obj.transform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", obj.transform.position.x, obj.transform.position.y, obj.transform.position.z));
                return true;
            }
        }
        return false;
    }


    public void Cube()
    {
        SpawnObj = CubeObj;
    }

    public void Sphere()
    {
        SpawnObj = SphereObj;
    }

    public void Can()
    {
        SpawnObj = CanObj;
    }

    public void Delete() // Deletes all objects in entire scene --> Not currently implemented, instead delete objects by deleting
    //plane, or delete just one by dragging it off the plane
    {
        if (CreatedObjs.Count > 0)
        {
            foreach (GameObject del in CreatedObjs)
            {
                Destroy(del);
            }
        }
    }
    void Update()
    {
        if (Input.touchCount == 1) //object not being scaled, either creating or moving an object
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartPos = Camera.main.ScreenToViewportPoint(touch.position);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                EndPos = Camera.main.ScreenToViewportPoint(touch.position);
                var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);

                Ray ray = Camera.main.ScreenPointToRay(new Vector2(screenPosition.x * Camera.main.pixelWidth, screenPosition.y * Camera.main.pixelHeight));
                RaycastHit hit;
                float dist;
                dist = Vector3.Distance(StartPos, EndPos); //distance between coordinate of where finger is put down vs. lifted off screen
                if (dist == 0) //if object being moved and not creating a new one 
                {
                    if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayer))
                    {
                        GameObject obj = Instantiate(SpawnObj, hit.point, hit.transform.rotation);
                        CreatedObjs.Add(obj); // Adds object to list for easy deletion

                        //depending on the currentProperty value, give this newly created object specific components
                        switch (CurrentProperty)
                        {
                            case SpawnProperty.Magnet:
                                magnetManager.magnets.Add(obj);
                                obj.GetComponent<SelectTracker>().normalColor = new Color(0.5f, 0.5f, 0.5f); //magents automatically have a color of gray 
                                obj.GetComponent<SelectTracker>().deactivateHighlight(); //start with the highlight deactivated
                                obj.GetComponent<Collider>().material = magneticMaterial; 
                                break;
                            case SpawnProperty.Rubber:
                                obj.GetComponent<Collider>().material = rubberMaterial;
                                break;
                            case SpawnProperty.Paper:
                                obj.GetComponent<Collider>().material = paperMaterial;
                                obj.GetComponent<Rigidbody>().drag = 13f; //gives the paper a slow falling effect
                                break;
                            case SpawnProperty.Default:
                                obj.GetComponent<Collider>().material = defaultMaterial;
                                break;
                        }

                        // Uncomment these lines for random color changing.
                        //Renderer rend = obj.GetComponent<Renderer>();
                        //rend.material.shader = Shader.Find("_Color");
                        //rend.material.SetColor("_Color", ObjColors[Random.Range(0,ObjColors.Length)]);

                        //we're going to get the position from the contact point
                        obj.transform.position = hit.point;
                        Debug.Log(string.Format("Normal spawn: x:{0:0.######} y:{1:0.######} z:{2:0.######}", obj.transform.position.x, obj.transform.position.y, obj.transform.position.z));
                        Debug.Log(string.Format("Screen Spawn area: x:{0:0.######} y:{1:0.######}", screenPosition.x, screenPosition.y));
                        Debug.Log(string.Format("Middle of Screen: x:{0:0.######} y:{1:0.######}", Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
                        //and the rotation from the transform of the plane collider
                        obj.transform.rotation = hit.transform.rotation;
                    }


                    //This code was commented out when we received the project in september, so we are just gonna leave it :)
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
        }

        // Update is called once per frame

    }
}
