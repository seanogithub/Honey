using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text;

public class PlayerMap : MonoBehaviour {
	
	
	public int MapSizeX = 20;
	public int MapSizeY = 20;
	public int MapTileSize = 2;
	public Buildable[,] MapBuildableLayer = new Buildable[20, 20];
	public BuildableList ListOfBuildables = new BuildableList();
	public GameObject[] BuildableSceneObjects = new GameObject[400];
	public MapBuildable[] BuildablesInMap = new MapBuildable[400];
//	public SaveData PlayerSaveData = new SaveData();	

	public MapBuildable[] TerrainInMap = new MapBuildable[400];
	public GameObject[,] NewMapTerrainLayer = new GameObject[20, 20];
	
	public Buildable DefaultTerrain;
	public Buildable EmptyTerrain;
	
	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 	 	
	string _data; 	
//	MapBuildable[] myData;
	SaveData myData;
	
	void Awake () 
	{
//		var targets = new List<MapBuildable>();

		// We setup our rectangles for our messages 
		_Save=new Rect(10,100,100,100); 
		_Load=new Rect(10,200,100,100);  
		_SaveMSG=new Rect(10,120,400,40); 
		_LoadMSG=new Rect(10,140,400,40); 
		
		// Where we want to save and load to and from 
//		_FileLocation=Application.dataPath; 
		_FileLocation=Application.persistentDataPath; 
		_FileName="SaveData.xml";  
		
		// we need soemthing to store the information into 
//		myData=new MapBuildable[400]; 	
		myData=new SaveData(); 	
	
		ListOfBuildables.Init();

		InitBuildableLayer();
		FillBuildableLayer();
		
		DefaultTerrain = new Buildable();
		DefaultTerrain = ListOfBuildables.FindByName("Terrain_Flat_Prefab");

		EmptyTerrain = new Buildable();
		EmptyTerrain = ListOfBuildables.FindByName("Terrain_Empty_Prefab");		
		
	}

	void Start()
	{
		if(File.Exists(_FileLocation+"\\"+ _FileName))
		{
//			LoadMap();
			LoadMapFromPlayerBlobManager();
		}
		else
		{
			InitTerrainLayer();
			print("WARNING: SAVE FILE IS MISSING!");
		}
	}

