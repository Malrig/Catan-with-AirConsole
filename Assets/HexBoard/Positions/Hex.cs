using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex {
	// It is easier to store hexagon positions in cubic coordinates
	public readonly int q, r, s;

	public static Layout hexLayout;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Hex(int q, int r) {
		this.q = q; this.r = r; this.s = -q - r;
	}
	public Hex(Hex hex) {
		this.q = hex.q; this.r = hex.r; this.s = hex.s;
	}
	public Hex(Hex hex, int q_change, int r_change) : this(hex.q + q_change, hex.r + r_change) {}

	//*******************************************************************************************
	// Static methods
	//*******************************************************************************************
	public static Hex ScreenToHex(Vector2 screenPosition) {
		FractionalHex fracCoords = ScreenPosToCoords(screenPosition);

		Hex intCoords = RoundFracHex(fracCoords);

		return intCoords;
	}

	public static void SetHexLayout(Layout newHexLayout) {
		hexLayout = newHexLayout;
	}

	// Takes a screen position and outputs the position in hexagon coordinates but not rounded.
	private static FractionalHex ScreenPosToCoords(Vector2 screenPosition) {
		Orientation ori = hexLayout.orientation;
		Vector2 pt = new Vector2((screenPosition.x - hexLayout.origin.x) / hexLayout.size,
														 (screenPosition.y - hexLayout.origin.y) / hexLayout.size);

		float q = ori.b0 * pt.x + ori.b1 * pt.y;
		float r = ori.b2 * pt.x + ori.b3 * pt.y;

		return new FractionalHex(q, r, -q - r);
	}

	// Rounds the fractional coordinates to integer coordinates.
	private static Hex RoundFracHex(FractionalHex fracHex) {
		int q = (int)(Mathf.Round(fracHex.q));
		int r = (int)(Mathf.Round(fracHex.r));
		int s = (int)(Mathf.Round(fracHex.s));

		double q_diff = Mathf.Abs(q - fracHex.q);
		double r_diff = Mathf.Abs(r - fracHex.r);
		double s_diff = Mathf.Abs(s - fracHex.s);

		if (q_diff > r_diff && q_diff > s_diff) {
			q = -r - s;
		}
		else if (r_diff > s_diff) {
			r = -q - s;
		}
		else {
			s = -q - r;
		}
		return new Hex(q, r);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public Vector2 HexToScreen() {
		Orientation ori = hexLayout.orientation;

		float x = (ori.f0 * this.q + ori.f1 * this.r) * hexLayout.size;
		float y = (ori.f2 * this.q + ori.f3 * this.r) * hexLayout.size;

		return new Vector2(x + hexLayout.origin.x, y + hexLayout.origin.y);
	}

	public Vector2 HexVertexPosition(bool top) {
		Vector2 centre = HexToScreen();
		Vector2 offset;

		if (top)
			offset = TileVertexOffset(1);
		else
			offset = TileVertexOffset(4);

		return offset + centre;
	}

	public Vector2 HexEdgePosition(int direction) {
		Vector2 centre = HexToScreen();
		Vector2 offset;

		switch (direction) {
			case -1:
				offset = TileEdgeOffset(4);
				break;
			case 0:
				offset = TileEdgeOffset(5);
				break;
			case 1:
				offset = TileEdgeOffset(0);
				break;
			default:
				throw new Exception("Direction must be either -1, 0 or 1. It should not be anything else.");
		}

		return offset + centre;
	}

	public List<Hex> GetAdjacentHexes() {
		List<Hex> hexes = new List<Hex>();

		return hexes;
	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************
	// Used in tile corners to get the position of the ith corner relative to the centre of the tile
	private Vector2 TileVertexOffset(int corner) {
		float angle = 2.0f * Mathf.PI * (corner + hexLayout.orientation.start_angle) / 6f;
		return new Vector2(hexLayout.size * Mathf.Cos(angle), hexLayout.size * Mathf.Sin(angle));
	}

	// Used in TileEdge() to get the offset from the centre of the hex to the ith edge.
	private Vector2 TileEdgeOffset(int edge) {
		float angle = 2.0f * Mathf.PI * (edge + hexLayout.orientation.start_angle + 0.5f) / 6f;
		return new Vector2(hexLayout.size * Mathf.Cos(angle) * Mathf.Cos(Mathf.PI / 6f), hexLayout.size * Mathf.Sin(angle) * Mathf.Cos(Mathf.PI / 6f));
	}

	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************
	public override bool Equals(object obj) {
		if (!(obj is Hex))
			return false;

		Hex mys = (Hex)obj;

		if ((mys.q != this.q) ||
			  (mys.r != this.r) ||
			  (mys.s != this.s)) {
			return false;
		}

		return true;
	}

	public override int GetHashCode() {
		int hash = 13;
		hash = (hash * 7) + (!UnityEngine.Object.ReferenceEquals(null, q) ? q.GetHashCode() : 0);
		hash = (hash * 7) + (!UnityEngine.Object.ReferenceEquals(null, r) ? r.GetHashCode() : 0);
		hash = (hash * 7) + (!UnityEngine.Object.ReferenceEquals(null, s) ? s.GetHashCode() : 0);
		return hash;
	}

}

// Contains the information determining a hexagons position but non-integer values are allowed.
public struct FractionalHex {
	public readonly float q, r, s;

	public FractionalHex(float q, float r, float s) {
		this.q = q; this.r = r; this.s = s;
	}
}

// Holds information determining the layout of the hexagons including
// the orientation.
public struct Layout {
	public readonly Orientation orientation;
	public readonly float size; // Size of each side of the hex
	public readonly Vector2 origin; // Origin of the Hex grid

	public Layout(Orientation orientation, float size, Vector2 origin) {
		this.orientation = orientation;
		this.size = size;
		this.origin = origin;
	}
}

// Holds information determining the orientation of the hexagons, i.e.
// whether they are pointy topped or flat topped.
public struct Orientation {
	public readonly float f0, f1, f2, f3;
	public readonly float b0, b1, b2, b3;
	public readonly float start_angle; // in multiples of 60°

	public Orientation(float f0, float f1, float f2, float f3,
										 float b0, float b1, float b2, float b3,
										 float start_angle) {
		this.f0 = f0; this.f1 = f1; this.f2 = f2; this.f3 = f3;
		this.b0 = b0; this.b1 = b1; this.b2 = b2; this.b3 = b3;
		this.start_angle = start_angle;
	}
}
