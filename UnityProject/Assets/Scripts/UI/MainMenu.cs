using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public Texture2D ButtonBitmap;
	private string[] Levels;
	private int CurrentLevel;

	private GUIStyle style;
	public Font myFont;
	public int myFontSize = 60;

	private float AspectRatioMultiplier = 1.5f;
	
	public Texture2D ButtonCloseBitmap;
	private Vector4 ButtonCloseBitmapSize = new Vector4( 0.96f, 0.02f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	
	public Texture2D MainMenuBackgroundBitmap;
	
	public Texture2D ButtonStoryModeBitmap;
	public Texture2D ButtonStoryModeSelectedBitmap;
	private Vector4 ButtonStoryModeBitmapSize = new Vector4( 0.125f, 0.7f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size
	
	public Texture2D ButtonCustomLevelBitmap;
	public Texture2D ButtonCustomLevelSelectedBitmap;
	private Vector4 ButtonCustomLevelBitmapSize = new Vector4( 0.45f, 0.7f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size

	public Texture2D ButtonLevelEditorBitmap;
	public Texture2D ButtonLevelEditorSelectedBitmap;
	private Vector4 ButtonLevelEditorBitmapSize = new Vector4( 0.75f, 0.7f, 0.2f, 0.2f); // x-pos,y-pos,x-size,y-size
	
	// Use this for initialization
	void Awake () 
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.autorotateToPortrait = false;

		AspectRatioMultiplier = ((float)Screen.width/1920 );
		
		Levels = new string[4];
		Levels[0] = "Custom_Level_01";
		Levels[1] = "HunnyPot_Level_01";
		Levels[2] = "HunnyPot_Level_02";
		Levels[3] = "HunnyPot_Level_03";

		for (int i = 0; i < Levels.Length; i++)
		{
			if (Application.loadedLevelName ==  Levels[i])
			{
				CurrentLevel = i;
			}
		}		
	}
	
	void Start()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.autorotateToPortrait = false;	
		
		// Scale Font to Screen Size
		style = new GUIStyle();
		style.font = myFont;
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		style.normal.textColor = Color.white;		
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect (0, 0, (Screen.width) , (Screen.height)), MainMenuBackgroundBitmap,ScaleMode.StretchToFill,true);
/*
// close button
		if (GUI.Button( new Rect (((float)Screen.width * ButtonCloseBitmapSize.x ),((float)Screen.height * ButtonCloseBitmapSize.y ),ButtonCloseBitmap.width * AspectRatioMultiplier , ButtonCloseBitmap.height * AspectRatioMultiplier) ,ButtonCloseBitmap,style))
		{
			Application.Quit();
//			Application.LoadLevel("Custom_Level_01");
		}		
*/			
		if (GUI.Button( new Rect ((Screen.width * ButtonCustomLevelBitmapSize.x ), (Screen.height * ButtonCustomLevelBitmapSize.y), (Screen.width * ButtonCustomLevelBitmapSize.z) , (Screen.height * ButtonCustomLevelBitmapSize.w)) ,ButtonCustomLevelBitmap,style))
		{
			ButtonCustomLevelBitmap = ButtonCustomLevelSelectedBitmap;
			Application.LoadLevel("Custom_Level_01");
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonStoryModeBitmapSize.x ), (Screen.height * ButtonStoryModeBitmapSize.y), (Screen.width * ButtonStoryModeBitmapSize.z) , (Screen.height * ButtonStoryModeBitmapSize.w)) ,ButtonStoryModeBitmap,style))
		{
			ButtonStoryModeBitmap = ButtonStoryModeSelectedBitmap;
			Application.LoadLevel("Story_Mode_Map");
//			LoadNextLevel();
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonLevelEditorBitmapSize.x ), (Screen.height * ButtonLevelEditorBitmapSize.y), (Screen.width * ButtonLevelEditorBitmapSize.z) , (Screen.height * ButtonLevelEditorBitmapSize.w)) ,ButtonLevelEditorBitmap,style))
		{
			ButtonLevelEditorBitmap = ButtonLevelEditorSelectedBitmap;
			Application.LoadLevel("Level_Editor");
		}		
	}

	void ReloadLevel()
	{
		Application.LoadLevel(Levels[CurrentLevel]);	
		print (Application.loadedLevelName);
		print (CurrentLevel);
	}
	
	void LoadNextLevel()
	{
		if (Application.isLoadingLevel == false)
		{
			CurrentLevel += 1;
			if (CurrentLevel > Levels.Length - 1)
			{
				CurrentLevel = 0;
			}
			
			Application.LoadLevel(Levels[CurrentLevel]);	
	
	//		print (Application.loadedLevelName);
	//		print (CurrentLevel);
		}
	}
	
	
	// Update is called once per frame
	void Update () 
	{
	}	
}
