using System.Collections;
using System.Collections.Generic;

public class Player {
	private Dictionary<Resource, int> resources;
	private int playerID;
	private int playerScore;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Player(int playerID) {
		initiateResources();
		this.playerID = playerID;
		playerScore = 0;
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void SetPlayerScore(int playerScore) {
		this.playerScore = playerScore;
	}

	public int GetPlayerScore() {
		return playerScore;
	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************
	private void initiateResources() {
		resources = new Dictionary<Resource, int>();
		resources.Add(Resource.WOOD, 0);
		resources.Add(Resource.WHEAT, 0);
		resources.Add(Resource.STONE, 0);
		resources.Add(Resource.BRICK, 0);
		resources.Add(Resource.WOOL, 0);
	}

	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************
}

public enum Resource {
	WOOD,
	WHEAT,
	STONE,
	BRICK,
	WOOL
}