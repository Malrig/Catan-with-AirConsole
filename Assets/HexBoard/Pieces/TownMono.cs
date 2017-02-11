using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is what the town class uses to perform all Unity logic
// i.e. updating appearance etc.
public class TownMono : MonoBehaviour, ITownController {

	public GameObject HighlightObject;

	private Town town;

	//*******************************************************************************************
	// Unity methods
	//*******************************************************************************************
	private void OnEnable() {
		town = new Town();
		town.SetTownController(this);
		HighlightObject.SetActive(false);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public Town GetTown() {
		return town;
	}

	//*******************************************************************************************
	// ITownController methods
	//*******************************************************************************************
	public void HighlightTown(bool highlight) {
		HighlightObject.SetActive(highlight);
  }
}

public class Town {
	private ITownController townController;
	private bool initialised;
	private Vertex vertex;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Town() {
		initialised = false;
		vertex = new Vertex(0, 0, true);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void SetUp(Vertex vertex) {
		this.vertex = vertex;

		initialised = true;
	}

	public void SetTownController(ITownController townController) {
		this.townController = townController;
	}

	public void HighlightTown(bool highlight) {
		townController.HighlightTown(highlight);
  }

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************


	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************

}

public interface ITownController {
	void HighlightTown(bool highlight);
}