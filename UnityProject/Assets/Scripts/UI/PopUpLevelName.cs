using UnityEngine;
using System.Collections;

public class PopUpLevelName : MonoBehaviour {

	public Texture2D LevelNameBitmap;
	private Vector4 LevelNameBitmapSize = new Vector4( 0.3f, 0.3f, 0.4f, 0.4f); // x-pos,y-pos,x-size,y-size

	void OnGUI()
	{
		GUI.DrawTexture(new Rect ((Screen.width * LevelNameBitmapSize.x ), (Screen.height * LevelNameBitmapSize.y), (Screen.width * LevelNameBitmapSize.z) , (Screen.height * LevelNameBitmapSize.w)), LevelNameBitmap,ScaleMode.ScaleToFit,true);
	}
}
