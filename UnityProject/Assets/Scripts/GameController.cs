using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	private string[] Levels = new string[10];
	public int CurrentLevel;
	
	public GameObject JoystickObject;
	public GameObject PlayerObject;
	public Texture2D ButtonClose;
	
	void Awake () 
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.autorotateToPortrait = false;
		
		JoystickObject = GameObject.FindGameObjectWithTag("joystick");
		PlayerObject = GameObject.FindGameObjectWithTag("Player");

		if (Application.isLoadingLevel == true)
		{
			PlayerObject.GetComponent<PlayerMovement>().enabled = false;
		}
		else
		{
			PlayerObject.GetComponent<PlayerMovement>().enabled = true;
		}
		
		Levels = new string[10];
		Levels[0] = "HunnyPot_Level_01";
		Levels[1] = "HunnyPot_Level_02";
		Levels[2] = "HunnyPot_Level_03";
		Levels[3] = "HunnyPot_Level_03";
		Levels[4] = "HunnyPot_Level_03";
		Levels[5] = "HunnyPot_Level_03";
		Levels[6] = "HunnyPot_Level_03";
		Levels[7] = "HunnyPot_Level_03";
		Levels[8] = "HunnyPot_Level_03";
		Levels[9] = "HunnyPot_Level_03";
		

		for (int i = 0; i < Levels.Length; i++)
		{
			if (Application.loadedLevelName ==  Levels[i])
			{
				CurrentLevel = i;
			}
		}
		
//		print (Application.loadedLevelName);
//		print (CurrentLevel);
	}
	

/*	
	void OnGUI()
	{
		if (GUI.Button(new Rect((Screen.width - 110), (10), 100, 100),ButtonClose))
		{
			Application.LoadLevel("MainMenu");
		}		
	}
*/
	
	public void ReloadLevel()
	{
		Application.LoadLevel(Application.loadedLevelName);	
	}
	
	public void LoadNextLevel()
	{
		PlayerObject.GetComponent<PlayerMovement>().enabled = false;
		if (Application.isLoadingLevel == false)
		{
			CurrentLevel += 1;
			if (CurrentLevel > Levels.Length - 1)
			{
				CurrentLevel = 0;
			}

			print (Levels[CurrentLevel]);
			print (CurrentLevel);
			Application.LoadLevel(Levels[CurrentLevel]);	
		}
	}
	
	void Update()
	{
		
		if (Application.isLoadingLevel == true)
		{
			PlayerObject.GetComponent<PlayerMovement>().enabled = false;
		}
		else
		{
			PlayerObject.GetComponent<PlayerMovement>().enabled = true;
		}		
	}
	
}
