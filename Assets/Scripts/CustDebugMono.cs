using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustDebugMono : MonoBehaviour {
	public List<ScriptType> allowedMessages;
	public int minimumPriority;
	public bool airConsoleInfo;

	private static CustDebug custDebug;

	public void Awake() {
		if (custDebug == null) {
			custDebug = new CustDebug(ScriptType.CUST_DEBUG,
															this,
															allowedMessages,
															minimumPriority);

			custDebug.Log("Created original CustDebug instance successfully.");
		}

		NDream.AirConsole.Settings.debug.info = airConsoleInfo;
	}
	
	public void LogToScreen(string debugMessage) {
		Debug.Log(debugMessage);
	}
}

public enum ScriptType {
	//GAME_MANAGER,
	BOARD_MANAGER,
	MESSAGE_MANAGER,
	//TURN_MANAGER,
	//PLAYER_CONTROLLER,
	//ACTION,
	CUST_DEBUG
}

public class CustDebug {
	private static CustDebugMono debugMono;
	private static List<ScriptType> allowedMessages;
	private static int minimumPriority;
	private ScriptType ownerScriptType;

	public CustDebug() { }
	public CustDebug(ScriptType newOwnerScriptType) {
		ownerScriptType = newOwnerScriptType;
	}
	public CustDebug(ScriptType ownerScriptType,
									CustDebugMono debugMono,
									List<ScriptType> allowedMessages,
									int minimumPriority) {
		this.ownerScriptType = ownerScriptType;
		CustDebug.allowedMessages = allowedMessages;
		CustDebug.minimumPriority = minimumPriority;
		CustDebug.debugMono = debugMono;
  }

	public void Log(string debugMessage) {
		if (allowedMessages.Count == 0 || allowedMessages.Contains(ownerScriptType)) {
			debugMono.LogToScreen(debugMessage);
		}
	}

	public void Log(string debugMessage, int priority) {
		if ((allowedMessages.Count == 0 || allowedMessages.Contains(ownerScriptType)) &&
				(priority >= minimumPriority)) {
			debugMono.LogToScreen(debugMessage);
		}
	}
}