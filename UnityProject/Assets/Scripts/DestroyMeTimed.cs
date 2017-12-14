using UnityEngine;
using System.Collections;

public class DestroyMeTimed : MonoBehaviour {
	
	public float DestroyTime = 3.0f;
	
	// Use this for initialization
	void Awake () 
	{
		Destroy(this.gameObject, DestroyTime);
	}

}
