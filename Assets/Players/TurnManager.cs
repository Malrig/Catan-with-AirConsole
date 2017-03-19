using System.Collections;
using System.Collections.Generic;

public class TurnManager {
	private int currentPlayer;
	private List<int> playerOrder;
	private PlayerManager playerManager;
	private CustDebug custDebug;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public TurnManager(PlayerManager playerManager) {
		custDebug = new CustDebug(ScriptType.TURN_MANAGER);

		this.playerManager = playerManager;
		playerOrder = new List<int>();
  }

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void AddPlayerToOrder(int playerID) {
		playerOrder.Add(playerID);
		if (playerOrder.Count == 1) {
			currentPlayer = playerID;
		}
	}

	public void RemovePlayerFromOrder(int playerID) {
		if (playerOrder.Contains(playerID)) {
			playerOrder.Remove(playerID);
		}
		else {
			custDebug.Warn("Attempted to remove player from order which does not exist.");
		}
	}

	public void NextPlayersTurn() {
		playerManager.SetPlayersTurn(currentPlayer, false);

		IncrementCurrentPlayer();

		playerManager.SetPlayersTurn(currentPlayer, true);
	}

	public void BeginGame() {
		playerManager.SetPlayersTurn(currentPlayer, true);
	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************
	private void IncrementCurrentPlayer() {
		int currentPlayerIndex = playerOrder.IndexOf(currentPlayer);

		if (currentPlayerIndex + 1 == playerOrder.Count) {
			currentPlayer = playerOrder[0];
		}
		else {
			currentPlayer = playerOrder[currentPlayerIndex + 1];
		}
	}

	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************
}
