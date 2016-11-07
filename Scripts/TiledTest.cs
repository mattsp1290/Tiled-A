using UnityEngine;
using System.Collections;
using TiledSharp;

// Primary issue has to do with Unity importing the spritesheet without empty cells

public class TiledTest : MonoBehaviour {
	// Use this for initialization
	public GameObject tilePrefab;

	void Start () {
		var map = new TmxMap("test1.tmx");
		Debug.Log ("Height");
		Debug.Log (map.Height);
		Debug.Log ("Width");
		Debug.Log (map.Width);
		Debug.Log ("Tile Height");
		Debug.Log (map.TileHeight);
		Debug.Log ("Tile Width");
		Debug.Log (map.TileWidth);
		// Debug.Log (map.Layers[0].Tiles[3].Gid);
		//Tile tile1 = GameObject.Find ("Tile").GetComponent <Tile> ();
		//tile1.currentTile = map.Layers [0].Tiles [0].Gid - 1;
		foreach (TmxLayerTile tile in map.Layers[0].Tiles){
			Vector3 tilePosition = new Vector3(tile.X, (-1 * tile.Y), 0);
			GameObject tileObject = Instantiate(tilePrefab, tilePosition, transform.rotation) as GameObject;
			Tile tileScript = tileObject.GetComponent <Tile> ();
			tileScript.currentTile = tile.Gid - 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
