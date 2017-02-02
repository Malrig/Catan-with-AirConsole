using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex {
	// This is true if the vertex is at the top of hex and false if at the bottom.
	public readonly bool top;
	public readonly Hex hex;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Vertex(int q, int r, bool top) {
		this.top = top; this.hex = new Hex(q, r);
	}
	public Vertex(Hex hex, bool top) {
		this.top = top; this.hex = new Hex(hex);
	}

	//*******************************************************************************************
	// Static methods
	//*******************************************************************************************
	public static Vertex ScreenToVertex(Vector2 screenPosition) {
		bool top = true;
		Hex nearestHex = Hex.ScreenToHex(screenPosition);

		Vector2 offset = screenPosition - nearestHex.HexToScreen();

		float angle = Mathf.Atan(offset.y / offset.x) / Mathf.PI; // TODO + something to do with the layout i.e. the start_angle

		if (offset.x < 0) {
			if (offset.y < 0)
				angle += -1;
			else
				angle += 1;
		}

		// Return the correct vertex based on the angle to the mouse.
		if (-1f < angle && angle <= -2f / 3f)
			return new Vertex(new Hex(nearestHex, 0, -1), top);
		if (-2f / 3f < angle && angle <= -1f / 3f)
			return new Vertex(new Hex(nearestHex, 0, 0), !top);
		else if (-1f / 3f < angle && angle <= 0f)
			return new Vertex(new Hex(nearestHex, 1, -1), top);
		else if (0f < angle && angle <= 1f / 3f)
			return new Vertex(new Hex(nearestHex, 0, 1), !top);
		else if (1f / 3f < angle && angle <= 2f / 3f)
			return new Vertex(new Hex(nearestHex, 0, 0), top);
		else
			return new Vertex(new Hex(nearestHex, -1, 1), !top);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public Vector2 VertexToScreen() {
		Vector2 vertexPos = hex.HexVertexPosition(top);

		return vertexPos;
	}


	public List<Vertex> GetAdjacentVertices() {
		List<Vertex> vertices = new List<Vertex>();

		return vertices;
	}

	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************
	public override bool Equals(object obj) {
		if (!(obj is Vertex))
			return false;

		Vertex mys = (Vertex)obj;

		if ((!mys.hex.Equals(this.hex)) ||
			  (!mys.top.Equals(this.top))) {
			return false;
		}

		return true;
	}

	public override int GetHashCode() {
		int hash = 13;
		hash = (hash * 7) + (!Object.ReferenceEquals(null, hex) ? hex.GetHashCode() : 0);
		hash = (hash * 7) + (!Object.ReferenceEquals(null, top) ? top.GetHashCode() : 0);
		return hash;
	}
}
