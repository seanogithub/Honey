using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {
	
	public int MapTileSize = 2;
	public string EditMode = "BuildableLayer";
	public string TranslationMode = "Move";
	public bool DeleteNextObject = false ;	
	public bool TouchScreen = false;
	public GameObject SelectedGameObject = null;
	public GameObject PlayerMapObject;
	public GameObject Cursor;
	public float SelectObjectTimer = 1.0f;
	private float SelectTimer;
	public bool SelectObject = false;
	public float zoomSpeed = 10;
	public Vector2 prevTouchPosition = new Vector2(0,0);
	public Vector2 curTouchPosition = new Vector2(0,0);
	
	public float movespeed = 0.005f;
	public Camera selectedCamera;
	public float MINSCALE = 2.0F;
	public float MAXSCALE = 5.0F;
	public float minPinchSpeed = 5.0F;
	public float varianceInDistances = 5.0F;
	private float touchDelta = 0.0F;
	private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);
	private float speedTouch0 = 0.0F;
	private float speedTouch1 = 0.0F;	
	
	private string fuck = "none";
	
	// Use this for initialization
	void Start () 
	{
		PlayerMapObject = GameObject.Find("PlayerMap");
		Cursor = GameObject.Find("Cursor_Editor_Prefab");
		Cursor.GetComponent<MeshRenderer>().enabled = false;
		MapTileSize = PlayerMapObject.GetComponent<PlayerMap>().MapTileSize;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(10, 80, 100, 50), (fuck));
	}	
	
	void ToggleEditMode()
	{
		if (EditMode == "BuildableLayer")
		{
			EditMode = "TerrainLayer";
		}
		else
		{
			EditMode = "BuildableLayer";
		}		
	}

	void ToggleTranslationMode(string newMode)
	{
		TranslationMode = newMode;
	}
	
	Vector3 RoundPostion(Vector3 myPos, Vector2 mySize)
	{
		// check for odd or even tilesize and add the remainder to the position
		var OddOrEven = Mathf.Round( mySize.x % 2);

//		myPos.x = Mathf.RoundToInt((myPos.x/MapTileSize) * MapTileSize);
		myPos.x = OddOrEven + (MapTileSize / 2) + (Mathf.FloorToInt(myPos.x/MapTileSize) * MapTileSize);

		// need to fix this so that buildables use terrain collision for the y value and for terrain parts the y value is set to 0
		myPos.y = 0f;

//		myPos.z = Mathf.RoundToInt((myPos.z/MapTileSize) * MapTileSize);
		myPos.z = OddOrEven + (MapTileSize / 2) + (Mathf.FloorToInt(myPos.z/MapTileSize) * MapTileSize);
		return(myPos);
	}

	Vector3 MapTileToPosition( Vector2 myTile, Vector2 Size)
	{
		var newX = (myTile.x * MapTileSize) + ((Size.x - 1) * (MapTileSize / 2));
		var newZ = (myTile.y * MapTileSize) + ((Size.y - 1) * (MapTileSize / 2));
		return (new Vector3 (newX,0,newZ));
	}

	Vector2 PositionToMapTile( Vector3 myPos, Vector2 Size)
	{
		var newX = (myPos.x / MapTileSize) - ((Size.x - 1) / 2);
		var newY = (myPos.z / MapTileSize) - ((Size.y - 1) / 2);
		return (new Vector2 (newX,newY));
	}
	
