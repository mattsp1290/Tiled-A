using UnityEngine;
using System.Collections;
//using TiledSharp;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {

	//private string mapLocation = "test1.tmx";
	private int sizeX;
	private int sizeY;
	public float tileSize = 1.0f;
	
	public Texture2D terrainTiles;
	public int tileResolution;
	GameObject gameStateObject;
	GameState state;
	
	void Start() {
		gameStateObject = GameObject.Find("GameStateObject");
		state = gameStateObject.GetComponent<GameState>();
	}
	
	Color[][] ChopUpTiles(int height) {
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;
		
		Color[][] tiles = new Color[numTilesPerRow*numRows][];
		
		for(int y=0; y<numRows; y++) {
			for(int x=0; x<numTilesPerRow; x++) {
				tiles[y*numTilesPerRow + x] = terrainTiles.GetPixels( x*tileResolution , ((numRows * tileResolution) - (y + 1)*tileResolution), tileResolution, tileResolution );
			}
		}

		return tiles;
	}
	
	void BuildTexture(int[,] grid) {
		//var map = new TmxMap(mapLocation);
		// DTileMap map = new DTileMap(sizeX, sizeY);
		tileResolution = 32;
		//tileSize = map.TileWidth;
		int texWidth = sizeX * tileResolution;
		int texHeight = sizeY * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);
		
		Color[][] tiles = ChopUpTiles(texHeight);

		for (int y = 0; y < sizeX; y++) {
			for (int x = 0; x < sizeX; x++) {
				// 265
				int gid = 53;
				if (grid[x, y] == 1) {
					gid = 48;
				}
				Color[] p = tiles[gid];
				texture.SetPixels(x*tileResolution, (texHeight - (y + 1)*tileResolution), tileResolution, tileResolution, p);
			}
		}
		/*
		foreach (int[] rowTiles in grid) {
			foreach (int tile in rowTiles) {

				int gid = tile.Gid;
				//Debug.Log ("ID in Map " + gid);
				Color[] p = tiles[gid - 1];
				texture.SetPixels(tile.X*tileResolution, (texHeight - (tile.Y + 1)*tileResolution), tileResolution, tileResolution, p);
			}
		}
		*/
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		
		Debug.Log ("Done Texture!");
	}
	
	public void BuildMesh(int[,] grid) {
		sizeX = state.sizeX;
		sizeY = state.sizeY;
		int numTiles = sizeX * sizeY;
		int numTris = numTiles * 2;
		
		int vsizeX = sizeX + 1;
		int vsizeY = sizeY + 1;
		int numVerts = vsizeX * vsizeY;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];

		int x, y;
		for(y=0; y < vsizeY; y++) {
			for(x=0; x < vsizeX; x++) {
				vertices[ y * vsizeX + x ] = new Vector3( x*tileSize, -y*tileSize,  0);
				normals[ y * vsizeX + x ] = Vector3.up;
				uv[ y * vsizeX + x ] = new Vector2( (float)x / sizeX, 1f - (float)y / sizeY );
			}
		}
		Debug.Log ("Done Verts!");
		
		for(y=0; y < sizeY; y++) {
			for(x=0; x < sizeX; x++) {
				int squareIndex = y * sizeX + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = y * vsizeX + x + 		   0;
				triangles[triOffset + 2] = y * vsizeX + x + vsizeX + 0;
				triangles[triOffset + 1] = y * vsizeX + x + vsizeX + 1;
				
				triangles[triOffset + 3] = y * vsizeX + x + 		   0;
				triangles[triOffset + 5] = y * vsizeX + x + vsizeX + 1;
				triangles[triOffset + 4] = y * vsizeX + x + 		   1;
			}
		}
		
		Debug.Log ("Done Triangles!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");
		
		BuildTexture(grid);
	}
	
	
}
