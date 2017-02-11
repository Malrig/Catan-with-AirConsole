using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {
	// This should take a value of -1, 0, 1 depending on the edge.
	public readonly int direction; 
	public readonly Hex hex;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Edge(int q, int r, int direction) {
		if((direction != -1) && (direction != 0) && (direction != 1))
			throw new Exception("Direction must be either -1, 0 or 1. It should not be anything else.");
		this.direction = direction; this.hex = new Hex(q, r);
	}
	public Edge(Hex hex, int direction) {
		if ((direction != -1) && (direction != 0) && (direction != 1))
			throw new Exception("Direction must be either -1, 0 or 1. It should not be anything else.");
		this.direction = direction; this.hex = new Hex(hex);
	}

	//*******************************************************************************************
	// Static methods
	//*******************************************************************************************
	public static Edge ScreenToEdge(Vector2 screenPosition) {
		Hex nearestHex = Hex.ScreenToHex(screenPosition);

		Vector2 offset = screenPosition - nearestHex.HexToScreen();
		float angle = Mathf.Atan(offset.y / offset.x) / Mathf.PI; // + something to do with the layout i.e. the start_angle
																															// Normalises the angle to run between -1 and 1
		if (offset.x < 0) {
			if (offset.y < 0)
				angle += -1;
			else
				angle += 1;
		}

		// if to determine that near the edge of the tile
		if (-5f / 6f < angle && angle <= -1f / 2f)
			return new Edge(new Hex(nearestHex, 0, -1), +1);
		if (-1f / 2f < angle && angle <= -1f / 6f)
			return new Edge(new Hex(nearestHex, 0, 0), -1);
		else if (-1f / 6f < angle && angle <= 1f / 6f)
			return new Edge(new Hex(nearestHex, 0, 0), 0);
		else if (1f / 6f < angle && angle <= 1f / 2f)
			return new Edge(new Hex(nearestHex, 0, 0), +1);
		else if (1f / 2f < angle && angle <= 5f / 6f)
			return new Edge(new Hex(nearestHex, -1, 1), -1);
		else
			return new Edge(new Hex(nearestHex, -1, 0), 0);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public Vector2 EdgeToScreen() {
		Vector2 roadPos = hex.HexEdgePosition(direction);

		return roadPos;
	}

	public List<Edge> GetAdjacentEdges() {
		List<Edge> Edges = new List<Edge>();

		return Edges;
	}

	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************
	public override bool Equals(object obj) {
		if (!(obj is Edge))
			return false;

		Edge mys = (Edge)obj;

		if ((!mys.hex.Equals(this.hex)) ||
				(!mys.direction.Equals(this.direction))) {
			return false;
		}

		return true;
	}

	public override int GetHashCode() {
		int hash = 13;
		hash = (hash * 7) + (!UnityEngine.Object.ReferenceEquals(null, hex) ? hex.GetHashCode() : 0);
		hash = (hash * 7) + (!UnityEngine.Object.ReferenceEquals(null, direction) ? direction.GetHashCode() : 0);
		return hash;
	}

	public override string ToString() {
		return "(" + hex.q.ToString() + ", " + hex.r.ToString() + "," + direction.ToString() + ")";
	}
}
