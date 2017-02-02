using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is what the tile class uses to perform all Unity logic
// i.e. updating appearance etc.
public class TileMono : MonoBehaviour, ITileController {

	private Tile tile;

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

	#endregion
}

public class Tile {
	private ITileController tileController;
	private bool initialised;
	private Hex hex;

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
	public void SetUp(Hex hex) {
		this.hex = hex;

		tileController.SetRotation(Hex.hexLayout.orientation.start_angle - 0.5f);

		initialised = true;
	}
	public void SetTileController(ITileController tileController) {
		this.tileController = tileController;
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

}