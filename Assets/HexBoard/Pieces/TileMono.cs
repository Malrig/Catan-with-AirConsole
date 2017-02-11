using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is what the tile class uses to perform all Unity logic
// i.e. updating appearance etc.
public class TileMono : MonoBehaviour, ITileController {

	private Tile tile;

	public Text debugTileTypeText;
	public Text debugTileNumberText;

	private void OnEnable() {
		tile = new Tile();
		tile.SetTileController(this);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public Tile GetTile() {
		return tile;
	}

	#region IRoadController implementation
	void ITileController.SetRotation(float angleDegrees) {
		this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 360 * angleDegrees / (2 * Mathf.PI));
	}

	public void ShowTileType(TileType tileType) {
		debugTileTypeText.gameObject.SetActive(true);
		debugTileTypeText.text = tileType.ToString();
  }

	public void ShowTileNumber(int tileNumber) {
		debugTileNumberText.gameObject.SetActive(true);
		debugTileNumberText.text = tileNumber.ToString();
	}

	public void HideAllDebug() {
		debugTileTypeText.gameObject.SetActive(false);
		debugTileNumberText.gameObject.SetActive(false);
	}
	#endregion
}

public class Tile {
	private ITileController tileController;
	private bool initialised;
	private Hex hex;

	private int tileDieNumber;
	private TileType tileType;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Tile() {
		initialised = false;
		hex = new Hex(0, 0);
	}	

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void SetUp(Hex hex, int tileDieNumber, TileType tileType) {
		this.hex = hex; this.tileDieNumber = tileDieNumber; this.tileType = tileType;

		tileController.SetRotation(Hex.hexLayout.orientation.start_angle - 0.5f);
		initialised = true;
	}

	public void SetTileController(ITileController tileController) {
		this.tileController = tileController;
	}

	public void DisplayDebug() {
		tileController.ShowTileNumber(tileDieNumber);
		tileController.ShowTileType(tileType);
	}

	public void HideDebugDebug() {
		tileController.ShowTileNumber(tileDieNumber);
		tileController.ShowTileType(tileType);
	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************


	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************

}

public interface ITileController {
	void SetRotation(float angleRadians);
	void ShowTileType(TileType tileType);
	void ShowTileNumber(int tileNumber);
	void HideAllDebug();
}