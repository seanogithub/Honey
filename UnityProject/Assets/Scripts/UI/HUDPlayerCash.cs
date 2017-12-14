using UnityEngine;
using System.Collections;

public class HUDPlayerCash : MonoBehaviour {
	
	private int HUDCash;
	private int HUDHoneyPoints;
	private GameObject PlayerBlob;
	private GameObject player;
	public bool Paused = false;
	private GUIStyle style;
	public Font myFont;
	public int myFontSize = 50;
	public Texture2D HUDCashBitmap;	
	private Vector2 HUDCashBitmapSize = new Vector2( 0.60f, 0.0f); // x-pos,y-pos
	private Vector2 HUDCashValueSize = new Vector2( 0.66f, 0.035f); // x-pos,y-pos
	public Texture2D HUDHoneyPointsBitmap;	
	private Vector2 HUDHoneyPointsBitmapSize = new Vector2( 0.75f, 0.0f); // x-pos,y-pos,x-size,y-size
	private Vector2 HUDHoneyPointsValueSize = new Vector2( 0.81f, 0.035f); // x-pos,y-pos,x-size,y-size
	private float AspectRatioMultiplier = 1.5f;
	
	// Use this for initialization
	void Start () 
	{
		PlayerBlob = GameObject.Find("PlayerBlobManager_Prefab");	
		player = GameObject.FindGameObjectWithTag(Tags.player);

		// Scale Font to Screen Size
		style = new GUIStyle();
		style.font = myFont;
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		style.normal.textColor = Color.black;
		AspectRatioMultiplier = ((float)Screen.width/1920 );

	}

	public void Pause (bool myValue)
	{
		Paused = myValue;
	}
	
	void OnGUI()
	{
		if (Paused == false)
		{
			HUDCash = PlayerBlob.GetComponent<PlayerBlobManager>().PlayerAccountData.playerCash;
			HUDHoneyPoints = PlayerBlob.GetComponent<PlayerBlobManager>().PlayerAccountData.playerHoneyPoints;
//			HUDCash = player.GetComponent<PlayerStats>().playerCash;
//			HUDHoneyPoints = player.GetComponent<PlayerStats>().playerHoneyPoints;
			
		}
		
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		AspectRatioMultiplier = ((float)Screen.width/1920 );

//		var tempTexture = (new Rect(((float)Screen.width * HUDCashBitmapSize.x ),((float)Screen.height * HUDCashBitmapSize.y ),HUDCashBitmap.width * ass,HUDCashBitmap.height * AspectRatioMultiplier));
//		var tempValue = (new Rect(((float)Screen.width * HUDCashValueSize.x ),((float)Screen.height * HUDCashValueSize.y ),(float)style.fontSize * ass,(float)style.fontSize * AspectRatioMultiplier));

//		print (style.fontSize);
//		print (tempTexture);
//		print (tempValue);
		
		// draw HUDCashBitmap
		GUI.DrawTexture((new Rect(((float)Screen.width * HUDCashBitmapSize.x ),((float)Screen.height * HUDCashBitmapSize.y ),HUDCashBitmap.width * AspectRatioMultiplier,HUDCashBitmap.height * AspectRatioMultiplier)), HUDCashBitmap,ScaleMode.StretchToFill,true);
		GUI.Label((new Rect(((float)Screen.width * HUDCashValueSize.x ),((float)Screen.height * HUDCashValueSize.y ),(float)style.fontSize * AspectRatioMultiplier,(float)style.fontSize * AspectRatioMultiplier)), HUDCash.ToString(), style);
//		GUI.Label(new Rect ( (Screen.width * HUDCashValueSize.x),(Screen.height * HUDCashValueSize.y),(Screen.width * HUDCashValueSize.z),(Screen.height * HUDCashValueSize.w)   ), HUDCash.ToString(), style);		

		// draw HUDHoneyPointsBitmap
		GUI.DrawTexture((new Rect(((float)Screen.width * HUDHoneyPointsBitmapSize.x ),((float)Screen.height * HUDHoneyPointsBitmapSize.y ),HUDHoneyPointsBitmap.width * AspectRatioMultiplier,HUDHoneyPointsBitmap.height * AspectRatioMultiplier)), HUDHoneyPointsBitmap,ScaleMode.StretchToFill,true);
		GUI.Label((new Rect(((float)Screen.width * HUDHoneyPointsValueSize.x ),((float)Screen.height * HUDHoneyPointsValueSize.y ),(float)style.fontSize * AspectRatioMultiplier,(float)style.fontSize * AspectRatioMultiplier)), HUDHoneyPoints.ToString(), style);

		
//		GUI.DrawTexture(new Rect ((Screen.width * HUDHoneyPointsBitmapSize.x ) - (Screen.width * HUDHoneyPointsBitmapSize.z), (Screen.height * HUDHoneyPointsBitmapSize.y), (Screen.width * HUDHoneyPointsBitmapSize.z) , (Screen.height * HUDHoneyPointsBitmapSize.w)), HUDHoneyPointsBitmap,ScaleMode.ScaleToFit,true);
//		GUI.Label(new Rect ( (Screen.width * HUDHoneyPointsValueSize.x),(Screen.height * HUDHoneyPointsValueSize.y),(Screen.width * HUDHoneyPointsValueSize.z),(Screen.height * HUDHoneyPointsValueSize.w)   ), HUDHoneyPoints.ToString(), style);		
	}
}