//*****************
// Buildable Layers	
//*****************

	bool BuildableLayerAvailable(Vector3 myPos)
	{
		var tileX = (int)(myPos.x/2);
		var tileY = (int)(myPos.z/2);
		if (tileX < 0 || tileX > (PlayerMapObject.GetComponent<PlayerMap>().MapSizeX - 1))
		{
			return false;
		}
		if (tileY < 0 || tileY > (PlayerMapObject.GetComponent<PlayerMap>().MapSizeY - 1))
		{
			return false;
		}
		if( PlayerMapObject.GetComponent<PlayerMap>().MapBuildableLayer[tileX,tileY] == null)
		{
			return(true);
		}
		return false;
	}
	
	void SetMapLayerObject(Vector3 myPos, Quaternion myRot, Buildable myObj)
	{
		var tileX = (int)(myPos.x/2);
		var tileY = (int)(myPos.z/2);
		PlayerMapObject.GetComponent<PlayerMap>().MapBuildableLayer[tileX,tileY] = myObj;
		
		var newBuildable = new MapBuildable();
//		var count = (tileX * (PlayerMapObject.GetComponent<PlayerMap>().MapSizeX -1)) + tileY;
		var count = (tileX * (PlayerMapObject.GetComponent<PlayerMap>().MapSizeX )) + tileY;
		newBuildable.Name = myObj.Name;
//		newBuildable.Position = myPos;
		newBuildable.Position = FindTerrainCollisionPointYAxis(myPos);
		newBuildable.Rotation = myRot.eulerAngles ;
		PlayerMapObject.GetComponent<PlayerMap>().BuildablesInMap[count] = newBuildable;	
		PlayerMapObject.GetComponent<PlayerMap>().SaveMap();
	}

	void ClearMapLayerObject(Vector3 myPos)
	{
		var tileX = (int)(myPos.x/2);
		var tileY = (int)(myPos.z/2);
		PlayerMapObject.GetComponent<PlayerMap>().MapBuildableLayer[tileX,tileY] = null;

		var count = (tileX * (PlayerMapObject.GetComponent<PlayerMap>().MapSizeX )) + tileY;
		PlayerMapObject.GetComponent<PlayerMap>().BuildablesInMap[count] = null;
		
	}
	
	Vector3 FindTerrainCollisionPointYAxis(Vector3 myPos)
	{
		var newPos = new Vector3(0,0,0);
		var ray = new Ray( new Vector3(myPos.x, myPos.y + 20, myPos.z), Vector3.down);
		var hit = new RaycastHit();	
		
//		Debug.DrawRay(ray.origin,ray.direction, Color.red, 5.0f);
		
		if (Physics.Raycast(ray, out hit, 100))
		{
//			print("y axis " + hit.point);
//			print("object " + hit.collider.gameObject.name );
			
			newPos = myPos;
			newPos.y = hit.point.y;
			return(newPos);
		}
		return(myPos);
	}
