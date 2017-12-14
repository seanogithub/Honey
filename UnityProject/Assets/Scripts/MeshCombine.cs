using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]
public class MeshCombine : MonoBehaviour {
	
	public Mesh NewCollisionMesh;
	private GameObject[] TerrainAddObjects;    // Reference to the player GameObject.
	
	void Awake()
	{
/*
		TerrainAddObjects = GameObject.FindGameObjectsWithTag("TerrainAdd");
		print (TerrainAddObjects.Length);

		for (var i = 0; i < TerrainAddObjects.Length; i++)
		{
			if (i == 0)
			{
				NewMesh = CombineMeshes(GetComponent<MeshCollider>().sharedMesh, TerrainAddObjects[i]);
			}
			else
			{
				NewMesh = CombineMeshes(NewMesh, TerrainAddObjects[i]);
			}
			TerrainAddObjects[i].GetComponent<MeshCollider>().enabled = false;
		}		
		GetComponent<MeshCollider>().sharedMesh = NewMesh;
*/		
	}

//*********************
// the time this takees needs to be linear and not exponential	
//*********************
	
	void Init()
	{
		TerrainAddObjects = GameObject.FindGameObjectsWithTag("TerrainAdd");
//		print (TerrainAddObjects.Length);
		
		NewCollisionMesh = CombineCollisionMeshes(GetComponent<MeshCollider>().sharedMesh, TerrainAddObjects);

/*		
		for (var i = 0; i < TerrainAddObjects.Length; i++)
		{
			DebugMeshesInfo(GetComponent<MeshCollider>().sharedMesh, TerrainAddObjects[i]);
		}
*/		
		
/*		
		for (var i = 0; i < TerrainAddObjects.Length; i++)
		{
			if (i == 0)
			{
				NewMesh = CombineMeshes(GetComponent<MeshCollider>().sharedMesh, TerrainAddObjects[i]);
			}
			else
			{
				NewMesh = CombineMeshes(NewMesh, TerrainAddObjects[i]);
			}
			TerrainAddObjects[i].GetComponent<MeshCollider>().enabled = false;
		}		
*/
		
		gameObject.GetComponent<MeshCollider>().sharedMesh = NewCollisionMesh;
	}
	
