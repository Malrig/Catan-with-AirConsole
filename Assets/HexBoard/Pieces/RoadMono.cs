using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is what the road class uses to perform all Unity logic
// i.e. updating appearance etc.
public class RoadMono : MonoBehaviour, IRoadController {

	private Road road;

	private void OnEnable() {
		road = new Road();
		road.SetRoadController(this);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public Road GetRoad() {
		return road;
	}

	#region IRoadController implementation
	void IRoadController.SetRotation(float angleDegrees) {
		this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 360 * angleDegrees / (2 * Mathf.PI));
	}

	#endregion
}

public class Road {
	private IRoadController roadController;
	private bool initialised;
	private Edge edge;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public Road() {
		initialised = false;
		edge = new Edge(0, 0, 0);
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************
	public void SetUp(Edge edge) {
		this.edge = edge;

		roadController.SetRotation((edge.direction * Mathf.PI / 3) + Hex.hexLayout.orientation.start_angle - 0.5f);

		initialised = true;
	}
	public void SetRoadController(IRoadController roadController) {
		this.roadController = roadController;
	}

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************


	//*******************************************************************************************
	// Overridden base methods
	//*******************************************************************************************

}

public interface IRoadController {
	void SetRotation(float angleRadians);
}