//*****************
// Terrain Layers	
//*****************
	
	bool TerrainLayerAvailable(Vector3 myPos, Quaternion myRot, Vector2 mySize)
	{
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;
		bool available = false;
		
		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);

				if (tileX < 0 || tileX > (PlayerMapObject.GetComponent<PlayerMap>().MapSizeX - 1))
				{
					return(false);
				}
				if (tileY < 0 || tileY > (PlayerMapObject.GetComponent<PlayerMap>().MapSizeY - 1))
				{
					return(false);
				}
				if (PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY] != null )
				{
					if( PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY].name == "Terrain_Flat_Prefab(Clone)")
					{
						available = true;
					}
					else
					{
						return(false);
					}
				}
				else
				{
					print ("ERROR: NULL");
					available = true;
				}
			}
		}			
		return available;
	}

	void SetTerrainLayerObject(Vector3 myPos, Quaternion myRot, Buildable myObj)
	{
		// fill with empty terrain pieces
		var mySize = myObj.TileSize;
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;
		
		DestroyTerrainSceneObjects(myPos, mySize);
		
		FillTerrainWithEmptyTerrain(myPos, myObj.TileSize);
		
		Destroy(PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY].gameObject);

		PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY] = (GameObject) Instantiate( Resources.Load(myObj.AssetPath,  typeof(GameObject)),myPos, myRot );	
		
		Destroy(SelectedGameObject.gameObject);
		
		PlayerMapObject.GetComponent<PlayerMap>().SaveMap();
	}

	void ClearTerrainLayerObject(Vector3 myPos, Quaternion myRot,  Buildable myObj)
	{
		DestroyEmptyTerrainSceneObjects(myPos, myObj.TileSize);
		FillTerrainWithDefaultTerrain(myPos, myObj.TileSize );
	}

	public void FillTerrainWithEmptyTerrain(Vector3 myPos, Vector2 mySize)
	{
		var EmptyTerrain = PlayerMapObject.GetComponent<PlayerMap>().EmptyTerrain;
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;
		
		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);
				var newPos = (MapTileToPosition( new Vector2(tileX,tileY), EmptyTerrain.TileSize));
				PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY] = (GameObject) Instantiate( Resources.Load(EmptyTerrain.AssetPath,  typeof(GameObject)), (MapTileToPosition( new Vector2(tileX,tileY), EmptyTerrain.TileSize)), Quaternion.identity );	
			}
		}		
	}

	public void FillTerrainWithDefaultTerrain(Vector3 myPos, Vector2 mySize)
	{
		Buildable DefaultTerrain = PlayerMapObject.GetComponent<PlayerMap>().DefaultTerrain;
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;

		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);
				
				PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY] = (GameObject) Instantiate( Resources.Load(DefaultTerrain.AssetPath,  typeof(GameObject)),(MapTileToPosition( new Vector2(tileX,tileY), DefaultTerrain.TileSize)), Quaternion.identity );	
			}
		}		
	}

	public void DestroyEmptyTerrainSceneObjects(Vector3 myPos, Vector2 mySize)
	{
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;
		
		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);
				if (PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY].name == "Terrain_Empty_Prefab(Clone)")
				{
	//				print (PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY].gameObject);
					Destroy(PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY].gameObject);
					PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY] = null;
				}
			}
		}			
	}
	
	public void DestroyTerrainSceneObjects(Vector3 myPos, Vector2 mySize)
	{
		var tileX = (int)PositionToMapTile(myPos, mySize).x;
		var tileY = (int)PositionToMapTile(myPos, mySize).y;
		
		for(var x=0; x < mySize.x; x++)
		{
			for(var y=0; y < mySize.y; y++)
			{
				tileX = (int)(PositionToMapTile(myPos, mySize).x + x);
				tileY = (int)(PositionToMapTile(myPos, mySize).y + y);
//				print (PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY].gameObject);
				Destroy(PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY].gameObject);
				PlayerMapObject.GetComponent<PlayerMap>().NewMapTerrainLayer[tileX,tileY] = null;
			}
		}			
	}
	
	void TurnOffColliders(GameObject myObj)
	{
		if (myObj.GetComponent<CapsuleCollider>() != null)
		{
			myObj.GetComponent<CapsuleCollider>().enabled = false;
		}
		if (myObj.GetComponent<SphereCollider>() != null)
		{
			myObj.GetComponent<SphereCollider>().enabled = false;
		}
		if (myObj.GetComponent<BoxCollider>() != null)
		{
			myObj.GetComponent<BoxCollider>().enabled = false;
		}
		if (myObj.GetComponent<MeshCollider>() != null)
		{
			myObj.GetComponent<MeshCollider>().enabled = false;
		}
		if (myObj.GetComponent<Pickup>() != null)
		{
			myObj.GetComponent<Pickup>().enabled = false;
		}		
	}

	void TurnOnColliders(GameObject myObj)
	{
		if (myObj.GetComponent<CapsuleCollider>() != null)
		{
			myObj.GetComponent<CapsuleCollider>().enabled = true;
		}
		if (myObj.GetComponent<SphereCollider>() != null)
		{
			myObj.GetComponent<SphereCollider>().enabled = true;
		}
		if (myObj.GetComponent<BoxCollider>() != null)
		{
			myObj.GetComponent<BoxCollider>().enabled = true;
		}
		if (myObj.GetComponent<MeshCollider>() != null)
		{
			myObj.GetComponent<MeshCollider>().enabled = true;
		}
		if (myObj.GetComponent<Pickup>() != null)
		{
			myObj.GetComponent<Pickup>().enabled = true;
		}		
	}

	void UpdateCursor()
	{
// position cursor if selected object is not null
		if(SelectedGameObject != null)
		{
			Cursor.transform.position = SelectedGameObject.transform.position;
// scale cursor to selected object size
			var SelectedGameObjectSize = PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName((SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7))).TileSize;
			Cursor.transform.localScale = new Vector3(SelectedGameObjectSize.x, 1, SelectedGameObjectSize.y);
		}
