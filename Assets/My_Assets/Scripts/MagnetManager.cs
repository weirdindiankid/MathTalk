using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour {

    //When an object is created with a magnetic property, it is added to this list. In the update function, each object in
    //this list is magnetically attracted to every other object in the list. Deleting an object from this list will make it
    //not magnetic
    public List<GameObject> magnets;

	// Use this for initialization
	void Start () {
        magnets = new List<GameObject>();
		
	}
	

    // Update is called once per frame
	void Update () {
        foreach (GameObject magnet1 in magnets){
            if (magnet1 != null){
                foreach (GameObject magnet2 in magnets){
                    if (magnet2 != null && magnet1.GetInstanceID() != magnet2.GetInstanceID()){ //magnetic force from one magnet only applied to magnets that are not null (destroyed) and are not this magnet itself
                        //vector force points in direction from magnet1 to magnet2
                        Vector3 magnetForce = new Vector3((magnet2.transform.position.x - magnet1.transform.position.x),
                                                          (magnet2.transform.position.y - magnet1.transform.position.y), 
                         
                                                          (magnet2.transform.position.z - magnet1.transform.position.z));
                        float magnetDistance = Mathf.Abs(Vector3.Distance(magnet2.transform.position, magnet1.transform.position));

                        //Uncomment this line to have a magnet's force correlate with its scale. There are currently some bugs with this feature
                        //float magnetForceStrength = 1f * (magnet2.transform.localScale.x/0.05f) ;

                        //as of right now, all magnetic forces are normalized to 1N. Change this value to do otherwise
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
