using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerSettings : MonoBehaviour
{

	public static MultiplayerSettings multiplayerSettings;
	
	public int maxPlayers;

	public int menuScene;
	public int multiplayerScene;
	
	public VehicleData vehicleData;
	public VehicleData tempVehicleData;

	public bool delayStart;

	void Awake(){
		if(MultiplayerSettings.multiplayerSettings == null)
			MultiplayerSettings.multiplayerSettings = this;
		else if (MultiplayerSettings.multiplayerSettings != this)
				Destroy(this.gameObject);

		DontDestroyOnLoad(this.gameObject);
	}
}
