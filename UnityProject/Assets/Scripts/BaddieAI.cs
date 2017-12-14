using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BaddieAI : MonoBehaviour {
	
	public bool Paused = false;
	public bool IdleState = true;
	public bool RunState = false;
	public bool AttackState = false;
	public bool DieState = false;
	public bool AlertState = false;
	public bool PickUpState = false;
	public bool CarryIdleState = false;
	public bool CarryWalkState = false;
	public bool ThrowState = false;
	public bool FlyState = false;
	public bool LandState = false;
	
	public string currentState = "Idle";
	
	private Animator anim;              // Reference to the animator component.
    private HashIDs hash;               // Reference to the HashIDs.
	private GameObject player;    // Reference to the player GameObject.
	private PlayerStats changeHealthScript;    // Reference to the player GameObject.

	public float RotationSpeed;
	private Quaternion lookRotation;
	private Vector3 direction;
	public float alertDistance = 8.0f;
	public float visionDistance = 5.0f;	
	private float distanceToPlayer;
		
	private float timerAttack = 0.6666f; // should be the the length of the attack animation
	private float timerIdleMove = 0.6666f; // should me the length of the run animation
	private float timerAlert = 3.0f; // should me the length of the run animation
	private float timerThrow = 0.6666f; // should me the length of the run animation
	public float setMinIdleMoveTime = 5;
	public float setMaxIdleMoveTime = 10;
	private float maxIdleMoveTime = 10;
	private Vector3 idleMoveTarget;
	private float myGravity = 0.00f;
	private float ThrowForce = 2000.0f;
	private float SpinForceX = 0f;
	private float SpinForceZ = 0f;
	public float MaxSpinForce = 0.5f;
	public float PickUpLerpTimer = 0.5f;
	public int AttackDamage = 1;
	
	private float SoundTimer;
	public AudioClip SoundIdle1;
	public AudioClip SoundIdle2;
	public AudioClip SoundIdle3;
	public AudioClip SoundIdle4;
	public AudioClip SoundIdle5;
	public AudioClip SoundIdle6;	
	public AudioClip SoundRun1;
	public AudioClip SoundRun2;
	public AudioClip SoundRun3;
	public AudioClip SoundRun4;
	public AudioClip SoundRun5;
	public AudioClip SoundAttack1;
	public AudioClip SoundAttack2;
	public AudioClip SoundAttack3;

	
	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		maxIdleMoveTime = Random.Range(setMinIdleMoveTime,setMaxIdleMoveTime); 
		myGravity = -0.1f;
	}
	
	public void Pause (bool myValue)
	{
		Paused = myValue;
		this.anim.enabled = !myValue;
	}
	
	void OnTriggerStay (Collider other)
    {
		// player is carrying and trigger is colliding with player
		if ( other.gameObject.tag == "Player" && (player.GetComponent<Animator>().GetBool(hash.PlayerCarryObjectState) == true) )
		{
			if (currentState != "Fly" && currentState != "Land")
			{
				currentState = "Attack";
			}
		}
    }

	void OnCollisionStay (Collision other)
    {
		// player runs into the baddie then attack 
		if ( other.gameObject.tag == "Player")
		{
			if (currentState != "Fly" && currentState != "Land")
			{
				currentState = "Attack";
			}
//			PlaySound("Run");
		}
    }
	
	void OnCollisionEnter (Collision other)
    {
		if ( other.gameObject.tag == "Terrain")
		{
			myGravity = 0.00f;
			if(currentState == "Fly")
			{
				currentState = "Land";
			}
		}
    }

	void OnCollisionExit (Collision other)
    {
		if ( other.gameObject.tag == "Terrain")
		{
			myGravity = -0.1f;
		}
    }
	
	public void PickUp (bool myValue)
	{
		currentState = "PickUp";
		this.GetComponent<SphereCollider>().enabled = !myValue;
		this.GetComponent<CapsuleCollider>().enabled = !myValue;
		this.FixedUpdate();
	}	

	public void CarryIdle (bool myValue)
	{
		currentState = "CarryIdle";
		this.GetComponent<SphereCollider>().enabled = myValue;
		this.GetComponent<CapsuleCollider>().enabled = myValue;
		this.FixedUpdate();
	}	

	public void CarryWalk (bool myValue)
	{
		currentState = "CarryWalk";
		this.GetComponent<SphereCollider>().enabled = myValue;
		this.GetComponent<CapsuleCollider>().enabled = myValue;
		this.FixedUpdate();
	}	

	public void Throw (bool myValue)
	{
		currentState = "Throw";
		this.GetComponent<SphereCollider>().enabled = !myValue;
		this.GetComponent<CapsuleCollider>().enabled = !myValue;
		this.FixedUpdate();
	}	
	
	void SubtractHealth()
	{
		changeHealthScript = player.GetComponent<PlayerStats>();
		changeHealthScript.SendMessage("subHealth", AttackDamage);		
	}
	
	void PlaySound(string sound, float length)
	{
		
		if(SoundTimer == 0)
		{
			var rand = Random.value;
			switch(sound)
			{
				case("Idle"):
				if(rand >= 0 && rand < 0.05)
				{
					SoundTimer = SoundIdle1.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle1);
				}
				if(rand >= 0.05 && rand < 0.10)
				{
					SoundTimer = SoundIdle2.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle2);
				}
				if(rand >= 0.10 && rand < 0.15)
				{
					SoundTimer = SoundIdle3.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle3);
				}	
				if(rand >= 0.15 && rand < 0.20)
				{
					SoundTimer = SoundIdle4.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle4);
				}
				if(rand >= 0.20 && rand < 0.25)
				{
					SoundTimer = SoundIdle5.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle5);
				}
				if(rand >= 0.25 && rand < 0.30)
				{
					SoundTimer = SoundIdle6.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle6);
				}	
				break;
				
				case("Run"):
				if(rand >= 0 && rand < 0.2)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundRun1);
				}
				if(rand >= 0.2 && rand < 0.4)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundRun2);
				}
				if(rand >= 0.4 && rand < 0.6)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundRun3);
				}
				if(rand >= 0.6 && rand < 0.8)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundRun4);
				}
				if(rand >= 0.8 && rand < 1.0)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundRun5);
				}
				break;	
	
				case("Attack"):
				if(rand >= 0.0 && rand < 0.96)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundAttack1);
				}
				if(rand >= 0.96 && rand < 0.98)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundAttack2);
				}
				if(rand >= 0.98 && rand < 1.0)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundAttack3);
				}
				break;	

				case("Alert"):
				if(rand >= 0 && rand < 0.15)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle1);
				}
				if(rand >= 0.15 && rand < 0.30)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle2);
				}
				if(rand >= 0.30 && rand < 0.45)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle3);
				}	
				if(rand >= 0.45 && rand < 0.60)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle4);
				}
				if(rand >= 0.60 && rand < 0.75)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle5);
				}
				if(rand >= 0.75 && rand < 0.90)
				{
					SoundTimer = length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle6);
				}	
				break;
			}
		}
		
	}
	
	void SetAnimationState(string myState)
	{
		IdleState = false;
		RunState = false;
		AttackState = false;
		DieState = false;
		AlertState = false;
		PickUpState = false;
		CarryIdleState = false;
		CarryWalkState = false;
		ThrowState = false;
		FlyState = false;
		LandState = false;
		
		switch(currentState)
		{
			case("Idle"):
				IdleState = true;
			break;
			case("IdleMove"):
				RunState = true;
			break;
			case("Run"):
				RunState = true;
			break;	
			case("Attack"):
				AttackState = true;
			break;	
			case("Die"):
				DieState = true;
			break;
			case("Alert"):
				AlertState = true;			
			break;			
			case("PickUp"):
				PickUpState = true;			
			break;			
			case("CarryIdle"):
				CarryIdleState = true;			
			break;			
			case("CarryWalk"):
				CarryWalkState = true;			
			break;			
			case("Throw"):
				ThrowState = true;			
			break;			
			case("Fly"):
				FlyState = true;			
			break;			
			case("Land"):
				LandState = true;			
			break;			
		}
		anim.SetBool(hash.IdleState, IdleState);
		anim.SetBool(hash.RunState, RunState);
		anim.SetBool(hash.AttackState, AttackState);
		anim.SetBool(hash.DieState, DieState);
		anim.SetBool(hash.AlertState, AlertState);		
		anim.SetBool(hash.PickUpState, PickUpState);		
		anim.SetBool(hash.CarryIdleState, CarryIdleState);		
		anim.SetBool(hash.CarryWalkState, CarryWalkState);		
		anim.SetBool(hash.ThrowState, ThrowState);		
		anim.SetBool(hash.FlyState, FlyState);		
		anim.SetBool(hash.LandState, LandState);	
			
	}
	
	void UpdateState()
	{
		
//		print (currentState);
		switch(currentState)
		{
			case("Idle"):
			if (player != null)
			{
				// run toward the player if the player is carrying something and the player is close enough
				distanceToPlayer = (player.transform.position - transform.position).magnitude;
				
				if (player.GetComponent<Animator>().GetBool(hash.PlayerCarryObjectState) == true && distanceToPlayer <= alertDistance)
				{
					currentState = "Alert";
					PlaySound("Alert", 0.6666f);
				}
				if (player.GetComponent<Animator>().GetBool(hash.PlayerCarryObjectState) == true && distanceToPlayer <= visionDistance)
				{
					currentState = "Run";
					PlaySound("Run", 0.6666f);
				}
				// alert or attack the player if the player is carrying another baddie				
				if (player.GetComponent<Animator>().GetBool(hash.PlayerPickUpBaddieState) == true && distanceToPlayer <= alertDistance)
				{
					currentState = "Alert";
					PlaySound("Alert", 0.6666f);
				}
				if (player.GetComponent<Animator>().GetBool(hash.PlayerPickUpBaddieState) == true && distanceToPlayer <= visionDistance)
				{
					currentState = "Run";
					PlaySound("Run", 0.6666f);
				}
			}
			
			// after a certain amount of time then move
			maxIdleMoveTime -= Time.deltaTime;
			if(maxIdleMoveTime < 0)
			{
				currentState = "IdleMove";
				maxIdleMoveTime = Random.Range(setMinIdleMoveTime,setMaxIdleMoveTime); 
				idleMoveTarget.x = Random.Range(-100,100);
				idleMoveTarget.z = Random.Range(-100,100);
			}			
			break;

			case("IdleMove"):
			direction = (idleMoveTarget - transform.position).normalized;
			 //create the rotation we need to be in to look at the target
			lookRotation = Quaternion.LookRotation(direction);
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
			
			timerIdleMove -= Time.deltaTime;
			if(timerIdleMove < 0)
			{
				currentState = "Idle";
				timerIdleMove = 0.6666f; // should me the length of the run animation
//				PlaySound("Idle");
			}
			break;
				
			case("Run"):
			if (player != null)
			{
				direction = (player.transform.position - transform.position).normalized;
				 //create the rotation we need to be in to look at the target
				lookRotation = Quaternion.LookRotation(direction);
				//rotate us over time according to speed until we are in the required rotation
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
			
				if (player.GetComponent<Animator>().GetBool(hash.PlayerCarryObjectState) == false && player.GetComponent<Animator>().GetBool(hash.PlayerPickUpBaddieState) == false)
				{
					currentState = "Idle";
				}
			}
			break;

			case("Attack"):
			if (player == null)
			{
				print ("player dead");
				currentState = "Idle";
			}
			else
			{
				direction = (player.transform.position - transform.position).normalized;
				 //create the rotation we need to be in to look at the target
				lookRotation = Quaternion.LookRotation(direction);
				//rotate us over time according to speed until we are in the required rotation
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);

				// subtract health based on timer
				// this should be done based on an animation event
				// https://www.assetstore.unity3d.com/#/content/5969
				timerAttack -= Time.deltaTime;
				if(timerAttack < 0)
				{
					timerAttack = 0.6666f;
//					print ("reset");
					SubtractHealth();
				
					if (player.GetComponent<Animator>().GetBool(hash.PlayerCarryObjectState) == false)
					{
						currentState = "Idle";
					}	
					currentState = "Idle";
				}
				if(timerAttack < 0.3 && timerAttack > 0.2)
				{
					PlaySound("Attack", 0.6666f);
				}
			}
			break;
			
			case("Die"):
			break;

			case("Alert"):
			if (player != null)
			{
				direction = (player.transform.position - transform.position).normalized;
				 //create the rotation we need to be in to look at the target
				lookRotation = Quaternion.LookRotation(direction);
				//rotate us over time according to speed until we are in the required rotation
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
				
				timerAlert -= Time.deltaTime;
				if(timerAlert < 0)
				{
					timerAlert = 3.0f;
					currentState = "Idle";
				}
			}
			break;
			case("PickUp"):
			if (player == null)
			{
				print ("player dead");
				currentState = "Idle";
			}
			else
			{
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
				
//				this.transform.position = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.position;
//				this.transform.rotation = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.rotation;
			}
			break;
			case("CarryIdle"):
			if (player == null)
			{
				print ("player dead");
				currentState = "Idle";
			}
			else
			{
				this.transform.position = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.position;
				this.transform.rotation = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.rotation;
			}			
			break;
			case("CarryWalk"):
			if (player == null)
			{
				print ("player dead");
				currentState = "Idle";
			}
			else
			{
				this.transform.position = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.position;
				this.transform.rotation = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.rotation;
			}			
			break;
			case("Throw"):
			if (player == null)
			{
				print ("player dead");
				currentState = "Idle";
			}
			else
			{
				this.transform.position = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.position;
				this.transform.rotation = player.transform.Find("Anim_Master/Anim_Dummy_Carry").transform.rotation;
			}			
			timerThrow -= Time.deltaTime;
			if(timerThrow < 0)
			{
				timerThrow = 0.6666f;
				// add physics
				this.GetComponent<SphereCollider>().enabled = true;
				this.GetComponent<CapsuleCollider>().enabled = true;
				currentState = "Fly";
				print ("force added");
				SpinForceX = Random.Range((-1 * MaxSpinForce), MaxSpinForce);
				SpinForceZ = Random.Range((-1 * MaxSpinForce), MaxSpinForce);
				// reset pickuplerptimer for next time baddie is picked up
				PickUpLerpTimer = 0.5f;
			}			
			break;
			case("Fly"):
			myGravity = -0.1f;
			ThrowForce -= 30;
			if (ThrowForce < 0)
			{
				ThrowForce = 0;
			}
			GetComponent<Rigidbody>().AddRelativeForce ((Vector3.forward + Vector3.up) * ThrowForce);
			
			var temp = this.transform.rotation.eulerAngles;
			temp.x += SpinForceX;
			temp.z += SpinForceZ;
			this.transform.rotation = Quaternion.Euler(temp);
			
			break;
			case("Land"):
			ThrowForce = 2000;
			if(anim.GetCurrentAnimatorStateInfo(0).nameHash.ToString() == "-178524345")
			{
				currentState = "Idle";
			}
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, (new Quaternion(0, this.transform.rotation.y,0,1)), 10.0f * Time.deltaTime );
			break;
		}	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Paused == false)
		{
			SoundTimer -= Time.deltaTime;
			if (SoundTimer < 0)
			{
				SoundTimer = 0;
			}
			
			if (player == null)
			{
				currentState = "Idle";
			}
	
			UpdateState();
	
// play animation
			SetAnimationState(currentState);			
			
// add gravity
			if ( myGravity != 0)
			{
				Vector3 playerGravity = new Vector3(0f, myGravity, 0f) + this.transform.position;
				GetComponent<Rigidbody>().MovePosition(playerGravity);
			}
// kill the baddy if it falls too far
			if (this.gameObject.transform.position.y < -10.0f)
			{
				print ("destroyed");
				Destroy(this.gameObject);
			}	
		}
	}
}
