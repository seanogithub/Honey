using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class Button : MonoBehaviour {

	public bool ButtonPressed = false;
	public Vector2 ButtonPressedPosition;
	private GUITexture gui;
	public Rect touchZone;
	private int ScreenWidthX ;
	
	
	private PlayerStats changeScoreScript;    // Reference to the player GameObject.
	
	// Use this for initialization
	void Awake () 
	{
		ScreenWidthX = Screen.width;
		
        gui = GetComponent<GUITexture>();
        if (gui.texture == null)
        {
            Debug.LogError("Button object requires a valid texture!");
            gameObject.GetComponent<Button>().enabled = false;
            return;
        }	
		touchZone = gui.pixelInset;
        touchZone.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5f;
        touchZone.y += transform.position.y * Screen.height;// - Screen.height * 0.5f;
 
//        transform.position = new Vector3(0, 0, transform.position.z);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Screen.width != ScreenWidthX) 
		{
			touchZone = gui.pixelInset;
	        touchZone.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5f;
	        touchZone.y += transform.position.y * Screen.height;// - Screen.height * 0.5f;			
		}
		ButtonPressed = false;
		int count = Input.touchCount;
        for (int i = 0; i < count; i++)
        {
			Touch touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began)
			{
				if (touchZone.Contains(touch.position))
				{
				ButtonPressedPosition = touch.position;
				ButtonPressed = true;
				}
			}
			if (touch.phase == TouchPhase.Ended)
			{
				ButtonPressed = false;
				print("ButtonReleased ");
			}
		}
	}
}
