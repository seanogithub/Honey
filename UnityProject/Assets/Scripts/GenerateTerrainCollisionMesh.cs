using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]
public class GenerateTerrainCollisionMesh : MonoBehaviour {

	public Mesh NewMesh;
	private GameObject[] TerrainAddObjects;    // Reference to the player GameObject.
	
	void Awake()
	{
		TerrainAddObjects = GameObject.FindGameObjectsWithTag("TerrainAdd");
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
		
	}
	
/*	
	// Use this for initialization
	void Start () 
	{
		Mesh oldMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = oldMesh.vertices;
        Vector2[] UVs = oldMesh.uv;
        Vector2[] UV2s = oldMesh.uv2;
        int[] Triangles = oldMesh.triangles;
        Vector3[] normals = oldMesh.normals;

		var addedMesh = new Mesh();		
        addedMesh.vertices = new Vector3[] {new Vector3(0, -2, 2), new Vector3(0, 5, 2), new Vector3(5, 5, 2)};
        addedMesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        addedMesh.uv1 = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        addedMesh.uv2 = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        addedMesh.normals = new Vector3[] {new Vector3(0, -2, 2), new Vector3(0, 5, 2), new Vector3(5, 5, 2)};
        addedMesh.triangles = new int[] {2, 1, 0};

		shit = CombineMeshes(oldMesh, addedMesh);
		
//      gameObject.AddComponent("MeshFilter");
//      gameObject.AddComponent("MeshRenderer");

		Mesh mesh = GetComponent<MeshFilter>().mesh;
 		mesh.Clear();
        mesh.vertices = new Vector3[] {new Vector3(0, -2, 2), new Vector3(0, 5, 2), new Vector3(5, 5, 2)};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = new int[] {2, 1, 0};
//		GetComponent<MeshCollider>().sharedMesh = mesh;

		GetComponent<MeshCollider>().sharedMesh = shit;
		GetComponent<MeshFilter>().mesh = shit;
		

	}
*/
	
	Mesh CombineMeshes (Mesh meshOne, GameObject TerrainAdd)
	{
		if (TerrainAdd.GetComponent<MeshCollider>() == null) 
		{
			print ("ERROR: NO MESH COLLIDER ON TERRAINADD OBJECT");
		}
		Mesh meshTwo = TerrainAdd.GetComponent<MeshCollider>().sharedMesh;
		
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
/*		
		print ("verts " + newVertices.Length);
		print ("UVs " +newUVs.Length);
		print ("UV1s " +newUV1s.Length);
		print ("UV2s " +newUV2s.Length);
		print ("normals " +newNormals.Length);
		print ("tris " +newTriangles.Length);
*/
		
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
	

}
