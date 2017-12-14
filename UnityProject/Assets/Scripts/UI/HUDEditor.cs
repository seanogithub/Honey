using UnityEngine;
using System.Collections;

public class HUDEditor : MonoBehaviour {
	
	private GUIStyle style;
	public Font myFont;
	public int myFontSize = 60;
	public Texture2D ButtonCloseBitmap;
	private Vector4 ButtonCloseBitmapSize = new Vector4( 0.96f, 0.02f, 0.1f, 0.1f); // x-pos,y-pos,x-size,y-size
	private float AspectRatioMultiplier = 1.5f;
	
	// Use this for initialization
	void Start () 
	{
		// Scale Font to Screen Size
		style = new GUIStyle();
		style.font = myFont;
		style.fontSize = Mathf.RoundToInt((float)myFontSize * (float)Screen.width / 1920);
		style.normal.textColor = Color.white;		

		AspectRatioMultiplier = ((float)Screen.width/1920 );
	}

	void OnGUI()
	{
		if (GUI.Button( new Rect (((float)Screen.width * ButtonCloseBitmapSize.x ),((float)Screen.height * ButtonCloseBitmapSize.y ),ButtonCloseBitmap.width * AspectRatioMultiplier , ButtonCloseBitmap.height * AspectRatioMultiplier) ,ButtonCloseBitmap,style))
		{
			Application.LoadLevel("MainMenu");
		}			
	}
	
}
