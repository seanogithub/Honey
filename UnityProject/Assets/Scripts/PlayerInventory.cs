using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory  {
	
	public List<InventoryItem> InventoryList = new List<InventoryItem>();
	
	public void Init ()
	{
		InventoryList.Clear();
/*
		var shit = new InventoryItem();
		shit.Name = "shit";
		shit.Quantity = 1; 

		InventoryList.Add( shit );
*/		
	}
	
	public int FindIndexByName ( string myName)
	{
		for (int i = 0; i < InventoryList.Count; i ++)
		{
			if (myName == InventoryList[i].Name)
			{
				return i;
			}
		}		
		return (InventoryList.Count + 1);
	}

}