	Mesh CombineCollisionMeshes (Mesh meshOne, GameObject[] TerrainAdd)
	{
		var NumNewVertices = meshOne.vertices.Length;
		var NumNewNormals = meshOne.normals.Length;
		var NumNewTris = meshOne.triangles.Length;
		var NewMesh = new Mesh();
		
		// set up the arrays for the new collison mesh
		for (var i = 0; i < TerrainAdd.Length; i++)
		{
			NumNewVertices += TerrainAdd[i].GetComponent<MeshCollider>().sharedMesh.vertices.Length;
			NumNewNormals += TerrainAdd[i].GetComponent<MeshCollider>().sharedMesh.normals.Length;
			NumNewTris += TerrainAdd[i].GetComponent<MeshCollider>().sharedMesh.triangles.Length;
		}
		
		var NewVertices = new Vector3[NumNewVertices];	
		var NewNormals = new Vector3[NumNewNormals];	
		var NewTris = new int[NumNewTris];	
		
//		print (NumNewVertices);
//		print (NumNewNormals);
//		print (NumNewTris);

		// add meshOne to NewCollisionMesh
		for (var i = 0; i < meshOne.vertices.Length; i++)
		{
			NewVertices[i] = meshOne.vertices[i];
			NewNormals[i] = meshOne.normals[i];
		}
		for (var i = 0; i < meshOne.triangles.Length; i++)
		{
			NewTris[i] = meshOne.triangles[i];
		}
		
		// add rest of objects to NewCollisionMesh
		var NewVertIndexCount = meshOne.vertices.Length;
		var NewTriIndexCount = meshOne.triangles.Length;

		
		for (var numObj = 0; numObj < TerrainAdd.Length; numObj++)
		{
			for (var i = 0; i < TerrainAdd[numObj].GetComponent<MeshCollider>().sharedMesh.vertices.Length; i++)
			{
				// add parents transforms to each of the verts
				Vector3 newVertPos = TerrainAdd[numObj].GetComponent<MeshCollider>().sharedMesh.vertices[i];
				//scale
				newVertPos = Vector3.Scale(TerrainAdd[numObj].transform.localScale, newVertPos);
					
				//rotation
				newVertPos = Quaternion.Euler(TerrainAdd[numObj].transform.localEulerAngles.x, TerrainAdd[numObj].transform.localEulerAngles.y, TerrainAdd[numObj].transform.localEulerAngles.z) * newVertPos;
	
				//position
				newVertPos = (newVertPos + TerrainAdd[numObj].transform.position);
				
//				newVertices[i + meshOne.vertices.Length] = newVertPos;
				
				NewVertices[NewVertIndexCount] = newVertPos;
//				NewVertices[NewVertIndexCount] = TerrainAdd[numObj].GetComponent<MeshCollider>().sharedMesh.vertices[i];

				NewNormals[NewVertIndexCount] = TerrainAdd[numObj].GetComponent<MeshCollider>().sharedMesh.normals[i];
				NewVertIndexCount +=1;
			}	
			for (var i = 0; i < TerrainAdd[numObj].GetComponent<MeshCollider>().sharedMesh.triangles.Length; i++)
			{
				// need to add previous tri indexes to the current tri index
				NewTris[NewTriIndexCount] = TerrainAdd[numObj].GetComponent<MeshCollider>().sharedMesh.triangles[i] + NewVertIndexCount - TerrainAdd[numObj].GetComponent<MeshCollider>().sharedMesh.vertices.Length;
//				print (NewTris[NewTriIndexCount]);

				NewTriIndexCount +=1;
			}		
			
			TerrainAdd[numObj].GetComponent<MeshCollider>().enabled = false;
		}		
		
		// add vert, normals, tris to new mesh
		NewMesh.Clear();
		NewMesh.vertices = NewVertices;
		NewMesh.normals = NewNormals;
		NewMesh.triangles = NewTris;
			
		return(NewMesh);
	}
	
