using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class MessageManager {

	private List<int> availablePlayerIds;
	private ReverseIntDictionary playerIdControllerIdMap;
	private CustDebug custDebug;

	//*******************************************************************************************
	// Constructors
	//*******************************************************************************************
	public MessageManager() {
		custDebug = new CustDebug(ScriptType.MESSAGE_MANAGER);
		
		availablePlayerIds = new List<int> { 1, 2, 3, 4 };
		playerIdControllerIdMap = new ReverseIntDictionary();

		// Register AirConsole events
		AirConsole.instance.onReady += OnReady;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
		AirConsole.instance.onMessage += OnMessage;
	}

	//*******************************************************************************************
	// Public methods
	//*******************************************************************************************

	//*******************************************************************************************
	// Private methods
	//*******************************************************************************************
	private void OnReady(string code) {
		// Fires once the AirConsole instance is ready. Will need to use this to stop the game 
		// trying to perform any logic before everything is good.
		custDebug.Log("AirConsole instance is ready.");
  }

	private void OnConnect(int device_id) {
		// Fires whenever a controller connects to the game
		// When a controller connects assign them the lowest player id available.
		// If no id is available then send a message informing them that the game is full.
		if (availablePlayerIds.Count > 0) {
			custDebug.Log("New controller connected, device id " + device_id.ToString() + "assigned player id " + availablePlayerIds[0].ToString() + ".");
			playerIdControllerIdMap.Add(availablePlayerIds[0], device_id);
			availablePlayerIds.RemoveAt(0);
		}
		else {
			// TODO send message to controller letting them know the game is full.
			custDebug.Log("New controller connected, no player ids available.");
		}
	}

	private void OnDisconnect(int device_id) {
		// Fires whenever a controller disconnects from the game
		// Remove the controller id from the map and make that player id available again.
		if (playerIdControllerIdMap.ContainsValue(device_id)) {
			int newAvailablePlayerId = playerIdControllerIdMap.GetKey(device_id);
			playerIdControllerIdMap.RemoveAtValue(device_id);

			availablePlayerIds.Add(newAvailablePlayerId);

			custDebug.Log("Controller disconnected, new player id available " + newAvailablePlayerId.ToString() + ".");
		}
		else {
			// Do nothing it was just an observer.
			custDebug.Log("Controller disconnected, no new player id available as was observer.");
		}
	}

	private void OnMessage(int from, JToken data) {
		// This is the method called whenever a message is received from a controller. Will need 
		// to put all the logic here which processes and then performs the actiosn the controller
		// will attempt to make.
	}
}

public class ReverseIntDictionary {
	private Dictionary<int, int> keyToValueDict;
	private Dictionary<int, int> valueToKeyDict;

	public ReverseIntDictionary() {
		keyToValueDict = new Dictionary<int, int>();
		valueToKeyDict = new Dictionary<int, int>();
	}

	public void Add(int key, int value) {
		if (!keyToValueDict.ContainsKey(key) &&
			  !valueToKeyDict.ContainsKey(value)) {
			keyToValueDict.Add(key, value);
			valueToKeyDict.Add(value, key);
		}
		else {
			throw new System.Exception("Key or value already exists.");
		}
	}

	public int GetKey(int value) {
		return valueToKeyDict[value];
	}

	public int GetValue(int key) {
		return keyToValueDict[key];
	}

	public bool ContainsKey(int key) {
		return keyToValueDict.ContainsKey(key);
	}

	public bool ContainsValue(int value) {
		return valueToKeyDict.ContainsKey(value);
	}

	public void RemoveAtKey(int key) {
		int value = keyToValueDict[key];

    keyToValueDict.Remove(key);
		valueToKeyDict.Remove(value);
	}

	public void RemoveAtValue(int value) {
		int key = valueToKeyDict[value];

		keyToValueDict.Remove(key);
		valueToKeyDict.Remove(value);
	}
}
