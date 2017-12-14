using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	public bool Paused = false;
	
	public bool PlayerIdleState = true;
	public bool PlayerWalkState = false;
	public bool PlayerDieState = false;
	public bool PlayerPickUpObjectState = false;
	public bool PlayerDropObjectState = false;
	public bool PlayerCarryObjectState = false;
	public bool PlayerPickUpBaddieState = false;
	public bool PlayerIdleCarryBaddieState = false;
	public bool PlayerWalkCarryBaddieState = false;
	public bool PlayerThrowBaddieState = false;
	public float timerPickupObject = 1.0f;
	public float timerDropObject = 1.0f;
	public float timerPickupBaddie = 1.0f;
	public float timerThrowBaddie = 1.0f;
	
	public string currentState = "Idle";	
	
	public Joystick moveJoystick;
	
    public float turnSmoothing = 5f;   // A smoothing value for turning the player.
    public float speedDampTime = 0.1f;  // The damping for the speed parameter

	private GameObject GameControllerObject;
	private GameController changeLevelScript;    // Reference to the player GameObject.
	private GameObject HUDObject;
	private GameObject LevelCompletedPopUp;
	private GameObject JoystickObject;
	private GameObject pickUpObject;    // Reference to the player GameObject.
	private GameObject pickUpObjectAvailable;    // Reference to the player GameObject.
	private GameObject[] pickUpObjects;    // Reference to the player GameObject.
	private GameObject RightButtonObject;
	public bool ButtonEnabled = true;
	public Pickup pickUpObjectScript;    // Reference to the player GameObject.
	private BaddieAI pickUpBaddieScript;    // Reference to the player GameObject.
    private Animator anim;              // Reference to the animator component.
    private Animation currentanim;              // Reference to the animator component.
    private HashIDs hash;               // Reference to the HashIDs.
	public bool pickUpAvailable;
	public string pickUpObjectType = "";
	private float myGravity = -0.00f;
	
    void Awake ()
    {
        // Setting up the references.
		GameControllerObject = GameObject.FindGameObjectWithTag("GameController");
		HUDObject = GameObject.FindGameObjectWithTag("HUD");
		JoystickObject = GameObject.FindGameObjectWithTag("joystick");
        anim = GetComponent<Animator>();
		currentanim = GetComponent<Animation>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        pickUpObject = GameObject.FindGameObjectWithTag("PickUp");
		pickUpObjectAvailable = GameObject.FindGameObjectWithTag("PickUp");
		RightButtonObject = GameObject.FindGameObjectWithTag("RightButton");
		myGravity = -0.1f;
    }

	public void Pause (bool myValue)
	{
		Paused = myValue;
		this.anim.enabled = !myValue;
	}
	
	void OnTriggerEnter (Collider other)
    {
		if (anim.GetBool(hash.PlayerCarryObjectState) == false && anim.GetBool(hash.PlayerPickUpBaddieState) == false)
		{
// is this a valid pickup object
			if ( other.gameObject.tag == "PickUp" )
			{
				pickUpAvailable = true;
				pickUpObjectAvailable = other.gameObject;
				pickUpObjectScript = other.GetComponent<Pickup>();
				pickUpObjectType = "PickUp";
			}
			else
			{
				pickUpAvailable = false;
				pickUpObjectAvailable = null;
				pickUpObjectScript = null;
				pickUpObjectType = "";
			}	
// is this a valid baddie object		
			if ( other.gameObject.tag == "Baddie" && pickUpAvailable == false)
			{
				pickUpAvailable = true;
				pickUpObjectAvailable = other.gameObject;
				pickUpBaddieScript = other.GetComponent<BaddieAI>();
				pickUpObjectType = "Baddie";
			}	
			else
			{
				pickUpAvailable = false;
				pickUpObjectAvailable = null;
				pickUpBaddieScript = null;
				pickUpObjectType = "";
			}	
		}
    }

	void OnTriggerStay (Collider other)
    {
		if (anim.GetBool(hash.PlayerCarryObjectState) == false && anim.GetBool(hash.PlayerPickUpBaddieState) == false)
		{
			if ( other.gameObject.tag == "PickUp" )
			{
				pickUpAvailable = true;
				pickUpObjectAvailable = other.gameObject;
				pickUpObjectScript = other.GetComponent<Pickup>();
				pickUpObjectType = "PickUp";
			}
			if ( other.gameObject.tag == "Baddie" )
			{
				pickUpAvailable = true;
				pickUpObjectAvailable = other.gameObject;
				pickUpBaddieScript = other.GetComponent<BaddieAI>();
				pickUpObjectType = "Baddie";
			}			
		}		
// is this a valid object for pick up
		
    }
	
	void OnTriggerExit (Collider other)
    {
		if (anim.GetBool(hash.PlayerCarryObjectState) == false && anim.GetBool(hash.PlayerPickUpBaddieState) == false)
		{
			pickUpAvailable = false;
			pickUpObjectAvailable = null;
			pickUpObjectType = "";
		}
/*
		if (anim.GetBool(hash.dropObjectBool) == true) // || anim.GetBool(hash.pickUpBaddieBool) == false)
		{
			pickUpAvailable = false;
			pickUpObjectAvailable = null;
			pickUpObjectType = "";
		}
*/
    }

	void OnCollisionEnter (Collision other)
    {
		if ( other.gameObject.tag == "Terrain")
		{
			myGravity = -0.00f;
		}
    }

	void OnCollisionExit (Collision other)
    {
		if ( other.gameObject.tag == "Terrain")
		{
			myGravity = -0.1f;
		}
    }

	
	void CheckForDeath()
	{
// check for death		
		if (this.GetComponent<PlayerStats>().playerHealth <= 0)
		{
			Paused = true;
			var temp = Resources.Load("UI/HUD/PopUp_Level_Failed_Prefab",typeof(GameObject)) as GameObject;
			LevelCompletedPopUp = GameObject.Instantiate(temp, new Vector3 (0.5f,0.75f,0), Quaternion.identity ) as GameObject;		

//			changeLevelScript = GameControllerObject.GetComponent<GameController>();
//			changeLevelScript.SendMessage("ReloadLevel");				
		}
		if (this.gameObject.transform.position.y < -10.0f)
		{
			Paused = true;
			var temp = Resources.Load("UI/HUD/PopUp_Level_Failed_Prefab",typeof(GameObject)) as GameObject;
			LevelCompletedPopUp = GameObject.Instantiate(temp, new Vector3 (0.5f,0.75f,0), Quaternion.identity ) as GameObject;		

//			changeLevelScript = GameControllerObject.GetComponent<GameController>();
//			changeLevelScript.SendMessage("ReloadLevel");
		}			
	}
	
	void ErrorCheckForCarryingObjects()
	{
// error check to see if carryobject is false and anim is a carrying animation	
		if (anim.GetBool(hash.PlayerCarryObjectState) == false && this.anim.GetCurrentAnimatorStateInfo(0).nameHash.ToString() == "1701690346")
		{
			anim.SetBool(hash.PlayerDropObjectState, true);
			currentState = "DropObject";
		}
		if (anim.GetBool(hash.PlayerCarryObjectState) == false && this.anim.GetCurrentAnimatorStateInfo(0).nameHash.ToString() == "1185035427")
		{
			anim.SetBool(hash.PlayerDropObjectState, true);
			currentState = "DropObject";
		}		

		if (anim.GetBool(hash.PlayerPickUpBaddieState) == false && this.anim.GetCurrentAnimatorStateInfo(0).nameHash.ToString() == "1701690346")
		{
			anim.SetBool(hash.PlayerThrowBaddieState, true);
			currentState = "ThrowBaddie";
		}
		if (anim.GetBool(hash.PlayerPickUpBaddieState) == false && this.anim.GetCurrentAnimatorStateInfo(0).nameHash.ToString() == "1185035427")
		{
			anim.SetBool(hash.PlayerThrowBaddieState, true);
			currentState = "ThrowBaddie";
		}			
	}
	
	void ButtonActions()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE
			bool pickUpDropButton = Input.GetButtonDown("Jump");
		#elif UNITY_ANDROID || UNITY_IPHONE
			bool pickUpDropButton = RightButtonObject.GetComponent<Button>().ButtonPressed;		
		#endif		
		
		if (pickUpDropButton == true && ButtonEnabled == true)
		{
// drop object			
			if (anim.GetBool(hash.PlayerCarryObjectState) == true || anim.GetBool(hash.PlayerPickUpBaddieState) == true)
			{	
				switch(pickUpObjectType)
				{
				case("PickUp"):
					if ( anim.GetBool(hash.PlayerCarryObjectState) == true)
					{
						anim.SetBool(hash.PlayerDropObjectState, true);
						anim.SetBool(hash.PlayerCarryObjectState, false);
						pickUpObjectAvailable.GetComponent<Pickup>().PickUp(false);
						pickUpAvailable = false;
						pickUpObjectAvailable = null;
						pickUpObjectType = "";
						currentState = "DropObject";
					}
					break;
				case("Baddie"):
					if ( anim.GetBool(hash.PlayerPickUpBaddieState) == true)
					{
						anim.SetBool(hash.PlayerThrowBaddieState, true);
						anim.SetBool(hash.PlayerPickUpBaddieState, false);
						pickUpObjectAvailable.GetComponent<BaddieAI>().Throw(true);
						pickUpAvailable = false;
						pickUpObjectAvailable = null;
						pickUpObjectType = "";	
						currentState = "ThrowBaddie";
					}
					break;
				}	
			}
			
// pick up object
			else
			{
				if (pickUpAvailable == true)
				{
					switch(pickUpObjectType)
					{
					case("PickUp"):
						if(anim.GetBool(hash.PlayerCarryObjectState) == false && pickUpObjectScript != null)
						{
							anim.SetBool(hash.PlayerCarryObjectState, true);
							anim.SetBool(hash.PlayerDropObjectState, false);
							pickUpObjectScript.SendMessage("PickUp",true);
							currentState = "PickUpObject";
						}
						break;
					case("Baddie"):
						if(anim.GetBool(hash.PlayerPickUpBaddieState) == false && pickUpBaddieScript != null)
						{
							anim.SetBool(hash.PlayerPickUpBaddieState, true);
							anim.SetBool(hash.PlayerThrowBaddieState, false);
							pickUpBaddieScript.SendMessage("PickUp",true);
							currentState = "PickUpBaddie";
						}
						break;
					}
				}				
			}
		}		
	}

	void SetAnimationState(string myState)
	{
		PlayerIdleState = false;
		PlayerWalkState = false;
		PlayerDieState = false;
		PlayerPickUpObjectState = false;
		PlayerDropObjectState = false;
		PlayerCarryObjectState = false;
		PlayerPickUpBaddieState = false;
		PlayerIdleCarryBaddieState = false;
		PlayerWalkCarryBaddieState = false;
		PlayerThrowBaddieState = false;
		
		switch(currentState)
		{
			case("Idle"):
//				PlayerIdleState = true;
			break;
			case("Walk"):
//				PlayerWalkState = true;
			break;
			case("Die"):
				PlayerDieState = true;
			break;
			case("PickUpObject"):
				PlayerPickUpObjectState = true;
			break;
			case("DropObject"):
				PlayerDropObjectState = true;
			break;
			case("PlayerCarryObject"):
				PlayerCarryObjectState = true;
			break;
			case("PickUpBaddie"):
				PlayerPickUpBaddieState = true;
			break;
			case("PlayerIdleCarryBaddie"):
				PlayerIdleCarryBaddieState = true;
			break;
			case("PlayerWalkCarryBaddie"):
				PlayerWalkCarryBaddieState = true;
			break;
			case("ThrowBaddie"):
				PlayerThrowBaddieState = true;
			break;
		}
		
//		anim.SetBool(hash.PlayerIdleState, PlayerIdleState);
//		anim.SetBool(hash.PlayerWalkState, PlayerWalkState);
//		anim.SetBool(hash.PlayerDieState, PlayerDieState);
		anim.SetBool(hash.PlayerPickUpObjectState, PlayerPickUpObjectState);		
		anim.SetBool(hash.PlayerDropObjectState, PlayerDropObjectState);		
		anim.SetBool(hash.PlayerCarryObjectState, PlayerCarryObjectState);			
		anim.SetBool(hash.PlayerPickUpBaddieState, PlayerPickUpBaddieState);		
		anim.SetBool(hash.PlayerIdleCarryBaddieState, PlayerIdleCarryBaddieState);		
		anim.SetBool(hash.PlayerWalkCarryBaddieState, PlayerWalkCarryBaddieState);			
		anim.SetBool(hash.PlayerThrowBaddieState, PlayerThrowBaddieState);			
	}
	
	void UpdateState()
	{
// switch states		
		switch(currentState)
		{
			case("Idle"):
			ButtonEnabled = true;
			break;
			case("Walk"):
			ButtonEnabled = true;
			break;
			case("Die"):
			ButtonEnabled = false;
			break;
			case("PickUpObject"):
			ButtonEnabled = false;
			timerPickupObject -= Time.deltaTime;
			if(timerPickupObject < 0)
			{
				timerPickupObject = 1.0f;
				currentState = "CarryIdle";
				ButtonEnabled = true;
			}			
			break;
			case("DropObject"):
			ButtonEnabled = false;
			timerDropObject -= Time.deltaTime;
			if(timerDropObject < 0)
			{
				timerDropObject = 1.0f;
				currentState = "Idle";
				ButtonEnabled = true;
			}			
			break;
			case("CarryIdle"):
			ButtonEnabled = true;
			break;
			case("CarryWalk"):
			ButtonEnabled = true;
			break;
			case("PickUpBaddie"):
			ButtonEnabled = false;
			timerPickupBaddie -= Time.deltaTime;
			if(timerPickupBaddie < 0)
			{
				timerPickupBaddie = 1.0f;
				currentState = "CarryIdleBaddie";
				ButtonEnabled = true;
			}
			break;
			case("CarryIdleBaddie"):
			ButtonEnabled = true;
			break;
			case("CarryWalkBaddie"):
			ButtonEnabled = true;
			break;
			case("ThrowBaddie"):
			ButtonEnabled = false;
			ButtonEnabled = false;
			timerThrowBaddie -= Time.deltaTime;
			if(timerThrowBaddie < 0)
			{
				timerThrowBaddie = 1.0f;
				currentState = "Idle";
				ButtonEnabled = true;
			}			
			break;			
		}			
	}
	void FixedUpdate ()
    {
		if(Paused == false)
		{
			CheckForDeath();		
		
//			ErrorCheckForCarryingObjects();
			
			ButtonActions();		
			
			UpdateState();
			
//			SetAnimationState(currentState);
	
			// Cache the inputs.
			#if UNITY_EDITOR || UNITY_STANDALONE
		        float h = Input.GetAxis("Horizontal");
		        float v = Input.GetAxis("Vertical");		
			#elif UNITY_ANDROID || UNITY_IPHONE
				float h = JoystickObject.GetComponent<Joystick>().position.x;		
				float v = JoystickObject.GetComponent<Joystick>().position.y;		
			#endif
					
			// need to round the joystick input for some reason???
			v = Mathf.Round(v*1000) / 1000;
			h = Mathf.Round(h*1000) / 1000;
	        MovementManagement(h, v);
			
			// reset button
			HUDObject.GetComponent<HUD>().ButtonPressed = false;
		}
    }
    
    void MovementManagement (float horizontal, float vertical)
    {
		
//		anim.SetBool(hash.carryObjectBool, carryObject);
		
        // If there is some axis input...
        if(horizontal != 0f || vertical != 0f)
        {
            // ... set the players rotation and set the speed parameter to 5.5f.
            Rotating(horizontal, vertical);
            anim.SetFloat(hash.speedFloat, 1.0f, speedDampTime, Time.deltaTime);
        }
        else
		{	
            // Otherwise set the speed parameter to 0.
            anim.SetFloat(hash.speedFloat, 0);
			if (myGravity != 0f)
			{
				Vector3 playerGravity = new Vector3(0f, myGravity, 0f) + this.transform.position;
				GetComponent<Rigidbody>().MovePosition(playerGravity);
			}
		}

    }
    
    
    void Rotating (float horizontal, float vertical)
    {
        // Create a new vector of the horizontal and vertical inputs.
        Vector3 targetDirection = new Vector3(horizontal, 0.0f, vertical);
        
        // Create a rotation based on this new vector assuming that up is the global y axis.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        
        // Create a rotation that is an increment closer to the target rotation from the player's rotation.
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        
        // Change the players rotation to this new rotation.
        GetComponent<Rigidbody>().MoveRotation(newRotation);
		
		//add gravity
		if ( myGravity != 0f)
		{
			Vector3 playerGravity = new Vector3(0f, myGravity, 0f) + this.transform.position;
			GetComponent<Rigidbody>().MovePosition(playerGravity);
		}
    }
    
}
