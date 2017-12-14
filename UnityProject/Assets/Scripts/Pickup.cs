using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

//    private Animator anim;                      // Reference to the animator component.
    private GameObject player;                  // Reference to the player GameObject.
	public bool pickedUp;
	public bool wasPickedup;
	private bool scored;
	private float PickUpLerpTimer = 0.5f;
	public int BonusPoints = 0;
	public float MinDistanceForBonusPoints = 100.0f;
	private GameObject[] DropOffObjects;    // Reference to the player GameObject.
	
    void Awake ()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag(Tags.player);
		pickedUp = false;
		wasPickedup = false;
		scored = false;
    }

	void Start ()
	{
		CalculateBonus();
	}
	
	public void PickUp (bool myValue)
	{
		pickedUp = myValue;
		this.GetComponent<CapsuleCollider>().enabled = myValue;
	}

	void PickUpDestroy (bool myValue)
	{
		scored = true;
		this.GetComponent<CapsuleCollider>().enabled = false;
		player.GetComponent<PlayerStats>().addHoneyPoints(BonusPoints);
		Destroy(this.gameObject, 3.0f);
	}
	
	void CalculateBonus()
	{
		DropOffObjects = GameObject.FindGameObjectsWithTag("DropOff");
		var MinDist = MinDistanceForBonusPoints;
		var CurrentDistance = 0.0f;
		
		for (var i = 0; i < DropOffObjects.Length; i++)
		{
			CurrentDistance = Vector3.Distance(	DropOffObjects[i].transform.position, this.transform.position);
			if (CurrentDistance < MinDist)
			{
				MinDist = CurrentDistance;
			}
		}		
		BonusPoints = Mathf.RoundToInt(MinDist);		
	}
	
    void FixedUpdate ()
    {
		if (scored == false) 
		{
			if (pickedUp == true)
			{
				wasPickedup = true;
				this.GetComponent<CapsuleCollider>().enabled = false;

				PickUpLerpTimer -=  Time.deltaTime;
				if (PickUpLerpTimer <= 0)
				{
					PickUpLerpTimer = 0;
				}
				if (PickUpLerpTimer == 0)
				{
					this.transform.position = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.position;
					this.transform.rotation = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.rotation;
				}
				// lerp to position to be picked up.
				else
				{
					this.transform.position = Vector3.Lerp( this.transform.position, player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.position ,10.0f * Time.deltaTime );
					this.transform.rotation = Quaternion.Lerp(this.transform.rotation, player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.rotation, 10.0f * Time.deltaTime );
				}
			}
			else
			{
				this.GetComponent<CapsuleCollider>().enabled = true;
				PickUpLerpTimer = 0.5f;
			}

			if(wasPickedup == true && pickedUp == false)
			{
				wasPickedup = false;
				if(this.GetComponent<Rigidbody>() != null && player != null)
				{
					GetComponent<Rigidbody>().AddRelativeForce (Vector3.forward * 1500);
//					Vector3 direction = transform.position - player.transform.position;
//				 	this.rigidbody.AddForceAtPosition(direction.normalized, transform.position);
				}
			}	
			
			
		}
		
// destroy object if it falls below the ground

		if (this.gameObject.transform.position.y < -10.0f)
		{
			print ("destroyed");
			Destroy(this.gameObject);
		}		
	}
}