// change cursor material if the edit mode changes
		
	}
	
	void Update () 
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit = new RaycastHit();

// zoom controls for touchscreen
		if ( Input.touchCount > 1)
		{			
			if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
			{
			 
				curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
				prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
				touchDelta = prevDist.magnitude - curDist.magnitude;
				speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
				speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
		
				fuck = (curDist.ToString() + "  " + prevDist.ToString() + "  " + touchDelta.ToString() + "  " + speedTouch0.ToString() + "  " + speedTouch1.ToString());
		
				Camera.main.fieldOfView += Mathf.Clamp(touchDelta*zoomSpeed, -1.0f, 1.0f);
		
			} 
		}
		else
		{
			if ( Input.GetMouseButtonDown(0))
			{
				curTouchPosition = Input.mousePosition;
				prevTouchPosition = Input.mousePosition;
				
				SelectTimer = SelectObjectTimer;
			    if (Physics.Raycast(ray, out hit, 100))
				{
					// don't move the raycast ground plane
					if(hit.collider.gameObject.name != "EditorPlane" && hit.collider.gameObject != null)
					{
						SelectedGameObject = hit.collider.transform.gameObject;
						
						if (SelectedGameObject != null)
						{
							var BuildableType = PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName((SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7))).Type;
							if(EditMode == "BuildableLayer")
							{
								if (BuildableType == "TerrainAdd")
								{
									if (SelectedGameObject != null)
									{
//										Destroy(SelectedGameObject.gameObject);
									}
									SelectedGameObject = null;
								}
								else
								{
									if(DeleteNextObject == true)
									{
										ClearMapLayerObject(SelectedGameObject.transform.position);
										Destroy(SelectedGameObject);
										DeleteNextObject = false;
										PlayerMapObject.GetComponent<PlayerMap>().SaveMap();
									}
									else
									{
										if (TranslationMode == "Move")
										{
											ClearMapLayerObject(SelectedGameObject.transform.position);
											Cursor.transform.position = SelectedGameObject.transform.position;
											TurnOffColliders(SelectedGameObject);
										}
										if (TranslationMode == "Rotate")
										{
											var newRotation = SelectedGameObject.transform.rotation;
											newRotation.eulerAngles = new Vector3( newRotation.eulerAngles.x, Mathf.Round(newRotation.eulerAngles.y + 90),  newRotation.eulerAngles.z);
											if (newRotation.eulerAngles.y >= 360)
											{
												newRotation.eulerAngles = new Vector3( newRotation.eulerAngles.x, 0,  newRotation.eulerAngles.z);
											}
											SelectedGameObject.transform.rotation = newRotation;
											
											var tempstring = SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7) ;
											var placedBuildable = PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName(tempstring);
											TurnOffColliders(SelectedGameObject);
											SetMapLayerObject(SelectedGameObject.transform.position, SelectedGameObject.transform.rotation, placedBuildable);
											TurnOnColliders(SelectedGameObject);
											if (SelectedGameObject != null)
											{
//												Destroy(SelectedGameObject.gameObject);
											}											
											SelectedGameObject = null;
											PlayerMapObject.GetComponent<PlayerMap>().SaveMap();
										}
									}
								}
							}
							if(EditMode == "TerrainLayer")
							{
								if (BuildableType == "TerrainAdd" && SelectedGameObject.name != "Terrain_Flat_Prefab(Clone)")
								{
									if(DeleteNextObject == true)
									{
										var tempstring = SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7) ;
										ClearTerrainLayerObject(SelectedGameObject.transform.position, SelectedGameObject.transform.rotation, PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName(tempstring));
										Destroy(SelectedGameObject.gameObject);
										DeleteNextObject = false;									
										PlayerMapObject.GetComponent<PlayerMap>().SaveMap();
									}
									else
									{
										if (TranslationMode == "Move")
										{
											var tempstring = SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7) ;
											ClearTerrainLayerObject(SelectedGameObject.transform.position,  SelectedGameObject.transform.rotation, PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName(tempstring));
											Cursor.transform.position = SelectedGameObject.transform.position;
											Cursor.GetComponent<MeshRenderer>().enabled = true;
										
										}
										if (TranslationMode == "Rotate")
										{
											var tempstring = SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7) ;
											var placedBuildable = PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName(tempstring);

											var newRotation = SelectedGameObject.transform.rotation;
											newRotation.eulerAngles = new Vector3( newRotation.eulerAngles.x, Mathf.Round(newRotation.eulerAngles.y + 90),  newRotation.eulerAngles.z);
											if (newRotation.eulerAngles.y >= 360)
											{
												newRotation.eulerAngles = new Vector3( newRotation.eulerAngles.x, 0,  newRotation.eulerAngles.z);
											}
											SelectedGameObject.transform.rotation = newRotation;
											
											TurnOffColliders(SelectedGameObject);
//											SetTerrainLayerObject(SelectedGameObject.transform.position, SelectedGameObject.transform.rotation, placedBuildable);
											TurnOnColliders(SelectedGameObject);
											if (SelectedGameObject != null)
											{
//												Destroy(SelectedGameObject.gameObject);
											}																						
											SelectedGameObject = null;
											PlayerMapObject.GetComponent<PlayerMap>().SaveMap();
										}										
										
									}
								}
								else
								{
									if (SelectedGameObject != null)
									{
//										Destroy(SelectedGameObject.gameObject);
									}																				
									SelectedGameObject = null;
								}
							}
						}
					}
		    	}
		    }
			
			if (Input.GetMouseButtonUp(0) && SelectedGameObject != null)
			{
				var tempstring = SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7) ;
				var placedBuildable = PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName(tempstring);
				if(EditMode == "BuildableLayer")
				{
					SetMapLayerObject(SelectedGameObject.transform.position, SelectedGameObject.transform.rotation, placedBuildable);
					TurnOnColliders(SelectedGameObject);
				}
				if(EditMode == "TerrainLayer")
				{
					SetTerrainLayerObject(SelectedGameObject.transform.position, SelectedGameObject.transform.rotation, placedBuildable);
				}			
				SelectTimer = SelectObjectTimer;
				SelectObject = false;
				SelectedGameObject = null;	
				Cursor.GetComponent<MeshRenderer>().enabled = false;
			}
			
			if (Input.GetMouseButton(0) )
			{
				curTouchPosition = Input.mousePosition;
				var posDelta = prevTouchPosition - curTouchPosition;
	
				SelectTimer -= Time.deltaTime;
				if (SelectTimer <= 0)
				{
					SelectTimer = 0;
					SelectObject = true;
				}
	
				fuck = posDelta.ToString();
				
				if (prevTouchPosition.x == 0 && prevTouchPosition.y == 0)
				{
					prevTouchPosition = Input.mousePosition;
				}
				
				// select object if the mouse button is held down			
				if (SelectObject == true && SelectedGameObject != null)
				{
					var tempstring = SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7) ;
					var placedBuildable = PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName(tempstring);

// Create a copy of the selected object
					var newObjectName = (string)(SelectedGameObject.name.Substring( 0,SelectedGameObject.name.Length - 7)).Clone();
					var newObjectAssetPath = (string) PlayerMapObject.GetComponent<PlayerMap>().ListOfBuildables.FindByName(newObjectName).AssetPath.Clone();
					var newObjectPos = (Vector3) SelectedGameObject.transform.position;
					var newObjectRot = (Quaternion) SelectedGameObject.transform.rotation;
					if (newObjectAssetPath != null)
					{
						Destroy(SelectedGameObject.gameObject);
						SelectedGameObject = (GameObject) Instantiate( Resources.Load(newObjectAssetPath,  typeof(GameObject)), newObjectPos, newObjectRot );
						PlayerMapObject.GetComponent<PlayerMap>().DisableComponentsForEditor();
					}
					else
					{
						SelectedGameObject = null;
					}

					int mask = ~(1 << 0); // ignore default layer
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					hit = new RaycastHit();
				    if (Physics.Raycast(ray, out hit, 100,mask))
					{
						var newPosition = RoundPostion(hit.point, placedBuildable.TileSize); // need to snap the hit point position
//						newPosition = MapTileToPosition( PositionToMapTile(newPosition, placedBuildable.TileSize), placedBuildable.TileSize);
						{
							if(placedBuildable.Type == "TerrainAdd")
							{
								if(TerrainLayerAvailable(newPosition, SelectedGameObject.transform.rotation, placedBuildable.TileSize ) )
//								if(true)
								{
									SelectedGameObject.transform.position = newPosition;
									Cursor.transform.position = newPosition;
									Cursor.GetComponent<MeshRenderer>().enabled = true;
								}
							}
							else
							{
								if(BuildableLayerAvailable(newPosition) )
								{
									TurnOffColliders(SelectedGameObject);
									newPosition = FindTerrainCollisionPointYAxis(newPosition);
//									TurnOnColliders(SelectedGameObject);
									SelectedGameObject.transform.position = newPosition;
									Cursor.transform.position = newPosition;
									Cursor.GetComponent<MeshRenderer>().enabled = true;
								}
							}
						}
			    	}
				}
				// move camera instead			
				else
				{
					// multipy the delta times the cameras fov to compensate for the camera zoom
					posDelta = posDelta * (Camera.main.fieldOfView / 15);
					selectedCamera.transform.position = new Vector3(selectedCamera.transform.position.x + (posDelta.x * movespeed),  selectedCamera.transform.position.y, selectedCamera.transform.position.z + (posDelta.y * movespeed));
				}
				
				prevTouchPosition = curTouchPosition ;
	
			}
			
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			if (scroll != 0.0f)
			{
				Camera.main.fieldOfView -= scroll*zoomSpeed;
			}
		}
	

// clamp camera position
		if(Camera.main.transform.position.x > 45.0f)
		{
			selectedCamera.transform.position = new Vector3(45.0f,  selectedCamera.transform.position.y, selectedCamera.transform.position.z);
		}
		if(Camera.main.transform.position.x < 0.0f)
		{
			selectedCamera.transform.position = new Vector3(0.0f,  selectedCamera.transform.position.y, selectedCamera.transform.position.z);
		}
		if(Camera.main.transform.position.z > 15.0f)
		{
			selectedCamera.transform.position = new Vector3(selectedCamera.transform.position.x,  selectedCamera.transform.position.y, 15.0f);
		}
		if(Camera.main.transform.position.z < -20.0f)
		{
			selectedCamera.transform.position = new Vector3(selectedCamera.transform.position.x,  selectedCamera.transform.position.y, -20.0f);
		}
		
// clamp camera zoom		
		if(Camera.main.fieldOfView > 60)
		{
			Camera.main.fieldOfView = 60;
		}
		if(Camera.main.fieldOfView < 15)
		{
			Camera.main.fieldOfView = 15;
		}	

// update the cursor 		
		UpdateCursor();
		
	}
}