	void LoadMapFromPlayerBlobManager()
	{
		var PlayerBlob = GameObject.Find("PlayerBlobManager_Prefab");
		PlayerBlob.GetComponent<PlayerBlobManager>().LoadPlayerData();
		
		myData.MapBuildableLayer = PlayerBlob.GetComponent<PlayerBlobManager>().BuildablesInMap;
		for(var i = 0; i < 400; i++)
		{
			if (myData.MapBuildableLayer[i] != null)
			{
				BuildablesInMap[i] = myData.MapBuildableLayer[i];
				
				var newAssetPath = ListOfBuildables.FindByName(BuildablesInMap[i].Name).AssetPath;
				var temp = Resources.Load(newAssetPath,typeof(GameObject)) as GameObject;
				
				BuildableSceneObjects[i] = GameObject.Instantiate(temp, BuildablesInMap[i].Position, Quaternion.Euler(BuildablesInMap[i].Rotation) ) as GameObject;
				
				var newBuildable = new MapBuildable();
				newBuildable.Name = BuildablesInMap[i].Name;
				newBuildable.Position = BuildablesInMap[i].Position;
				newBuildable.Rotation = BuildablesInMap[i].Rotation;
				BuildablesInMap[i] = newBuildable;
				
				DisableComponentsForEditor();
				
			}
		}
		
		myData.TerrainBuildableLayer = PlayerBlob.GetComponent<PlayerBlobManager>().TerrainInMap;
		for(var i = 0; i < 400; i++)
		{
			if (myData.TerrainBuildableLayer[i] != null)
			{
				TerrainInMap[i] = myData.TerrainBuildableLayer[i];
				
				var newAssetPath = ListOfBuildables.FindByName(TerrainInMap[i].Name).AssetPath;
				var temp = Resources.Load(newAssetPath,typeof(GameObject)) as GameObject;
				
				
				var TileX = (int)(i/MapSizeX);
				var TileY = (int)((i % MapSizeY));
//					print ("x = " + TileX + " y = " + TileY);
				NewMapTerrainLayer[TileX,TileY] = (GameObject) Instantiate(temp, TerrainInMap[i].Position, Quaternion.Euler(TerrainInMap[i].Rotation) );				
			}
		}		
	}
/*	
	void LoadMap()
	{
		LoadXML(); 
		if(_data.ToString() != "") 
		{ 
			// notice how I use a reference to type (UserData) here, you need this 
			// so that the returned object is converted into the correct type 

			myData = (SaveData)DeserializeObject(_data); 

			// set buildablelayer
			for(var i = 0; i < 400; i++)
			{
				if (myData.MapBuildableLayer[i] != null)
				{
					BuildablesInMap[i] = myData.MapBuildableLayer[i];
					
					var newAssetPath = ListOfBuildables.FindByName(BuildablesInMap[i].Name).AssetPath;
					var temp = Resources.Load(newAssetPath,typeof(GameObject)) as GameObject;
					
//					print (Quaternion.Euler(BuildablesInMap[i].Rotation));
					
					BuildableSceneObjects[i] = GameObject.Instantiate(temp, BuildablesInMap[i].Position, Quaternion.Euler(BuildablesInMap[i].Rotation) ) as GameObject;
					
					var newBuildable = new MapBuildable();
					newBuildable.Name = BuildablesInMap[i].Name;
					newBuildable.Position = BuildablesInMap[i].Position;
					newBuildable.Rotation = BuildablesInMap[i].Rotation;
					BuildablesInMap[i] = newBuildable;
					
					DisableComponentsForEditor();
					
					
				}
				else
				{
					BuildablesInMap[i] = null;
				}
			}

			// set TerrainLayer
			for(var i = 0; i < 400; i++)
			{
				if (myData.TerrainBuildableLayer[i] != null)
				{
					TerrainInMap[i] = myData.TerrainBuildableLayer[i];
					
					var newAssetPath = ListOfBuildables.FindByName(TerrainInMap[i].Name).AssetPath;
					var temp = Resources.Load(newAssetPath,typeof(GameObject)) as GameObject;
					
					
					var TileX = (int)(i/MapSizeX);
					var TileY = (int)((i % MapSizeY));
//					print ("x = " + TileX + " y = " + TileY);
					NewMapTerrainLayer[TileX,TileY] = (GameObject) Instantiate(temp, TerrainInMap[i].Position, Quaternion.Euler(TerrainInMap[i].Rotation) );
				}
			}
	  	}
	}
*/	
	public void SaveMap()
	{
		UpdateTerrainMapSaveData();
		UpdateInventorySaveData();
		
		myData.MapBuildableLayer = BuildablesInMap;
		myData.TerrainBuildableLayer = TerrainInMap;
/*
		// Time to creat our XML! 
		_data = SerializeObject(myData); 
		
		// This is the final resulting XML from the serialization process 
		CreateXML(); 
*/
		var PlayerBlob = GameObject.Find("PlayerBlobManager_Prefab");
		PlayerBlob.GetComponent<PlayerBlobManager>().SaveMapData(BuildablesInMap, TerrainInMap);
		
	}
	
	void UpdateInventorySaveData()
	{
		var PlayerBlob = GameObject.Find("PlayerBlobManager_Prefab");
		myData.Inventory = PlayerBlob.GetComponent<PlayerBlobManager>().Inventory;
	}
	
