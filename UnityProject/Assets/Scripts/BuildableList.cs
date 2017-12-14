using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildableList {

	public Buildable[] myListOfBuildables = new Buildable[20];
	
	void Awake () 
	{
		Init();
	}

	public Buildable FindByCode(string myCode)
	{
		for (var i = 0; i < myListOfBuildables.Length; i++)
		{
			if(myCode == myListOfBuildables[i].Code)
			{
				return ( myListOfBuildables[i]);
			}
		}
//		print("WARNING: no objects found");
		return null;
	}

	public Buildable FindByName(string myName)
	{
		for (var i = 0; i < myListOfBuildables.Length; i++)
		{
			if(myName == myListOfBuildables[i].Name)
			{
				return ( myListOfBuildables[i]);
			}
		}
//		print("WARNING: no objects found");
		return null;
	}

	public Buildable FindByAssetPath(string myPath)
	{
		for (var i = 0; i < myListOfBuildables.Length; i++)
		{
			if(myPath == myListOfBuildables[i].AssetPath)
			{
				return ( myListOfBuildables[i]);
			}
		}
//		print("WARNING: no objects found");
		return null;
	}
	
	public void Init()
	{
// this should be read in from an XMl file on the server.		
		var count = (int)0;

		var temp = new Buildable();
		temp.Code = "TP00";
		temp.AssetPath = "Terrain_Parts/Terrain_Flat/Terrain_Empty_Prefab";
		temp.Name = "Terrain_Empty_Prefab";
		temp.Type = "TerrainEmpty";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = false;
		temp.Sellable = false;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 0;
		temp.CoinCost = 0;
		myListOfBuildables[count] = temp;
		count +=1;		
		
		temp = new Buildable();
		temp.Code = "TP01";
		temp.AssetPath = "Terrain_Parts/Terrain_Flat/Terrain_Flat_Prefab";
		temp.Name = "Terrain_Flat_Prefab";
		temp.Type = "TerrainAdd";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 0;
		temp.CoinCost = 0;		
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "TP02";
		temp.AssetPath = "Terrain_Parts/Terrain_Hill/Terrain_Hill_Prefab";
		temp.Name = "Terrain_Hill_Prefab";
		temp.Type = "TerrainAdd";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;		
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "TP03";
		temp.AssetPath = "Terrain_Parts/Terrain_Hill_01/Terrain_Hill_01_Prefab";
		temp.Name = "Terrain_Hill_01_Prefab";
		temp.Type = "TerrainAdd";
		temp.TileSize = new Vector2(4,4);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;
		
		temp = new Buildable();
		temp.Code = "TP04";
		temp.AssetPath = "Terrain_Parts/Terrain_Hill_02/Terrain_Hill_02_Prefab";
		temp.Name = "Terrain_Hill_02_Prefab";
		temp.Type = "TerrainAdd";
		temp.TileSize = new Vector2(2,2);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "TP05";
		temp.AssetPath = "Terrain_Parts/Terrain_Cliff_01/Terrain_Cliff_01_Prefab";
		temp.Name = "Terrain_Cliff_01_Prefab";
		temp.Type = "TerrainAdd";
		temp.TileSize = new Vector2(4,4);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "TP05";
		temp.AssetPath = "Terrain_Parts/Terrain_Pond_01/Terrain_Pond_01_Prefab";
		temp.Name = "Terrain_Pond_01_Prefab";
		temp.Type = "TerrainAdd";
		temp.TileSize = new Vector2(5,5);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;
		
		temp = new Buildable();
		temp.Code = "PU01";
		temp.AssetPath = "DropOffs/DropOffs_Rocks_01/DropOffs_Rocks_01_Prefab";
		temp.Name = "DropOffs_Rocks_01_Prefab";
		temp.Type = "DropOff";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = true;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;
		
		temp = new Buildable();
		temp.Code = "PU01";
		temp.AssetPath = "PickUps/PickUps_Acorn_01/PickUps_Acorn_01_Prefab";
		temp.Name = "PickUps_Acorn_01_Prefab";
		temp.Type = "PickUp";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 20;
		temp.CoinCost = 20;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "PU02";
		temp.AssetPath = "PickUps/PickUps_Raspberry/PickUps_Raspberry_Prefab";
		temp.Name = "PickUps_Raspberry_Prefab";
		temp.Type = "PickUp";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 20;
		temp.CoinCost = 20;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "PU03";
		temp.AssetPath = "PickUps/PickUps_Strawberry/PickUps_Strawberry_Prefab";
		temp.Name = "PickUps_Strawberry_Prefab";
		temp.Type = "PickUp";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 20;
		temp.CoinCost = 20;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "D001";
		temp.AssetPath = "Decorations/Deco_Mushroom_01/Deco_Mushroom_01_Prefab";
		temp.Name = "Deco_Mushroom_01_Prefab";
		temp.Type = "Decoration";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "D002";
		temp.AssetPath = "Decorations/Deco_Plant_01/Deco_Plant_01_Prefab";
		temp.Name = "Deco_Plant_01_Prefab";
		temp.Type = "Decoration";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "D003";
		temp.AssetPath = "Decorations/Deco_Plant_02/Deco_Plant_02_Prefab";
		temp.Name = "Deco_Plant_02_Prefab";
		temp.Type = "Decoration";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "D004";
		temp.AssetPath = "Decorations/Deco_Plant_04/Deco_Plant_04_Prefab";
		temp.Name = "Deco_Plant_04_Prefab";
		temp.Type = "Decoration";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "D005";
		temp.AssetPath = "Decorations/Deco_Tree_01/Deco_Tree_01_Prefab";
		temp.Name = "Deco_Tree_01_Prefab";
		temp.Type = "Decoration";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;

		temp = new Buildable();
		temp.Code = "D005";
		temp.AssetPath = "Decorations/Deco_Tree_01/Deco_Tree_01_Prefab";
		temp.Name = "Deco_Tree_01_Prefab";
		temp.Type = "Decoration";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 10;
		temp.CoinCost = 10;			
		myListOfBuildables[count] = temp;
		count +=1;
		
		temp = new Buildable();
		temp.Code = "BA001";
		temp.AssetPath = "Baddies/Baddies_Bunny/Baddies_Bunny_Prefab";
		temp.Name = "Baddies_Bunny_Prefab";
		temp.Type = "Baddie";
		temp.TileSize = new Vector2(1,1);
		temp.Placeable = true;
		temp.Sellable = true;
		temp.RequiredForMap = false;
		temp.HoneyPointCost = 50;
		temp.CoinCost = 50;			
		myListOfBuildables[count] = temp;
		count +=1;		
		
	}
}
