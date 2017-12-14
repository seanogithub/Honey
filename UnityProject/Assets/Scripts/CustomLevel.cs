using UnityEngine;
using System.Collections;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text;

public class CustomLevel : MonoBehaviour 
{
	public int MapSizeX = 20;
	public int MapSizeY = 20;
	public int MapTileSize = 2;
	public GameObject PlayerMapObject;
	public Buildable[,] MapBuildableLayer = new Buildable[20, 20];
	public BuildableList ListOfBuildables = new BuildableList();
	public Object[] BuildableSceneObjects = new Object[400];
	public MapBuildable[] BuildablesInMap = new MapBuildable[400];
	public MapBuildable[] TerrainInMap = new MapBuildable[400];
	private Buildable DefaultTerrain;

	public GameObject[,] NewMapTerrainLayer = new GameObject[20, 20];
	
	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 
	string _data; 	
	SaveData myData;
	
	public bool InitCollision = false;
	
	
	void Awake ()
	{
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
		myData=new SaveData(); 		
		
		ListOfBuildables.Init();
		
		DefaultTerrain = new Buildable();
		DefaultTerrain = ListOfBuildables.FindByName("Terrain_Flat_Prefab");

		InitBuildableLayer();
		FillBuildableLayer();
		
		if(File.Exists(_FileLocation+"\\"+ _FileName))
		{
//			LoadCustomLevel();
			LoadMapFromPlayerBlobManager();

		}
		else
		{
			InitTerrainLayer();
			FillTerrainLayer();			
			print("WARNING: SAVE FILE IS MISSING!");
		}
		
		gameObject.AddComponent<MeshCombine>();
	}
/*	
	void Start()
	{
		PlayerMapObject = GameObject.Find("PlayerMap");
		MapSizeX = PlayerMapObject.GetComponent<PlayerMap>().MapSizeX;
		MapSizeY = PlayerMapObject.GetComponent<PlayerMap>().MapSizeY;
		MapTileSize = PlayerMapObject.GetComponent<PlayerMap>().MapTileSize;		
	}
*/	
	void Update()
	{
		if(InitCollision == false)
		{
			gameObject.GetComponent<MeshCombine>().SendMessage("Init");
			InitCollision = true;
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
				
//				DisableComponentsForEditor();
				
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

	void LoadXML() 
	{ 
	  StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName); 
	  string _info = r.ReadToEnd(); 
	  r.Close(); 
	  _data=_info; 
	  Debug.Log("File Read"); 
	} 

	void LoadCustomLevel()
	{

		// Load our UserData into myData 
		LoadXML(); 
		if(_data.ToString() != "") 
		{ 
			// notice how I use a reference to type (UserData) here, you need this 
			// so that the returned object is converted into the correct type 
//			myData = (MapBuildable[])DeserializeObject(_data); 
			myData = (SaveData)DeserializeObject(_data); 

			// set buildablelayer

			for(var i = 0; i < 400; i++)
			{
//				if (myData[i] != null)
				if (myData.MapBuildableLayer[i] != null)
				{
//					BuildablesInMap[i] = myData[i];
					BuildablesInMap[i] = myData.MapBuildableLayer[i];
//					print (BuildablesInMap[i].Name);
					
					var newAssetPath = ListOfBuildables.FindByName(BuildablesInMap[i].Name).AssetPath;
					var temp = Resources.Load(newAssetPath,typeof(GameObject)) as GameObject;
					
//					print (temp);
					BuildableSceneObjects[i] = GameObject.Instantiate(temp, BuildablesInMap[i].Position, Quaternion.Euler(BuildablesInMap[i].Rotation) );
					
					var newBuildable = new MapBuildable();
					newBuildable.Name = BuildablesInMap[i].Name;
					newBuildable.Position = BuildablesInMap[i].Position;
					newBuildable.Rotation = BuildablesInMap[i].Rotation;
					BuildablesInMap[i] = newBuildable;
					
				}
				else
				{
					BuildablesInMap[i] = null;
				}
			}

			// set TerrainLayer
			for(var i = 0; i < 400; i++)
			{
//				if (myData[i] != null)
				if (myData.TerrainBuildableLayer[i] != null)
				{
//					BuildablesInMap[i] = myData[i];
					TerrainInMap[i] = myData.TerrainBuildableLayer[i];
//					print (BuildablesInMap[i].Name);
					
					var newAssetPath = ListOfBuildables.FindByName(TerrainInMap[i].Name).AssetPath;
					var temp = Resources.Load(newAssetPath,typeof(GameObject)) as GameObject;
					
//					print (temp);
					
					var TileX = (int)(i/MapSizeX);
					var TileY = (int)((i % MapSizeY));
//					print ("x = " + TileX + " y = " + TileY);
					NewMapTerrainLayer[TileX,TileY] = (GameObject) Instantiate(temp, TerrainInMap[i].Position, Quaternion.Euler(TerrainInMap[i].Rotation) );
				}
			}
	  	} 
		else
		{
			print ("ERROR: LEVEL NOT LOADED");
		}
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
					BuildableSceneObjects[count] = GameObject.Instantiate(temp, new Vector3(x * MapTileSize,0,y * MapTileSize), Quaternion.identity );
					
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
		// fill scene with terrain layer objects
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
}
