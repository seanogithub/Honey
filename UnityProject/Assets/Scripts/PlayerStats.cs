using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public int playerScore = 0 ;
	public int playerHealth = 100 ;
	public int playerCash = 0;
	public int playerHoneyPoints = 0;

	public void addScore (int myValue)
	{
		playerScore += myValue;
	}
	
	public void subScore (int myValue)	
	{	
		playerScore -= myValue;
	}
	
	public void addHealth (int myValue)
	{
		playerHealth += myValue;
	}
	
	public void subHealth (int myValue)	
	{	
		playerHealth -= myValue;
	}

	public void addCash (int myValue)
	{
		playerCash += myValue;
	}
	
	public void subCash (int myValue)	
	{	
		playerCash -= myValue;
	}

	public void addHoneyPoints (int myValue)
	{
		playerHoneyPoints += myValue;
	}
	
	public void subHoneyPoints (int myValue)	
	{	
		playerHoneyPoints -= myValue;
	}
	
}
