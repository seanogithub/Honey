using UnityEngine;
using System.Collections;

public class EditorController : MonoBehaviour {

	private string[] Levels;
	private int CurrentLevel;
	public GameObject JoystickObject;
	public GameObject PlayerObject;
	public Texture2D ButtonClose;
	
	void Awake () 
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.autorotateToPortrait = false;
		
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
	
	void OnGUI()
	{
		GameObject[] fuck = FindObjectsOfType(typeof(GameObject)) as GameObject[];
//		print (fuck.Length);
		GUI.Label(new Rect(10, 30, 100, 20), (fuck.Length.ToString()));
/*		
		if (GUI.Button(new Rect((Screen.width - 110), (10), 100, 100),ButtonClose))
		{
			Application.LoadLevel("MainMenu");
		}		
*/		
	}
	
	void ReloadLevel()
	{
		Application.LoadLevel(Levels[CurrentLevel]);	
		print (Application.loadedLevelName);
		print (CurrentLevel);
	}
	
	void LoadNextLevel()
	{
		PlayerObject.GetComponent<PlayerMovement>().enabled = false;
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

	void Update()
	{
	}
}
