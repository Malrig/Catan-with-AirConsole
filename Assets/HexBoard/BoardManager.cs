using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public GameObject tileObject;
	public GameObject townObject;
	public GameObject roadObject;
	public int mapRadius;
	public bool pointyTopped;

	private Dictionary<Hex, Tile> allTiles;
	private Dictionary<Vertex, Town> allTowns;
	private Dictionary<Edge, Road> allRoads;

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
	}

	private void Start() {
		InitBoard();
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void InitBoard() {
		for (int q = -mapRadius; q <= mapRadius; q++) {
			int r1 = Mathf.Max(-mapRadius, -q - mapRadius);
			int r2 = Mathf.Min(mapRadius, -q + mapRadius);

			for (int r = r1; r <= r2; r++) {
				CreateTile(new Hex(q, r));

				// Create top towns
				if (q != -mapRadius && r != mapRadius && r + q != mapRadius) {
					CreateTown(new Vertex(q, r, true));
				}
				// Create bottom towns
				if (q != +mapRadius && r != -mapRadius && r + q != -mapRadius) {
					CreateTown(new Vertex(q, r, false));
				}

				// Create Eastern roads
				if (q != mapRadius && Mathf.Abs(r) != mapRadius && r + q != mapRadius) {
					CreateRoad(new Edge(q, r, 0));
				}
				// Create Northern roads
				if (Mathf.Abs(q) != mapRadius && r != mapRadius && r + q != mapRadius) {
					CreateRoad(new Edge(q, r, +1));
				}
				// Create Southern roads
				if (q != mapRadius && r != -mapRadius && Mathf.Abs(r + q) != mapRadius) {
					CreateRoad(new Edge(q, r, -1));
				}
			}
		}
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
		newTile = newTileObj.GetComponent<TileMono>().GetTile();

    allTiles.Add(hex, newTile);
		newTile.SetUp(hex);
	}

	private void CreateTown(Vertex vertex) {
		GameObject toInstantiate = townObject;
		Vector2 instantiatePosition = vertex.VertexToScreen();
		Quaternion rotation = Quaternion.identity;
		GameObject newTownObj;
		Town newTown;

		newTownObj = Instantiate(toInstantiate, instantiatePosition, rotation) as GameObject;
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
		newRoad = newRoadObj.GetComponent<RoadMono>().GetRoad();

		allRoads.Add(edge, newRoad);
		newRoad.SetUp(edge);
	}
}
