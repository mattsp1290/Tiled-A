using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameState : MonoBehaviour {

	public int sizeX = 6;
	public int sizeY = 6;
	private float nextActionTime = 0.0f;
	public float period = 3.0f;
	private int[,] grid;
	GameObject timeMapObject;
	TileMap tileMap;
	public Boolean running;

	// Use this for initialization
	void Start () {
		running = false;
		grid = new int[sizeX, sizeY];
		timeMapObject = GameObject.Find("TileMap");
		tileMap = timeMapObject.GetComponent<TileMap>();
		RandomizeGrid();
	}
	
	// Update is called once per frame
	void Update () {
		if (running && Time.time > nextActionTime ) {
			nextActionTime = Time.time + period; 
			RunConwayStep();
		}
	}

	public void UpdateGrid(int x, int y, int state) {
		grid[x, y] = state;
		tileMap.BuildMesh(grid);
	}

	public int GetPointInGrid(int x, int y) {
		return grid[x, y];
	}

	public int[] GetGridSize() {
		return new int[2]{sizeX, sizeY};
	}

	public void ActOnGrid(Action<int, int> act){
		for (int y = 0; y < sizeY; y++) {
			for (int x = 0; x < sizeX; x++) {
				act(x, y);
			}
		}
		tileMap.BuildMesh(grid);
	}

	public void RandomizeGrid() {
		Action<int, int> setGridTile = (x, y) => { grid[x, y] = (int)UnityEngine.Random.Range(0.0F, 1.9F); };
		ActOnGrid(setGridTile);
	}

	public void StartGameOfLife() {
		Debug.Log ("Starting Game of Life!");
		running = true;
	}

	public void RunConwayStep(){
		Action<int, int> setGridTile = (x, y) => {
			List<GridTile> neighbors = GetNeighbors(x, y);
			int livingNeighbors = 0;
			int deadNeighbors = 0;
			foreach(GridTile neighbor in neighbors) {
				if (neighbor.value == 1) {
					livingNeighbors += 1;
				} else {
					deadNeighbors += 1;
				}
			}
			int result = grid[x, y];
			if (grid[x, y] == 1 && livingNeighbors == 2) {
				result = 1;
			} else if (grid[x, y] == 1) {
				result = 0;
			}
			if (livingNeighbors == 3) {
				result = 1;
			}
			if (x == 5 && y == 0) {
				Debug.Log ("what");
			}
			grid[x, y] = result; 
		};
		ActOnGrid(setGridTile);
	}

	public List<GridTile> GetNeighbors(int x, int y){
		List<GridTile> neighbors = new List<GridTile>();
		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				int newX = i + x;
				int newY = j + y;
				// make sure you do not count the origin tile
				if (!(x == newX && y == newY)){
					// check if outside x bounds
					if (newX > -1 && newX < sizeX) {
						// check if outside y bounds
						if (newY > -1 && newY < sizeY) {
							GridTile tile = new GridTile();
							tile.x = newX;
							tile.y = newY;
							tile.value = grid[tile.x, tile.y];
							neighbors.Add(tile);
						}
					}
				}
			}
		}
		return neighbors;
	}

	public class GridTile {
		public int x;
		public int y;
		public int value;
	}
}
