using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessingWithBoardsManager : MonoBehaviour {

	public BoardManager boardManager;

	private BoardNavigator boardNavigator;

	// Use this for initialization
	void Start () {
		boardNavigator = new BoardNavigator(boardManager);
	}

	public void Move(int move) {
		if(move == 1) {
			boardNavigator.MoveOnBoard(1, 0);
		}
		else if (move == 2) {
			boardNavigator.MoveOnBoard(-1, 0);
		}
		else if (move == 3) {
			boardNavigator.MoveOnBoard(0, 1);
		}
		else if (move == 4) {
			boardNavigator.MoveOnBoard(0, -1);
		}
	}
}
