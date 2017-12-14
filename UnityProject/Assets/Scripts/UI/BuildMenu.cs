using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour {
	
	public string EditMode = "BuildableLayer";
	public TouchControl TouchContorlScript;
	public GameObject PlayerMapObject;
	public Texture2D ButtonMoveBitmap; 
	private Vector4 ButtonMoveBitmapSize = new Vector4( 0.0f, 0.2f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	public Texture2D ButtonRotateBitmap; 
	private Vector4 ButtonRotateBitmapSize = new Vector4( 0.0f, 0.3f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	public Texture2D ButtonDeleteBitmap; 
	private Vector4 ButtonDeleteBitmapSize = new Vector4( 0.0f, 0.4f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size

	private GUIStyle style;
	public Font myFont;
	public int myFontSize = 60;
	
	
	// Use this for initialization
	void Awake () 
	{
		TouchContorlScript = GameObject.Find("TouchControl").GetComponent<TouchControl>();
		PlayerMapObject = GameObject.Find("PlayerMap");
	}
	
	void Start ()
	{
		// Scale Font to Screen Size
		style = new GUIStyle();
		style.font = myFont;
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		style.normal.textColor = Color.white;		
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect((Screen.width * 0.0f ), (Screen.height * 0.11f), 100, 100),TouchContorlScript.EditMode))
		{
			TouchContorlScript.SendMessage("ToggleEditMode");	
		}		

		if (GUI.Button(new Rect ((Screen.width * ButtonMoveBitmapSize.x ), (Screen.height * ButtonMoveBitmapSize.y), (Screen.width * ButtonMoveBitmapSize.z) , (Screen.height * ButtonMoveBitmapSize.w)), ButtonMoveBitmap, style))
		{
			TouchContorlScript.SendMessage("ToggleTranslationMode", "Move");
		}	
		if (GUI.Button(new Rect ((Screen.width * ButtonRotateBitmapSize.x ), (Screen.height * ButtonRotateBitmapSize.y), (Screen.width * ButtonRotateBitmapSize.z) , (Screen.height * ButtonRotateBitmapSize.w)), ButtonRotateBitmap, style))
		{
			TouchContorlScript.SendMessage("ToggleTranslationMode", "Rotate");
		}			
		if (GUI.Button(new Rect ((Screen.width * ButtonDeleteBitmapSize.x ), (Screen.height * ButtonDeleteBitmapSize.y), (Screen.width * ButtonDeleteBitmapSize.z) , (Screen.height * ButtonDeleteBitmapSize.w)), ButtonDeleteBitmap, style))
		{
			TouchContorlScript.DeleteNextObject = true;
		}		

		if (TouchContorlScript.EditMode == "BuildableLayer")
		{
			
			// buildable buttons
			if (GUI.Button(new Rect((0), (Screen.height - 100), 100, 100),(Texture)(Resources.Load("PickUps/PickUps_Acorn_01/PickUps_Acorn_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{			
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("PickUps/PickUps_Acorn_01/PickUps_Acorn_01_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((100), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("PickUps/PickUps_Strawberry/PickUps_Strawberry_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{			
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("PickUps/PickUps_Strawberry/PickUps_Strawberry_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((200), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("PickUps/PickUps_Raspberry/PickUps_Raspberry_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{			
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("PickUps/PickUps_Raspberry/PickUps_Raspberry_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((300), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Decorations/Deco_Mushroom_01/Deco_Mushroom_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("Decorations/Deco_Mushroom_01/Deco_Mushroom_01_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((400), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Decorations/Deco_Plant_01/Deco_Plant_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{			
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("Decorations/Deco_Plant_01/Deco_Plant_01_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((500), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Decorations/Deco_Plant_02/Deco_Plant_02_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("Decorations/Deco_Plant_02/Deco_Plant_02_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((600), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Decorations/Deco_Plant_04/Deco_Plant_04_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("Decorations/Deco_Plant_04/Deco_Plant_04_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((700), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Decorations/Deco_Tree_01/Deco_Tree_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{			
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("Decorations/Deco_Tree_01/Deco_Tree_01_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((800), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("DropOffs/DropOffs_Rocks_01/DropOffs_Rocks_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{			
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("DropOffs/DropOffs_Rocks_01/DropOffs_Rocks_01_Prefab");
				}
			}
			
	        if (GUI.Button(new Rect((900), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Baddies/Baddies_Bunny/Bunny_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "BuildableLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddBuildableByAssetPath("Baddies/Baddies_Bunny/Baddies_Bunny_Prefab");
				}
			}
		}
		if (TouchContorlScript.EditMode == "TerrainLayer")
		{		
	        if (GUI.Button(new Rect((0), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Terrain_Parts/Terrain_Hill/Terrain_Hill_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "TerrainLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddTerrainByAssetPath("Terrain_Parts/Terrain_Hill/Terrain_Hill_Prefab");
				}
			}		
	        if (GUI.Button(new Rect((100), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Terrain_Parts/Terrain_Hill_01/Terrain_Hill_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "TerrainLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddTerrainByAssetPath("Terrain_Parts/Terrain_Hill_01/Terrain_Hill_01_Prefab");
				}
			}		
	        if (GUI.Button(new Rect((200), (Screen.height - 100), 100, 100),  (Texture)(Resources.Load("Terrain_Parts/Terrain_Hill_02/Terrain_Hill_02_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "TerrainLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddTerrainByAssetPath("Terrain_Parts/Terrain_Hill_02/Terrain_Hill_02_Prefab");
				}
			}	
	        if (GUI.Button(new Rect((300), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Terrain_Parts/Terrain_Cliff_01/Terrain_Cliff_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "TerrainLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddTerrainByAssetPath("Terrain_Parts/Terrain_Cliff_01/Terrain_Cliff_01_Prefab");
				}
			}	
	        if (GUI.Button(new Rect((400), (Screen.height - 100), 100, 100), (Texture)(Resources.Load("Terrain_Parts/Terrain_Pond_01/Terrain_Pond_01_Icon"))))
			{
				if(TouchContorlScript.GetComponent<TouchControl>().EditMode == "TerrainLayer")
				{
					TouchContorlScript.DeleteNextObject = false;
					PlayerMapObject.GetComponent<PlayerMap>().AddTerrainByAssetPath("Terrain_Parts/Terrain_Pond_01/Terrain_Pond_01_Prefab");
				}
			}		
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
