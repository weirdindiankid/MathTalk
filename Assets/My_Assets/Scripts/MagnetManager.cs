using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour {
    
    public List<GameObject> magnets;

	// Use this for initialization
	void Start () {
        magnets = new List<GameObject>();
		
	}
	

    //SOMEHOW MAKE THIS ONLY APPLY TO OBJECTS ON THE SAME PLANE
    //MAKE THE MAGNETIC FORCE DEPEND ON THE SCALE OF THE OBJECT (i.e. bigger objects give off a bigger pull)
    //MAKE THE MAGENTIC FORCE GET WEAKER WITH DISTANCE (currently it's the opposite)
    //CUBES NOT EXPERIENCING THE FORCE BECAUSE OF THE FRICTION OF THEIR MATERIAL ON FLAT SURFACE
    //CONSTANT SHAKING IN PLACE ONCE THE MAGNETS COLLIDE
    //SOMETIMES OBJECTS CURL AROUND UNDER THE PLANE WHEN THEY FALL OFF DUE TO MAGENTIC FORCE FROM OBJECTS ON THE PLANE
	//DELETE OBJECTS FROM MAGNETS LIST ONCE THEY ARE DEstroyed
    // Update is called once per frame
	void Update () {
        foreach (GameObject magnet1 in magnets){
            if (magnet1 != null){
                foreach (GameObject magnet2 in magnets){
                    if (magnet2 != null && magnet1.GetInstanceID() != magnet2.GetInstanceID()){ //magnetic force from one magnet only applied to magnets that are not null (destroyed) and are not this magnet itself
                        Vector3 magnetForce = new Vector3((magnet2.transform.position.x - magnet1.transform.position.x),
                                                          (magnet2.transform.position.y - magnet1.transform.position.y), 
                                                          (magnet2.transform.position.z - magnet1.transform.position.z));
                        float magnetDistance = Mathf.Abs(Vector3.Distance(magnet2.transform.position, magnet1.transform.position));

                        //Uncomment this line to have a magnet's force correlate with its scale. There are currently some bugs with this feature
                        //float magnetForceStrength = 1f * (magnet2.transform.localScale.x/0.05f) ;

                        float magnetForceStrength = 1f;
                        Vector3 finalMagneticForce = new Vector3((magnetForceStrength * magnetForce.x / magnetDistance),
                                                                 (magnetForceStrength * magnetForce.y / magnetDistance),
                                                                 (magnetForceStrength * magnetForce.z / magnetDistance));
                        magnet1.GetComponent<Rigidbody>().AddForce(finalMagneticForce);
                    }
                }
            }
        }
		
	}
}
