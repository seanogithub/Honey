using UnityEngine;
using System.Collections;


[RequireComponent(typeof(GUITexture))]
public class HUDScreenFader : MonoBehaviour
{
    public float fadeSpeed = 1f;          // Speed that the screen fades to and from black.
    
    void Awake ()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }
    
    
    void Update ()
    {
		GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, fadeSpeed * Time.deltaTime);	
		if (GetComponent<GUITexture>().color.a < 0.05f)
		{
			Destroy(this.gameObject, 0.5f);
		}
	}
    
}