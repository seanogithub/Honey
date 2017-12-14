using UnityEngine;
using System.Collections;

public class PopUpLevelComplete : MonoBehaviour {
	
	private GameObject GameControllerObject;
	private GameController changeLevelScript;    // Reference to the player GameObject.
	private GameObject player;    // Reference to the player GameObject.
	private GameObject[] baddies;    // Reference to the player GameObject.
	private Vector2 PopUpLevelCompleteTextSize = new Vector2( 0.3f, 0.50f); // x-pos,y-pos
	private Vector2 PopUpHoneyPointsTextSize = new Vector2( 0.6f, 0.50f); // x-pos,y-pos
	
	public Texture2D LevelCompleteBitmap;
	private Vector4 LevelCompleteBitmapSize = new Vector4( 0.33f, 0.25f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size

	private float AspectRatioMultiplier = 1.5f;
	
	private GUIStyle style;
	public Font myFont;
	public int myFontSize = 60;
	
	public float Timer = 3.0f; 	
	private int HoneyPoints = 0;
	
	// Use this for initialization
	void Start () 
	{
		GameControllerObject = GameObject.FindGameObjectWithTag("GameController");
		player = GameObject.FindGameObjectWithTag(Tags.player);
		baddies = GameObject.FindGameObjectsWithTag("Baddie");
		
		// Scale Font to Screen Size
		style = new GUIStyle();
		style.font = myFont;
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		style.normal.textColor = Color.white;
		AspectRatioMultiplier = ((float)Screen.width/1920 );
		
		// pause player and baddies
		player.GetComponent<PlayerMovement>().Pause(true);
		
		for (var i = 0; i < baddies.Length; i++)
		{
			baddies[i].GetComponent<BaddieAI>().Pause(true);
		}		
		
		HoneyPoints = player.GetComponent<PlayerStats>().playerHoneyPoints;
		
		
		var PlayerBlob = GameObject.Find("PlayerBlobManager_Prefab");
		PlayerBlob.GetComponent<PlayerBlobManager>().PlayerAccountData.playerHoneyPoints += HoneyPoints;
		
		// unlock next level if not the custom map
		if(Application.loadedLevelName != "Custom_Level_01")
		{
			var UnlockNextLevel = GameControllerObject.GetComponent<GameController>().CurrentLevel + 1;
			if (UnlockNextLevel <= 9)
			{
				PlayerBlob.GetComponent<PlayerBlobManager>().PlayerAccountData.StoryModeLevelLocked[UnlockNextLevel] = false;
			}
		}
		
		PlayerBlob.GetComponent<PlayerBlobManager>().SaveAccountData();
	}

	void OnGUI()
	{
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		AspectRatioMultiplier = ((float)Screen.width/1920 );
		// draw LevelCompeteBitmap
		GUI.DrawTexture((new Rect(((float)Screen.width * LevelCompleteBitmapSize.x ),((float)Screen.height * LevelCompleteBitmapSize.y ),LevelCompleteBitmap.width * AspectRatioMultiplier,LevelCompleteBitmap.height * AspectRatioMultiplier)), LevelCompleteBitmap,ScaleMode.StretchToFill,true);
		
		GUI.Label((new Rect(((float)Screen.width * PopUpLevelCompleteTextSize.x ),((float)Screen.height * PopUpLevelCompleteTextSize.y ),(float)style.fontSize * AspectRatioMultiplier,(float)style.fontSize * AspectRatioMultiplier)), "Honey Points:", style);
		GUI.Label((new Rect(((float)Screen.width * PopUpHoneyPointsTextSize.x ),((float)Screen.height * PopUpHoneyPointsTextSize.y ),(float)style.fontSize * AspectRatioMultiplier,(float)style.fontSize * AspectRatioMultiplier)), HoneyPoints.ToString(), style);
		
//		gameObject.GetComponentsInChildren<GUIText>()[0].text = HoneyPoints.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Timer -= Time.deltaTime;
		
		if (Timer <=0)
		{
			if(Application.loadedLevelName != "Custom_Level_01")
			{			
//				changeLevelScript = GameControllerObject.GetComponent<GameController>();
//				changeLevelScript.SendMessage("LoadNextLevel");	
				GameControllerObject.GetComponent<GameController>().LoadNextLevel();	
			}
			else
			{
				Application.LoadLevel("MainMenu");				
			}
		}
	}
}
