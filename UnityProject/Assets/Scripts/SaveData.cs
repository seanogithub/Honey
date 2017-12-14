using UnityEngine;
using System.Collections;

public class SaveData {
	
	public MapBuildable[] MapBuildableLayer = new MapBuildable[400];
	public MapBuildable[] TerrainBuildableLayer = new MapBuildable[400];
	public PlayerInventory Inventory = new PlayerInventory();
	public PlayerData PlayerAccountData = new PlayerData();
	
	public SaveData() 
	{
		MapBuildableLayer = new MapBuildable[400];
		TerrainBuildableLayer = new MapBuildable[400];
		PlayerInventory Inventory = new PlayerInventory();
		PlayerAccountData = new PlayerData();
	}
	
	
}
