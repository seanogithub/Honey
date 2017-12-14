using UnityEngine;
using System.Collections;

public class PopUpLevelFailed : MonoBehaviour {
	
	private GameObject GameControllerObject;
	private GameController changeLevelScript;    // Reference to the player GameObject.
	private GameObject player;    // Reference to the player GameObject.
	private GameObject[] baddies;    // Reference to the player GameObject.
	
	public float Timer = 3.0f; 	

	
	// Use this for initialization
	void Start () 
	{
		GameControllerObject = GameObject.FindGameObjectWithTag("GameController");
		player = GameObject.FindGameObjectWithTag(Tags.player);
		baddies = GameObject.FindGameObjectsWithTag("Baddie");
		
		// pause player and baddies
		player.GetComponent<PlayerMovement>().Pause(true);
		
		for (var i = 0; i < baddies.Length; i++)
		{
			baddies[i].GetComponent<BaddieAI>().Pause(true);
		}		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Timer -= Time.deltaTime;
		
		if (Timer <=0)
		{
//			changeLevelScript = GameControllerObject.GetComponent<GameController>();
//			changeLevelScript.SendMessage("ReloadLevel");	
			GameControllerObject.GetComponent<GameController>().ReloadLevel();
		}
	}
}
