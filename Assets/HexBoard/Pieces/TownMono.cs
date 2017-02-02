using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is what the town class uses to perform all Unity logic
// i.e. updating appearance etc.
public class TownMono : MonoBehaviour, ITownController {

	private Town town;

	private void OnEnable() {
		town = new Town();
		town.SetTownController(this);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public Town GetTown() {
		return town;
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

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************


	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************

}

public interface ITownController {

}