	Mesh CombineMeshes (Mesh meshOne, GameObject TerrainAdd)
	{
		if (TerrainAdd.GetComponent<MeshCollider>() == null) 
		{
			print ("ERROR: NO MESH COLLIDER ON TERRAINADD OBJECT");
		}
		Mesh meshTwo = TerrainAdd.GetComponent<MeshCollider>().sharedMesh;
		print(TerrainAdd.transform.position);	
		
//****************
// In order to combine meshes the vert, uv, and normals arrays must be the same length
// need to error check for this !		
//****************
		
		var newVertices = new Vector3[(meshOne.vertices.Length + meshTwo.vertices.Length)];	
		var newUVs = new Vector2[(meshOne.uv.Length + meshTwo.uv.Length)];	
		var newUV1s = new Vector2[(meshOne.uv2.Length + meshTwo.uv2.Length)];	
		var newUV2s = new Vector2[(meshOne.uv2.Length + meshTwo.uv2.Length)];	
		var newNormals = new Vector3[(meshOne.normals.Length + meshTwo.normals.Length)];	
		var newTriangles = new int[(meshOne.triangles.Length + meshTwo.triangles.Length)];
		var newMesh = new Mesh();
		
//		print ("verts " + newVertices.Length);
//		print ("UVs " +newUVs.Length);
//		print ("UV1s " +newUV1s.Length);
//		print ("UV2s " +newUV2s.Length);
//		print ("normals " +newNormals.Length);
//		print ("tris " +newTriangles.Length);

		
// verts		
		for (var i = 0; i < meshOne.vertices.Length; i++)
		{
			newVertices[i] = meshOne.vertices[i];
		}
		for (var i = 0; i < meshTwo.vertices.Length; i++)
		{
			// add parents transforms to each of the verts
			Vector3 newVertPos = meshTwo.vertices[i];
			//scale
			newVertPos = Vector3.Scale(TerrainAdd.transform.localScale, newVertPos);
				
			//rotation
			newVertPos = Quaternion.Euler(TerrainAdd.transform.localEulerAngles.x, TerrainAdd.transform.localEulerAngles.y, TerrainAdd.transform.localEulerAngles.z) * newVertPos;

			//position
			newVertPos = (newVertPos + TerrainAdd.transform.position);
			
			newVertices[i + meshOne.vertices.Length] = newVertPos;
		}

// uv		
		for (var i = 0; i < meshOne.uv.Length; i++)
		{
			newUVs[i] = meshOne.uv[i];
		}
		for (var i = 0; i < meshTwo.uv.Length; i++)
		{
			newUVs[i + meshOne.uv.Length] = meshTwo.uv[i];
		}
		
// uv1		
		for (var i = 0; i < meshOne.uv2.Length; i++)
		{
			newUV1s[i] = meshOne.uv2[i];
		}
		for (var i = 0; i < meshTwo.uv2.Length; i++)
		{
			newUV1s[i + meshOne.uv2.Length] = meshTwo.uv2[i];
		}

// uv2		
		for (var i = 0; i < meshOne.uv2.Length; i++)
		{
			newUV2s[i] = meshOne.uv2[i];
		}
		for (var i = 0; i < meshTwo.uv2.Length; i++)
		{
			newUV2s[i + meshOne.uv2.Length] = meshTwo.uv2[i];
		}

// Normals		
		for (var i = 0; i < meshOne.normals.Length; i++)
		{
			newNormals[i] = meshOne.normals[i];
		}
		for (var i = 0; i < meshTwo.normals.Length; i++)
		{
			newNormals[i + meshOne.normals.Length] = meshTwo.normals[i];
		}	
		
// triangles		
		for (var i = 0; i < meshOne.triangles.Length; i++)
		{
			newTriangles[i] = meshOne.triangles[i];
		}
		for (var i = 0; i < meshTwo.triangles.Length; i++)
		{
			newTriangles[i + meshOne.triangles.Length] = (meshTwo.triangles[i] + meshOne.vertices.Length );
		}
		
		newMesh.Clear();
		
		newMesh.vertices = newVertices;
//		newMesh.uv = newUVs;
//		newMesh.uv1 = newUV1s;
//		newMesh.uv2 = newUV2s;
		newMesh.triangles = newTriangles;
		newMesh.normals = newNormals;
		
		return(newMesh);
		
	}

	
	void DebugMeshesInfo (Mesh meshOne, GameObject TerrainAdd)
	{
		if (TerrainAdd.GetComponent<MeshCollider>() == null) 
		{
			print ("ERROR: NO MESH COLLIDER ON TERRAINADD OBJECT");
		}
		Mesh meshTwo = TerrainAdd.GetComponent<MeshCollider>().sharedMesh;
		print(TerrainAdd.transform.position);	
		
//****************
// In order to combine meshes the vert, uv, and normals arrays must be the same length
// need to error check for this !		
//****************
		
		var newVertices = new Vector3[(meshOne.vertices.Length + meshTwo.vertices.Length)];	
		var newUVs = new Vector2[(meshOne.uv.Length + meshTwo.uv.Length)];	
		var newUV1s = new Vector2[(meshOne.uv2.Length + meshTwo.uv2.Length)];	
		var newUV2s = new Vector2[(meshOne.uv2.Length + meshTwo.uv2.Length)];	
		var newNormals = new Vector3[(meshOne.normals.Length + meshTwo.normals.Length)];	
		var newTriangles = new int[(meshOne.triangles.Length + meshTwo.triangles.Length)];
		var newMesh = new Mesh();
		
		print ("verts " + newVertices.Length);
		print ("UVs " +newUVs.Length);
		print ("UV1s " +newUV1s.Length);
		print ("UV2s " +newUV2s.Length);
		print ("normals " +newNormals.Length);
		print ("tris " +newTriangles.Length);
		
	}
/*	
	// Update is called once per frame
    void Update() 
    {
    }
*/
}
