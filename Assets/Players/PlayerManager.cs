using System.Collections;
using System.Collections.Generic;

public class PlayerManager {
	private Dictionary<int, Player> players;
	private CustDebug custDebug;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public PlayerManager() {
		custDebug = new CustDebug(ScriptType.PLAYER_MANAGER);

		players = new Dictionary<int, Player>();
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void CreatePlayer(int playerID) {		 
		Player newPlayer = new Player(playerID);

		if(players.Count >= 4) {
			custDebug.Warn("Creating fifth player, this is not currently supported.");
		}

		players.Add(playerID, newPlayer);
	}

	public void DeletePlayer(int playerID) {
		if (!players.ContainsKey(playerID)) {
			custDebug.Warn("Attempted to delete player which is already deleted.");
		}
		else {
			custDebug.Log("Removing player with ID: " + playerID.ToString());
			players.Remove(playerID);
		}
	}
	
	public void SetPlayersTurn(int playerID, bool playersTurn) {

	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************

	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************

}
