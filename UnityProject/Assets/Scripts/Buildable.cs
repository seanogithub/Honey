using UnityEngine;
using System.Collections;

//[Serializable]
public class Buildable {
	
	public string Code;
	public string AssetPath;
	public string Name;
	public string Type;
	public Vector2 TileSize;
	public bool Placeable;
	public bool Sellable;
	public bool RequiredForMap;
	public int HoneyPointCost;
	public int CoinCost;

	public Buildable() 
	{
	}
	
}
