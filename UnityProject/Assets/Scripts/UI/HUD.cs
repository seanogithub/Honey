using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	public bool Paused = false;
	private GameObject LevelCompletedPopUp;
	private GameObject GameControllerObject;
	private GameController changeLevelScript;    // Reference to the player GameObject.
	private GameObject playerObject;    // Reference to the player GameObject.
	private GameObject[] pickUpObjects;    // Reference to the player GameObject.
	
	public Texture2D ButtonBitmap;
	public Texture2D ButtonCloseBitmap;
	private Vector4 ButtonCloseBitmapSize = new Vector4( 0.96f, 0.02f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	
	private GUIStyle style;
	public Font myFont;
	public int myFontSize = 60;
	
	public Texture2D HealthBarBitmap;
	public Texture2D HealthBarFrameBitmap;
	private Vector4 HealthBarSize = new Vector4( 0.45f, 0.00f, 0.2f, 0.15f); // x-pos,y-pos,x-size,y-size

	public Texture2D ScoreBitmap;
	private Vector4 ScoreBitmapSize = new Vector4( 0.0f, 0.00f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size
	private Vector4 ScoreValueSize = new Vector4( 0.15f, 0.00f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size

	public Texture2D TimeBitmap;
	private Vector4 TimeBitmapSize = new Vector4( 0.0f, 0.05f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size
	private Vector4 TimeValueSize = new Vector4( 0.15f, 0.05f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size
	
	private float AspectRatioMultiplier = 1.5f;
	
	public bool ButtonPressed;
	private float LevelTimer = 1.0f;	

	public float LevelTimerMultiplier = 30.0f;
	
	
	// This needs to be start instead of awake so it will find the pickup objects for the score
	void Start () 
	{
		GameControllerObject = GameObject.FindGameObjectWithTag("GameController");
		playerObject = GameObject.FindGameObjectWithTag("Player");
		pickUpObjects = GameObject.FindGameObjectsWithTag("PickUp");
		LevelTimer *= (pickUpObjects.Length * LevelTimerMultiplier);
		
		// Scale Font to Screen Size
		style = new GUIStyle();
		style.font = myFont;
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		style.normal.textColor = Color.white;
		
		AspectRatioMultiplier = ((float)Screen.width/1920 );
	}

	public void Pause (bool myValue)
	{
		Paused = myValue;
	}
	
	void OnGUI()
	{
		if (Paused == false)
		{
			int score = playerObject.GetComponent<PlayerStats>().playerScore;
			int health = playerObject.GetComponent<PlayerStats>().playerHealth;
			if (health <=2) // keep heath bar from going negative.
			{
				health = 0;
			}
	
			style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
			AspectRatioMultiplier = ((float)Screen.width/1920 );
			
			// draw score
			GUI.DrawTexture((new Rect(((float)Screen.width * ScoreBitmapSize.x ),((float)Screen.height * ScoreBitmapSize.y ),ScoreBitmap.width * AspectRatioMultiplier * health / 100, ScoreBitmap.height * AspectRatioMultiplier)), ScoreBitmap,ScaleMode.StretchToFill,true);
			GUI.Label((new Rect(((float)Screen.width * ScoreValueSize.x ),((float)Screen.height * ScoreValueSize.y ),(float)style.fontSize * AspectRatioMultiplier,(float)style.fontSize * AspectRatioMultiplier)), (pickUpObjects.Length - score).ToString(), style);

			// draw timer
			GUI.DrawTexture((new Rect(((float)Screen.width * TimeBitmapSize.x ),((float)Screen.height * TimeBitmapSize.y ),TimeBitmap.width * AspectRatioMultiplier * health / 100, TimeBitmap.height * AspectRatioMultiplier)), TimeBitmap,ScaleMode.StretchToFill,true);
			GUI.Label((new Rect(((float)Screen.width * TimeValueSize.x ),((float)Screen.height * TimeValueSize.y ),(float)style.fontSize * AspectRatioMultiplier,(float)style.fontSize * AspectRatioMultiplier)), (Mathf.Round( LevelTimer*10) / 10).ToString(), style);

			
			// screen proportional health bar
			GUI.DrawTexture((new Rect(((float)Screen.width * HealthBarSize.x ),((float)Screen.height * HealthBarSize.y ),HealthBarBitmap.width * AspectRatioMultiplier * health / 100, HealthBarBitmap.height * AspectRatioMultiplier)), HealthBarBitmap,ScaleMode.StretchToFill,true);
			GUI.DrawTexture((new Rect(((float)Screen.width * HealthBarSize.x ),((float)Screen.height * HealthBarSize.y ),HealthBarBitmap.width * AspectRatioMultiplier, HealthBarBitmap.height * AspectRatioMultiplier)), HealthBarFrameBitmap,ScaleMode.StretchToFill,true);

			
			#if UNITY_EDITOR || UNITY_STANDALONE
	        if (GUI.Button(new Rect((Screen.width - 200), (Screen.height - 200), 100, 100), ButtonBitmap))
			{
	            ButtonPressed = true;
			}	
			#endif			
			
			if (score == pickUpObjects.Length) 
			{
				Paused = true;
				var temp = Resources.Load("UI/HUD/PopUp_Level_Complete_Prefab",typeof(GameObject)) as GameObject;
				LevelCompletedPopUp = GameObject.Instantiate(temp, new Vector3 (0,0,0), Quaternion.identity ) as GameObject;		
			}
			
			if (LevelTimer <= 0)
			{
				Paused = true;
				var temp = Resources.Load("UI/HUD/PopUp_Level_Failed_Prefab",typeof(GameObject)) as GameObject;
				LevelCompletedPopUp = GameObject.Instantiate(temp, new Vector3 (0.5f,0.75f,0), Quaternion.identity ) as GameObject;		
			}			
		}
		
		if (GUI.Button( new Rect (((float)Screen.width * ButtonCloseBitmapSize.x ),((float)Screen.height * ButtonCloseBitmapSize.y ),ButtonCloseBitmap.width * AspectRatioMultiplier , ButtonCloseBitmap.height * AspectRatioMultiplier) ,ButtonCloseBitmap,style))
		{
			Application.LoadLevel("MainMenu");
		}			
		
	}

	void Update()
	{
		if (Paused == false)
		{
			LevelTimer -= Time.deltaTime;
		}
	}
}
