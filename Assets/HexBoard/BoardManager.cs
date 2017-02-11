using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour, IVertexHighlighter {

	public GameObject tileObject;
	public GameObject townObject;
	public GameObject roadObject;
	public bool pointyTopped;

	private Transform boardObject;
	private Dictionary<Hex, Tile> allTiles;
	private Dictionary<Vertex, Town> allTowns;
	private Dictionary<Edge, Road> allRoads;

	private List<int> tileNumbersList;
	private List<TileType> tileTypesList;
	private const int mapRadius = 3;

	//*******************************************************************************************
	// Unity methods
	//*******************************************************************************************
	private void OnEnable() {
		// Set the orientation of the board
		Orientation orientation;

		if (pointyTopped)
			orientation = new Orientation(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
																		Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
																		0.5f);
		else
			orientation = new Orientation(3.0f / 2.0f, 0.0f, Mathf.Sqrt(3.0f) / 2.0f, Mathf.Sqrt(3.0f),
																		2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Mathf.Sqrt(3.0f) / 3.0f,
																		0.0f);

		float size = 0.5f;
		Vector2 origin = new Vector2(0f, 0f);

		Layout hexLayout = new Layout(orientation, size, origin);

		Hex.SetHexLayout(hexLayout);

		allTiles = new Dictionary<Hex, Tile>();
		allTowns = new Dictionary<Vertex, Town>();
		allRoads = new Dictionary<Edge, Road>();

		boardObject = new GameObject("Board").transform;
	}

	private void Start() {
		InitBoard();
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void InitBoard() {
	// TODO Ensure that this works for both pointy topped and flat topped
	// hexes. Currently towns/roads are not created in the correct places.
	// TODO Create nice functions for checking if a tile should have a 
	// town/road and use them here.
		tileNumbersList = CreateTileNumbers();
		tileTypesList = CreateTileTypes();

		for (int q = -mapRadius; q <= mapRadius; q++) {
			int r1 = Mathf.Max(-mapRadius, -q - mapRadius);
			int r2 = Mathf.Min(mapRadius, -q + mapRadius);

			for (int r = r1; r <= r2; r++) {
				CreateTile(new Hex(q, r));

				// Create top towns
				if (TileHasTown(new Hex(q, r), true)) {
					CreateTown(new Vertex(q, r, true));
				}
				// Create bottom towns
				if (TileHasTown(new Hex(q, r), false)) {
					CreateTown(new Vertex(q, r, false));
				}

				// Create Northern roads
				if (TileHasRoad(new Hex(q, r), 1)) {
					CreateRoad(new Edge(q, r, +1));
				}
				// Create Eastern roads
				if (TileHasRoad(new Hex(q, r), 0)) {
					CreateRoad(new Edge(q, r, 0));
				}
				// Create Southern roads
				if (TileHasRoad(new Hex(q, r), -1)) {
					CreateRoad(new Edge(q, r, -1));
				}
			}
		}
	}

	//*******************************************************************************************
	// IVertexHighlighter methods
	//*******************************************************************************************
	public void HighlightVertex(Vertex toHighlight, bool highlight) {
		allTowns[toHighlight].HighlightTown(highlight);
	}

	public bool VertexOutOfBounds(Vertex toCheck) {
		return !TileHasTown(toCheck.hex, toCheck.top);
  }

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************
	// Functions for creating the different types of board pieces
	// TODO Combine these into one function somehow, would probably require refactoring some
	// of the piece/position code so that the methods have similar names.
	private void CreateTile(Hex hex) {
		GameObject toInstantiate = tileObject;
		Vector2 instantiatePosition = hex.HexToScreen();
		Quaternion rotation = Quaternion.identity;
		GameObject newTileObj;
		Tile newTile;

		newTileObj = Instantiate(toInstantiate, instantiatePosition, rotation) as GameObject;
		newTileObj.transform.parent = boardObject;
		newTile = newTileObj.GetComponent<TileMono>().GetTile();

		allTiles.Add(hex, newTile);
		SetUpTile(newTile, hex);

    newTile.DisplayDebug();
  }

	private void CreateTown(Vertex vertex) {
		GameObject toInstantiate = townObject;
		Vector2 instantiatePosition = vertex.VertexToScreen();
		Quaternion rotation = Quaternion.identity;
		GameObject newTownObj;
		Town newTown;

		newTownObj = Instantiate(toInstantiate, instantiatePosition, rotation) as GameObject;
		newTownObj.transform.parent = boardObject;
		newTown = newTownObj.GetComponent<TownMono>().GetTown();

		allTowns.Add(vertex, newTown);
		newTown.SetUp(vertex);
	}

	private void CreateRoad(Edge edge) {
		GameObject toInstantiate = roadObject;
		Vector2 instantiatePosition = edge.EdgeToScreen();
		Quaternion rotation = Quaternion.identity;
		GameObject newRoadObj;
		Road newRoad;

		newRoadObj = Instantiate(toInstantiate, instantiatePosition, rotation) as GameObject;
		newRoadObj.transform.parent = boardObject;
		newRoad = newRoadObj.GetComponent<RoadMono>().GetRoad();

		allRoads.Add(edge, newRoad);
		newRoad.SetUp(edge);
	}

	private void SetUpTile(Tile tileToSetup, Hex tileHex) {
		int indexTileType = UnityEngine.Random.Range(0, tileTypesList.Count);
		int tileNumber;
		TileType tileType;

		if (IsOceanTile(tileHex)) {
			tileType = TileType.OCEAN;
			tileNumber = 0;
    }
		else {
			if (tileTypesList[indexTileType] != TileType.DESERT) {
				tileNumber = tileNumbersList[0];
				tileNumbersList.RemoveAt(0);
			}
			else {
				tileNumber = 7;
			}

			tileType = tileTypesList[indexTileType];
			tileTypesList.RemoveAt(indexTileType);
    }

		tileToSetup.SetUp(tileHex, tileNumber, tileType);

	}

	private bool IsOceanTile(Hex hex) {
		return !(Mathf.Abs(hex.q) < mapRadius && Mathf.Abs(hex.r) < mapRadius && Mathf.Abs(hex.q + hex.r) < mapRadius);
  }

	private bool TileHasTown(Hex hex, bool top) {
		if (top) {
			return ((hex.q < mapRadius + 1) &&
							(hex.q > - mapRadius) &&
							(hex.q + hex.r < mapRadius) &&
							(hex.q + hex.r > - mapRadius - 1) &&
							(hex.r < mapRadius) &&
							(hex.r > - mapRadius - 1));
		}
		else {
			return ((hex.q < mapRadius) &&
							(hex.q > - mapRadius - 1) &&
							(hex.q + hex.r < mapRadius + 1) &&
							(hex.q + hex.r > - mapRadius) &&
							(hex.r < mapRadius + 1) &&
							(hex.r > - mapRadius));
		}
	}

	private bool TileHasRoad(Hex hex, int direction) {
		if (direction == 1) {
			return ((hex.q < mapRadius) &&
							(hex.q > -mapRadius) &&
							(hex.r < mapRadius) &&
							(hex.r > -mapRadius - 1) &&
							(hex.r + hex.q < mapRadius) &&
							(hex.r + hex.q > -mapRadius - 1));
		}
		else if (direction == 0) {
			return ((hex.q < mapRadius) &&
							(hex.q > - mapRadius - 1) &&
							(hex.r < mapRadius) &&
							(hex.r > -mapRadius) &&
							(hex.r + hex.q < mapRadius) &&
							(hex.r + hex.q > -mapRadius - 1));
		}
		else {
			return ((hex.q < mapRadius) &&
							(hex.q > -mapRadius - 1) &&
							(hex.r < mapRadius + 1) &&
							(hex.r > -mapRadius) &&
							(hex.r + hex.q < mapRadius) &&
							(hex.r + hex.q > -mapRadius));
		}
	}

	private List<int> CreateTileNumbers() {
		return new List<int>() {
			5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11
		};
	}

	private List<TileType> CreateTileTypes() {
		return new List<TileType>() {
			TileType.DESERT, TileType.WOOD, TileType.WOOD, TileType.WOOD, TileType.WOOD,
			TileType.FARM, TileType.FARM, TileType.FARM, TileType.FARM, TileType.HILL,
			TileType.HILL, TileType.HILL, TileType.HILL, TileType.CLAY, TileType.CLAY, 
			TileType.CLAY, TileType.MOUNTAIN, TileType.MOUNTAIN, TileType.MOUNTAIN
		};
	}
}

public enum TileType {
	DESERT,
	WOOD,
	FARM,
	HILL,
	CLAY,
	MOUNTAIN,
	OCEAN
}