	void UpdateTerrainMapSaveData()
	{
		var count = 0;
		for (var x = 0; x < MapSizeX; x++)
		{
			for (var y = 0; y < MapSizeY; y++)
			{
//				print (NewMapTerrainLayer[x,y]);
				
				if(NewMapTerrainLayer[x,y] == null)
				{
					TerrainInMap[count] = null;
					print ("ERROR:" + x + ", "+ y);
				}
				else
				{
					var newTerrainSaveData = new MapBuildable();
					newTerrainSaveData.Name = ((NewMapTerrainLayer[x,y].name.Substring( 0,NewMapTerrainLayer[x,y].name.Length - 7))); // NewMapTerrainLayer[TileX,TileY].name;
					newTerrainSaveData.Position = NewMapTerrainLayer[x,y].transform.position;
					newTerrainSaveData.Rotation = NewMapTerrainLayer[x,y].transform.rotation.eulerAngles;	
					TerrainInMap[count] = newTerrainSaveData;
				}
				count+=1;
			}
		}
	}
	
	void OnGUI() 
	{ 
/*		
		//*************************************************** 
		// Loading The Player... 
		// **************************************************       
		
		if (GUI.Button(_Load,"Load")) 
		{ 
			GUI.Label(_LoadMSG,"Loading from: "+_FileLocation); 
			// Load our UserData into myData 
//			LoadMap();
			LoadMapFromPlayerBlobManager();
		} 
			
		//*************************************************** 
		// Saving The Player... 
		// **************************************************    
		if (GUI.Button(_Save,"Save")) 
		{ 
		
			GUI.Label(_SaveMSG,"Saving to: "+_FileLocation); 
			SaveMap();
		}	
*/		

		
	}	

/*	
// The following metods came from the referenced URL  
	string UTF8ByteArrayToString(byte[] characters) 
	{      
	  UTF8Encoding encoding = new UTF8Encoding(); 
	  string constructedString = encoding.GetString(characters); 
	  return (constructedString); 
	} 
	
	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
	  UTF8Encoding encoding = new UTF8Encoding(); 
	  byte[] byteArray = encoding.GetBytes(pXmlString); 
	  return byteArray; 
	} 

	// Here we deserialize it back into its original form 
	object DeserializeObject(string pXmlizedString) 
	{ 
		// type of need to be the type of data we are saving
//		XmlSerializer xs = new XmlSerializer(typeof(MapBuildable[])); 
		XmlSerializer xs = new XmlSerializer(typeof(SaveData)); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(memoryStream); 
	} 
	
	string SerializeObject(object pObject) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		// type of need to be the type of data we are saving
//		XmlSerializer xs = new XmlSerializer(typeof(MapBuildable[])); 
		XmlSerializer xs = new XmlSerializer(typeof(SaveData)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xmlTextWriter.Settings.Indent = true;
		xmlTextWriter.Formatting = Formatting.Indented ;
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
	
	void CreateXML() 
	{ 
	  StreamWriter writer; 
	  FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName); 
	  if(!t.Exists) 
	  { 
	     writer = t.CreateText(); 
	  } 
	  else 
	  { 
	     t.Delete(); 
	     writer = t.CreateText(); 
	  } 
	  writer.Write(_data); 
	  writer.Close(); 
	  Debug.Log("File written."); 
	} 

	void LoadXML() 
	{ 
	  StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName); 
	  string _info = r.ReadToEnd(); 
	  r.Close(); 
	  _data=_info; 
	  Debug.Log("File Read"); 
	} 	
*/
	void InitBuildableLayer()
	{
		// init Object Layer		
		for (var x = 0; x < MapSizeX; x++)
		{
			for (var y = 0; y < MapSizeY; y++)
			{
				MapBuildableLayer[x,y] = null;				
			}
		}	
	}
	
