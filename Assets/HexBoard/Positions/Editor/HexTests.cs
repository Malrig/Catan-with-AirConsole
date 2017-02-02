using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class HexTest {

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
	public void HexTestScreenToHex() {
		NormalHexSetup();

		Hex hex1 = Hex.ScreenToHex(new Vector2(1.5f, 0f));
		Hex hex2 = Hex.ScreenToHex(new Vector2(-1.3f, 2.3f));
		Hex hex3 = Hex.ScreenToHex(new Vector2(0f, -1.5f));
		
		Assert.AreEqual(hex1, new Hex(2, 0));
		Assert.AreEqual(hex2, new Hex(-3, 3));
		Assert.AreEqual(hex3, new Hex(1, -2));
	}

	[Test]
	public void HexTestScreenToHexNonZeroOrigin() {
		// Set up the Hexs to be flat topped centred at the origin and
		// have a size of 0.5
		Orientation ori = new Orientation(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
																			Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
																			0.5f);
		float size = 0.5f;
		Vector2 origin = new Vector2(5f, -1f);

		Layout hexLayout = new Layout(ori, size, origin);

		Hex.SetHexLayout(hexLayout);

		Hex hex1 = Hex.ScreenToHex(new Vector2(6.5f, -1f));
		Hex hex2 = Hex.ScreenToHex(new Vector2(3.3f, 1.3f));
		Hex hex3 = Hex.ScreenToHex(new Vector2(5f, -2.5f));

		Assert.AreEqual(hex1, new Hex(2, 0));
		Assert.AreEqual(hex2, new Hex(-3, 3));
		Assert.AreEqual(hex3, new Hex(1, -2));
	}

	[Test]
	public void HexTestsHexToScreen() {
		NormalHexSetup();

		Hex hex1 = new Hex(0, 0);
		Hex hex2 = new Hex(3, -2);
		Hex hex3 = new Hex(4, 1);

		Vector2 vector1 = hex1.HexToScreen();
		Vector2 vector2 = hex2.HexToScreen();
		Vector2 vector3 = hex3.HexToScreen();

		Assert.AreEqual(vector1, new Vector2(0, 0));
		Assert.AreEqual(vector2, new Vector2(Mathf.Sqrt(3f) * ((1 / 2f) * 3f + (1 / 4f) * (-2f)), 0.75f * (-2f)));
		Assert.AreEqual(vector3, new Vector2(Mathf.Sqrt(3f) * ((1 / 2f) * 4f + (1 / 4f) * 1f), 0.75f * 1f));

	}
}