using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text;

public class PlayerBlobManager : MonoBehaviour {

	public int MapSizeX = 20;
	public int MapSizeY = 20;
	public int MapTileSize = 2;
	public BuildableList ListOfBuildables = new BuildableList();
	public GameObject[] BuildableSceneObjects = new GameObject[400];
	public GameObject[,] NewMapTerrainLayer = new GameObject[20, 20];
	
	public MapBuildable[] BuildablesInMap = new MapBuildable[400];
	public MapBuildable[] TerrainInMap = new MapBuildable[400];

	public PlayerInventory Inventory = new PlayerInventory();
	
	public PlayerData PlayerAccountData = new PlayerData();
	
	SaveData myData = new SaveData();

	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 	 	
	string _data; 
	
	
	void Awake()
	{
		_FileLocation=Application.persistentDataPath; 
		_FileName="SaveData.xml";  
		
		// we need soemthing to store the information into 
		myData=new SaveData(); 	

		if(File.Exists(_FileLocation+"\\"+ _FileName))
		{
			LoadPlayerData();
			print (PlayerAccountData.playerHoneyPoints.ToString());
		}
/*
		Inventory.InventoryList[0].Quantity += 1;
		for (int i = 0; i < Inventory.InventoryList.Count; i ++)
		{
			print(Inventory.InventoryList[i].Name + " " + Inventory.InventoryList[i].Quantity);
		}
*/		
/*				
		if(File.Exists(_FileLocation+"\\"+ _FileName))
		{
			LoadMap();
		}
		else
		{
			InitTerrainLayer();
			print("WARNING: SAVE FILE IS MISSING!");
		}
*/
	}

	public void LoadPlayerData()
	{
		LoadXML(); 
		if(_data.ToString() != "") 
		{ 
			myData = (SaveData)DeserializeObject(_data); 

			// set buildablelayer
			for(var i = 0; i < 400; i++)
			{
				if (myData.MapBuildableLayer[i] != null)
				{
					BuildablesInMap[i] = myData.MapBuildableLayer[i];
				}
			}
			// set TerrainLayer
			for(var i = 0; i < 400; i++)
			{
				if (myData.TerrainBuildableLayer[i] != null)
				{
					TerrainInMap[i] = myData.TerrainBuildableLayer[i];
				}
			}
			// set Inventory
			Inventory.InventoryList.Clear();
			for(var i = 0; i < myData.Inventory.InventoryList.Count; i++)
			{
				Inventory.InventoryList.Add(myData.Inventory.InventoryList[i]);
			}
			// set Account Data
			PlayerAccountData = myData.PlayerAccountData;
			
		}		
	}
	
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
	
	public void SavePlayerData()
	{
		// Time to creat our XML! 
		_data = SerializeObject(myData); 
		
		// This is the final resulting XML from the serialization process 
		CreateXML(); 
	}

	public void SaveMapData(MapBuildable[] BuildablesInMap, MapBuildable[] TerrainInMap)
	{

		myData.MapBuildableLayer = BuildablesInMap;
		myData.TerrainBuildableLayer = TerrainInMap;
		myData.Inventory = Inventory;
		myData.PlayerAccountData = PlayerAccountData;
		
		SavePlayerData();
	}

	public void SaveAccountData()
	{

		myData.MapBuildableLayer = BuildablesInMap;
		myData.TerrainBuildableLayer = TerrainInMap;
		myData.Inventory = Inventory;
		myData.PlayerAccountData = PlayerAccountData;
		
		SavePlayerData();
	}
	
	
	
	/* The following metods came from the referenced URL */ 
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
	// Update is called once per frame
	void Update () 
	{
	
	}
}