	void FillBuildableLayer()
	{
		var count = 0;
		// fill scene with object layer objects
		for (var x = 0; x < MapSizeX; x++)
		{
			for (var y = 0; y < MapSizeY; y++)
			{
				if (MapBuildableLayer[x,y] != null)
				{
					var temp = Resources.Load(MapBuildableLayer[x,y].AssetPath,typeof(GameObject)) as GameObject;
					BuildableSceneObjects[count] = (GameObject) Instantiate(temp, new Vector3(x * MapTileSize,0,y * MapTileSize), Quaternion.identity );
					
					var newBuildable = new MapBuildable();
					newBuildable.Name = BuildableSceneObjects[count].name;
					newBuildable.Position = new Vector3(x * MapTileSize,0,y * MapTileSize);
					newBuildable.Rotation = Quaternion.identity.eulerAngles ;
					BuildablesInMap[count] = newBuildable;
					
					count +=1;
				}
				else
				{
					BuildableSceneObjects[count] = null;
					BuildablesInMap[count] = null;
				}
			}
		}		
	}	
	
	void InitTerrainLayer()
	{
		// init Terrain Layer		
		for (var x = 0; x < MapSizeX; x++)
		{
			for (var y = 0; y < MapSizeY; y++)
			{
				var temp = Resources.Load(DefaultTerrain.AssetPath,typeof(GameObject)) as GameObject;		
				NewMapTerrainLayer[x,y] = (GameObject) Instantiate( temp,new Vector3(x * MapTileSize,0,y * MapTileSize), Quaternion.identity );
				
			}
		}		
	}
	
	void FillTerrainLayer()
	{
		var count = 0;
		// fill scene with default terrain layer objects
		for (var x = 0; x < MapSizeX; x++)
		{
			for (var y = 0; y < MapSizeY; y++)
			{
				var temp = Resources.Load(DefaultTerrain.AssetPath,typeof(GameObject)) as GameObject;		
				NewMapTerrainLayer[x,y] = (GameObject) Instantiate( temp,new Vector3(x * MapTileSize,0,y * MapTileSize), Quaternion.identity );
				count +=1;
			}
		}		
	}
	
	public void DisableComponentsForEditor()
	{
		object[] AllObjects = GameObject.FindSceneObjectsOfType(typeof (GameObject));

		foreach(Object Obj in AllObjects)
		{
			GameObject GObj = (GameObject) Obj;
			if(GObj.GetComponent<Rigidbody>() != null)
			{
				Destroy(GObj.GetComponent<Rigidbody>());
			}
			if(GObj.GetComponent<BaddieAI>() != null)
			{
				GObj.GetComponent<BaddieAI>().enabled = false;
			}		
		}
		
	}

	public void FillTerrainWithEmptyTerrain(Vector3 myPos, Vector2 mySize)
	{
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;

//		DestroyTerrainSceneObjects(myPos, mySize);
		
		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);
				var newPos = (MapTileToPosition( new Vector2(tileX,tileY), EmptyTerrain.TileSize));
				NewMapTerrainLayer[tileX,tileY] = (GameObject) Instantiate( Resources.Load(EmptyTerrain.AssetPath,  typeof(GameObject)), (MapTileToPosition( new Vector2(tileX,tileY), EmptyTerrain.TileSize)), Quaternion.identity );	
			}
		}		
	}

	public void FillTerrainWithDefaultTerrain(Vector3 myPos, Vector2 mySize)
	{
//		Buildable DefaultTerrain = PlayerMapObject.GetComponent<PlayerMap>().DefaultTerrain;
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;

//		DestroyTerrainSceneObjects(myPos, mySize);
		
		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);
				
				NewMapTerrainLayer[tileX,tileY] = (GameObject) Instantiate( Resources.Load(DefaultTerrain.AssetPath,  typeof(GameObject)),(MapTileToPosition( new Vector2(tileX,tileY), DefaultTerrain.TileSize)), Quaternion.identity );	
			}
		}		
	}
	
	public void DestroyTerrainSceneObjects(Vector3 myPos, Vector2 mySize)
	{
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;
		
		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);
