using UnityEngine;
using System.Collections;

public class StoryModeMap : MonoBehaviour {

	private GameObject PlayerBlob;

	private GUIStyle style;
	public Font myFont;
	public int myFontSize = 60;

	private float AspectRatioMultiplier = 1.5f;
	
	public Texture2D ButtonCloseBitmap;
	private Vector4 ButtonCloseBitmapSize = new Vector4( 0.96f, 0.02f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	
	public Texture2D StoryModeMapBitmap;

	public Texture2D ButtonStoryModeMapLevelBitmap;
	public Texture2D ButtonStoryModeMapLevelSelectedBitmap;

	public Texture2D ButtonStoryModeMapLevelLockedBitmap;
	
	private Texture2D ButtonBitmapLevel01;
	private Texture2D ButtonBitmapLevel02;
	private Texture2D ButtonBitmapLevel03;
	private Texture2D ButtonBitmapLevel04;
	private Texture2D ButtonBitmapLevel05;
	private Texture2D ButtonBitmapLevel06;
	private Texture2D ButtonBitmapLevel07;
	private Texture2D ButtonBitmapLevel08;
	private Texture2D ButtonBitmapLevel09;
	private Texture2D ButtonBitmapLevel10;
	
	private Vector4 ButtonBitmapLevel01Size = new Vector4( 0.11f, 0.11f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel02Size = new Vector4( 0.16f, 0.37f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel03Size = new Vector4( 0.21f, 0.62f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel04Size = new Vector4( 0.36f, 0.78f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel05Size = new Vector4( 0.42f, 0.46f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel06Size = new Vector4( 0.52f, 0.20f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel07Size = new Vector4( 0.68f, 0.37f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel08Size = new Vector4( 0.83f, 0.11f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel09Size = new Vector4( 0.89f, 0.45f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private Vector4 ButtonBitmapLevel10Size = new Vector4( 0.84f, 0.78f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	
	public bool[] StoryModeLevelLocked = new bool[10];
	
	
	// Use this for initialization
	void Start () 
	{
		PlayerBlob = GameObject.Find("PlayerBlobManager_Prefab");	
		
		// Scale Font to Screen Size
		style = new GUIStyle();
		style.font = myFont;
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		style.normal.textColor = Color.white;	

		AspectRatioMultiplier = ((float)Screen.width/1920 );
		
		// init story mode level locks		
		for(int i = 0; i <= 9 ; i++)
		{
//			print (PlayerBlob);
			StoryModeLevelLocked[i] = PlayerBlob.GetComponent<PlayerBlobManager>().PlayerAccountData.StoryModeLevelLocked[i];
		}
/*		
		StoryModeLevelLocked[0] = false;
		StoryModeLevelLocked[1] = false;
		StoryModeLevelLocked[2] = false;
		StoryModeLevelLocked[3] = true;
		StoryModeLevelLocked[4] = true;
		StoryModeLevelLocked[5] = true;
		StoryModeLevelLocked[6] = true;
		StoryModeLevelLocked[7] = true;
		StoryModeLevelLocked[8] = true;
		StoryModeLevelLocked[9] = true;
*/		
		
		UpdateLevelButtons();
		
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect (0, 0, (Screen.width) , (Screen.height)), StoryModeMapBitmap,ScaleMode.StretchToFill,true);
// close button
		if (GUI.Button( new Rect (((float)Screen.width * ButtonCloseBitmapSize.x ),((float)Screen.height * ButtonCloseBitmapSize.y ),ButtonCloseBitmap.width * AspectRatioMultiplier , ButtonCloseBitmap.height * AspectRatioMultiplier) ,ButtonCloseBitmap,style))
		{
			Application.LoadLevel("MainMenu");
		}
// load level buttons
		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel01Size.x ), (Screen.height * ButtonBitmapLevel01Size.y), (Screen.width * ButtonBitmapLevel01Size.z) , (Screen.height * ButtonBitmapLevel01Size.w)) ,ButtonBitmapLevel01,style))
		{
			if(StoryModeLevelLocked[0] == false)
			{
				ButtonBitmapLevel01 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel02Size.x ), (Screen.height * ButtonBitmapLevel02Size.y), (Screen.width * ButtonBitmapLevel02Size.z) , (Screen.height * ButtonBitmapLevel02Size.w)) ,ButtonBitmapLevel02,style))
		{
			if(StoryModeLevelLocked[1] == false)
			{
				ButtonBitmapLevel02 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_02");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel03Size.x ), (Screen.height * ButtonBitmapLevel03Size.y), (Screen.width * ButtonBitmapLevel03Size.z) , (Screen.height * ButtonBitmapLevel03Size.w)) ,ButtonBitmapLevel03,style))
		{
			if(StoryModeLevelLocked[2] == false)
			{
				ButtonBitmapLevel03 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_03");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel04Size.x ), (Screen.height * ButtonBitmapLevel04Size.y), (Screen.width * ButtonBitmapLevel04Size.z) , (Screen.height * ButtonBitmapLevel04Size.w)) ,ButtonBitmapLevel04,style))
		{
			if(StoryModeLevelLocked[3] == false)
			{
				ButtonBitmapLevel04 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}		
		}
		
		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel05Size.x ), (Screen.height * ButtonBitmapLevel05Size.y), (Screen.width * ButtonBitmapLevel05Size.z) , (Screen.height * ButtonBitmapLevel05Size.w)) ,ButtonBitmapLevel05,style))
		{
			if(StoryModeLevelLocked[4] == false)
			{
				ButtonBitmapLevel05 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel06Size.x ), (Screen.height * ButtonBitmapLevel06Size.y), (Screen.width * ButtonBitmapLevel06Size.z) , (Screen.height * ButtonBitmapLevel06Size.w)) ,ButtonBitmapLevel06,style))
		{
			if(StoryModeLevelLocked[5] == false)
			{
				ButtonBitmapLevel06 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel07Size.x ), (Screen.height * ButtonBitmapLevel07Size.y), (Screen.width * ButtonBitmapLevel07Size.z) , (Screen.height * ButtonBitmapLevel07Size.w)) ,ButtonBitmapLevel07,style))
		{
			if(StoryModeLevelLocked[6] == false)
			{
				ButtonBitmapLevel07 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel08Size.x ), (Screen.height * ButtonBitmapLevel08Size.y), (Screen.width * ButtonBitmapLevel08Size.z) , (Screen.height * ButtonBitmapLevel08Size.w)) ,ButtonBitmapLevel08,style))
		{
			if(StoryModeLevelLocked[7] == false)
			{
				ButtonBitmapLevel08 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel09Size.x ), (Screen.height * ButtonBitmapLevel09Size.y), (Screen.width * ButtonBitmapLevel09Size.z) , (Screen.height * ButtonBitmapLevel09Size.w)) ,ButtonBitmapLevel09,style))
		{
			if(StoryModeLevelLocked[8] == false)
			{
				ButtonBitmapLevel09 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}
		}		

		if (GUI.Button( new Rect ((Screen.width * ButtonBitmapLevel10Size.x ), (Screen.height * ButtonBitmapLevel10Size.y), (Screen.width * ButtonBitmapLevel10Size.z) , (Screen.height * ButtonBitmapLevel10Size.w)) ,ButtonBitmapLevel10,style))
		{
			if(StoryModeLevelLocked[9] == false)
			{
				ButtonBitmapLevel10 = ButtonStoryModeMapLevelSelectedBitmap;
				Application.LoadLevel("HunnyPot_Level_01");
			}
		}		
	}
	
	void UpdateLevelButtons()
	{
		if(StoryModeLevelLocked[0] == false)
		{
			ButtonBitmapLevel01 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel01 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[1] == false)
		{
			ButtonBitmapLevel02 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel02 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[2] == false)
		{
			ButtonBitmapLevel03 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel03 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[3] == false)
		{
			ButtonBitmapLevel04 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel04 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[4] == false)
		{
			ButtonBitmapLevel05 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel05 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[5] == false)
		{
			ButtonBitmapLevel06 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel06 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[6] == false)
		{
			ButtonBitmapLevel07 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel07 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[7] == false)
		{
			ButtonBitmapLevel08 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel08 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[8] == false)
		{
			ButtonBitmapLevel09 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel09 = ButtonStoryModeMapLevelLockedBitmap;
		}
		if(StoryModeLevelLocked[9] == false)
		{
			ButtonBitmapLevel10 = ButtonStoryModeMapLevelBitmap;
		}
		else
		{
			ButtonBitmapLevel10 = ButtonStoryModeMapLevelLockedBitmap;
		}
		
	}
}
