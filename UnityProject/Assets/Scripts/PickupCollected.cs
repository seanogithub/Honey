using UnityEngine;
using System.Collections;

public class PickupCollected : MonoBehaviour {


	private GameObject pickUpObject;    // Reference to the player GameObject.
	private GameObject playerObject;    // Reference to the player GameObject.
	private PlayerStats changeScoreScript;    // Reference to the player GameObject.
//    private HashIDs hash;               // Reference to the HashIDs.
	
    void Awake ()
    {
        // Setting up the references.
//        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        pickUpObject = GameObject.FindGameObjectWithTag("PickUp");
		playerObject = GameObject.FindGameObjectWithTag("Player");
    }
	
	void OnTriggerEnter (Collider other)
    {
// is this a valid object for pick up
		if ( other.gameObject.tag == "PickUp" )
		{
			changeScoreScript = playerObject.GetComponent<PlayerStats>();
			changeScoreScript.SendMessage("addScore", 1);

			pickUpObject = other.gameObject;
			pickUpObject.SendMessage("PickUpDestroy", true);
		}
    }
}