//				print (NewMapTerrainLayer[tileX,tileY].gameObject);
				Destroy(NewMapTerrainLayer[tileX,tileY].gameObject);
				NewMapTerrainLayer[tileX,tileY] = null;
			}
		}			
	}
	
	Vector3 MapTileToPosition( Vector2 myTile, Vector2 Size)
	{
		var newX = (myTile.x * MapTileSize) + ((Size.x - 1) * (MapTileSize / 2));
		var newZ = (myTile.y * MapTileSize) + ((Size.y - 1) * (MapTileSize / 2));
		return (new Vector3 (newX,0,newZ));
	}

	Vector2 PositionToMapTile( Vector3 myPos, Vector2 Size)
	{
		var newX = (myPos.x / MapTileSize) - ((Size.x - 1) / 2);
		var newY = (myPos.z / MapTileSize) - ((Size.y - 1) / 2);
		return (new Vector2 (newX,newY));
	}

	public void AddBuildableByAssetPath(string BuildableAssetPath)
	{
		var newBuildable = ListOfBuildables.FindByAssetPath(BuildableAssetPath);
		var Pos = MapTileToPosition(new Vector2(0, 0), newBuildable.TileSize);
		
		MapBuildableLayer[0,0] = newBuildable;
		BuildableSceneObjects[0] = (GameObject) Instantiate( Resources.Load(BuildableAssetPath,  typeof(GameObject)),Pos, Quaternion.identity ) as GameObject;
		if(BuildableSceneObjects[0].GetComponent<Rigidbody>() != null)
		{
			Destroy(BuildableSceneObjects[0].GetComponent<Rigidbody>());
		}
		if(BuildableSceneObjects[0].GetComponent<BaddieAI>() != null)
		{
			BuildableSceneObjects[0].GetComponent<BaddieAI>().enabled = false;
		}
		
		SaveMap();
	}

	public void AddBuildableByName(string BuildableName)
	{
		var newBuildable = ListOfBuildables.FindByName(BuildableName);
		var Pos = MapTileToPosition(new Vector2(0, 0), newBuildable.TileSize);
		
		MapBuildableLayer[0,0] = newBuildable;
		BuildableSceneObjects[0] = (GameObject) Instantiate( Resources.Load(BuildableName,  typeof(GameObject)),Pos, Quaternion.identity ) as GameObject;
		if(BuildableSceneObjects[0].GetComponent<Rigidbody>() != null)
		{
			Destroy(BuildableSceneObjects[0].GetComponent<Rigidbody>());
		}
		
		SaveMap();
	}
	
	public void AddTerrainByAssetPath(string BuildableAssetPath)
	{
		var newBuildable = ListOfBuildables.FindByAssetPath(BuildableAssetPath);
		var Pos = MapTileToPosition(new Vector2(0,0), newBuildable.TileSize);
		
		DestroyTerrainSceneObjects(Pos, newBuildable.TileSize);
		
		FillTerrainWithEmptyTerrain(Pos, newBuildable.TileSize);
		
		if(NewMapTerrainLayer[0,0] != null)
		{
//			print (NewMapTerrainLayer[0,0].name);
			Destroy(NewMapTerrainLayer[0,0].gameObject);
		}
		
		NewMapTerrainLayer[0,0] = (GameObject) Instantiate( Resources.Load(BuildableAssetPath,  typeof(GameObject)),Pos, Quaternion.identity );
		
		SaveMap();
	}

	public void AddTerrainByName(string BuildableName)
	{
		var newBuildable = ListOfBuildables.FindByName(BuildableName);
		var Pos = MapTileToPosition(new Vector2(0, 0), newBuildable.TileSize);
		
		if(NewMapTerrainLayer[0,0] != null)
		{
			print (NewMapTerrainLayer[0,0].name);
			Destroy(NewMapTerrainLayer[0,0].gameObject);
		}
		
		NewMapTerrainLayer[0,0] = (GameObject) Instantiate( Resources.Load(BuildableName,  typeof(GameObject)),Pos, Quaternion.identity );
		if(NewMapTerrainLayer[0,0].GetComponent<Rigidbody>() != null)
		{
			Destroy(NewMapTerrainLayer[0,0].GetComponent<Rigidbody>());
		}
		
		SaveMap();
	}
}

