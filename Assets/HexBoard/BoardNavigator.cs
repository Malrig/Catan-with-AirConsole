using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNavigator {
	// Class which consumes navigation commands and moves around the board.
	// Used when creating/upgrading towns and roads to select the correct position.
	private IVertexHighlighter vertexHighlighter;

	private Vertex currentVertex;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public BoardNavigator(IVertexHighlighter vertexHighlighter) {
		this.vertexHighlighter = vertexHighlighter;

		currentVertex = new Vertex(0, 0, true);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void MoveOnBoard(int horizontal, int vertical) {
		// No longer want old vertex to be highlighted
		vertexHighlighter.HighlightVertex(currentVertex, false);
		
		// Move the current vertex
		// Could probably rewrite all this code so that it doesn't do one space at
		// a time but not necessary as should only be doing one space at a time.
		for (int i = 0; i < horizontal; i++) {
			MoveCurrentVertexHorizontal(true);
		}
		for (int i = 0; i > horizontal; i--) {
			MoveCurrentVertexHorizontal(false);
		}
		for (int i = 0; i < vertical; i++) {
			MoveCurrentVertexVertical(true);
		}
		for (int i = 0; i > vertical; i--) {
			MoveCurrentVertexVertical(false);
		}

		// Highlight the new vertex
		vertexHighlighter.HighlightVertex(currentVertex, true);
	}

	public Vertex GetCurrentVertex() {
		return currentVertex;
	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************
	private void MoveCurrentVertexHorizontal(bool right) {
		int directionFactor = right ? 1 : -1;
		int positionFactor = currentVertex.top ? 1 : -1;

		Hex newHex = new Hex(currentVertex.hex,
												 (directionFactor - positionFactor)/2,
												 positionFactor);

		if (!vertexHighlighter.VertexOutOfBounds(new Vertex(newHex, !currentVertex.top))) {
			currentVertex = new Vertex(newHex, !currentVertex.top);
		}
	}

	private void MoveCurrentVertexVertical(bool up) {
		int directionFactor = up ? 1 : -1;
		int positionFactor = currentVertex.top ? 1 : -1;

		Hex newHex = new Hex(currentVertex.hex,
												 -(positionFactor + directionFactor)/2,
												 (positionFactor + directionFactor));

		if (!vertexHighlighter.VertexOutOfBounds(new Vertex(newHex, !currentVertex.top))) {
			currentVertex = new Vertex(newHex, !currentVertex.top);
		}
	}
}

public interface IVertexHighlighter {
	void HighlightVertex(Vertex toHighlight, bool highlight);
	bool VertexOutOfBounds(Vertex toCheck);
}
