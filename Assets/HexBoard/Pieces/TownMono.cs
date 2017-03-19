using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is what the town class uses to perform all Unity logic
// i.e. updating appearance etc.
public class TownMono : MonoBehaviour, ITownController {

	public GameObject highlightObject;
	public GameObject settlement;
	public GameObject city;

	private Town town;

	//*******************************************************************************************
	// Unity methods
	//*******************************************************************************************
	private void OnEnable() {
		town = new Town();
		town.SetTownController(this);
		highlightObject.SetActive(false);
		settlement.SetActive(false);
		city.SetActive(false);
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
		highlightObject.SetActive(highlight);
  }

	public void DisplayTownState(TownState townState) {
		if(townState == TownState.SETTLEMENT) {
			settlement.SetActive(true);
			city.SetActive(false);
		}
		else if (townState == TownState.CITY) {
			settlement.SetActive(false);
			city.SetActive(true);
		}
		else {
			settlement.SetActive(false);
			city.SetActive(false);
		}
  }
}

public class Town {
	private ITownController townController;
	private CustDebug custDebug;

	private bool initialised;
	private Vertex vertex;
	private TownState townState;
	private int playerOwnerID;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Town() {
		custDebug = new CustDebug(ScriptType.TOWN);

		initialised = false;
		vertex = new Vertex(0, 0, true);
		playerOwnerID = 0;
		townState = TownState.EMPTY;

		custDebug.Log("New town created.", 0);
  }

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void SetUp(Vertex vertex) {
		this.vertex = vertex;

		initialised = true;

		custDebug.Log("Town SetUp() at vertex " + vertex.ToString(), 0);
	}

	public void SetTownController(ITownController townController) {
		this.townController = townController;
	}

	public void HighlightTown(bool highlight) {
		CheckInitialised();
		townController.HighlightTown(highlight);
  }

	public bool CanChangeTownState(TownState townState, int playerOwnerID) {
		CheckInitialised();
		custDebug.Log("Check if town at " + vertex.ToString() + " can change state.", 5);
		
		if ((this.townState == TownState.EMPTY) ||
				((playerOwnerID == this.playerOwnerID) &&
				 (this.townState == TownState.SETTLEMENT) &&
				 (townState == TownState.CITY))) {
			custDebug.Log("It can change state.", 5);
			return true;
		}
		else {
			custDebug.Log("It can't change state.", 5);
			return false;
		}
	}

	public void SetTownState(TownState townState) {
		CheckInitialised();
		custDebug.Log("SetTownState(" + townState.ToString() + ").", 5);
		custDebug.Warn("This should not be callable by the player only setup scripts.");

    this.townState = townState;
		townController.DisplayTownState(townState);
  }

	public void SetTownState(TownState townState, int playerOwnerID) {
		CheckInitialised();
    custDebug.Log("SetTownState(" + townState.ToString() + "," + playerOwnerID + ").", 5);

		if (!CanChangeTownState(townState, playerOwnerID)) {
			custDebug.Warn("SetTownState called when players cannot create a town.");
		}

		this.townState = townState;
		this.playerOwnerID = playerOwnerID;
		townController.DisplayTownState(townState);
	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************
	private void CheckInitialised() {
		if (!initialised) {
			custDebug.Warn("Town is not initialised.");
		}
	}

	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************

}

public interface ITownController {
	void HighlightTown(bool highlight);
	void DisplayTownState(TownState townState);
}

public enum TownState {
	EMPTY,
	SETTLEMENT,
	CITY
}