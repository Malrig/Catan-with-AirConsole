using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class VertexTests {

	private void NormalHexSetup() {
		// Set up the Hexs to be pointy topped centred at the origin and
		// have a size of 0.5
		Orientation ori = new Orientation(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
																			Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
																			0.5f);
		float size = 0.5f;
		Vector2 origin = new Vector2(0f, 0f);

		Layout hexLayout = new Layout(ori, size, origin);

		Hex.SetHexLayout(hexLayout);
	}

	[Test]
	public void VertexTestScreenToHex() {
		NormalHexSetup();

		Vertex vertex1 = Vertex.ScreenToVertex(new Vector2(0f, .5f));
		Vertex vertex2 = Vertex.ScreenToVertex(new Vector2(-1.3f, 1.9f));
		Vertex vertex3 = Vertex.ScreenToVertex(new Vector2(0f, -1.3f));

		Assert.AreEqual(vertex1, new Vertex(0, 0, true));
		Assert.AreEqual(vertex2, new Vertex(-3, 3, false));
		Assert.AreEqual(vertex3, new Vertex(1, -2, true));
	}


}

