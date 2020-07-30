using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{

	public static PhotonLobby lobby;


	public bool delayStart;

	public GameObject vehicleUI;
	
	public GameObject editVehicleCanvas;
	public GameObject menuCanvas;

	public GameObject debugButton;
	public GameObject editButton;
	public GameObject battleButton;
	public GameObject cancelButton;
	


	private void Awake(){
		lobby = this;
	}

    // Start is called before the first frame update
    void Start()
    {

    	debugButton.GetComponentInChildren<Text>().text = MultiplayerSettings.multiplayerSettings.delayStart ? "NORMAL" : "DEBUG";

    	this.vehicleUI = GameObject.Find("Vehicle_UI");

        PhotonNetwork.ConnectUsingSettings();

        if(MultiplayerSettings.multiplayerSettings.vehicleData == null){
			MultiplayerSettings.multiplayerSettings.vehicleData = new VehicleData(this.vehicleUI);
			MultiplayerSettings.multiplayerSettings.tempVehicleData = VehicleData.GetInstance(MultiplayerSettings.multiplayerSettings.vehicleData);
		}
		else
		{
			MultiplayerSettings.multiplayerSettings.vehicleData.UI = this.vehicleUI;
			MultiplayerSettings.multiplayerSettings.vehicleData.UpdateUI();
		}
	}

    public override void OnConnectedToMaster()
    {
    	Debug.Log("Connected to master");
    	PhotonNetwork.AutomaticallySyncScene = true; //when master client load scene also other client load that scene
    	battleButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
    	Debug.Log("Failed to join a random room. there must be no open games available");
    	CreateRoom();
    }

    void CreateRoom(){
    	Debug.Log("trying create new room");
    	int randomRoomName = Random.Range(0, 10000);
    	RoomOptions roomOpt = new RoomOptions(){ IsVisible = true, IsOpen = true, MaxPlayers = (byte)  MultiplayerSettings.multiplayerSettings.maxPlayers };
    	PhotonNetwork.CreateRoom(" Room " + randomRoomName, roomOpt );
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
		Debug.Log("Tried to create a room but failed, there should already be a room with the same name");
		CreateRoom();
    }



    public void OnBattleButtonClicked()
    {
		battleButton.SetActive(false);
    	cancelButton.SetActive(true);
    	this.editButton.SetActive(false);
    	PhotonNetwork.JoinRandomRoom();
    	Debug.Log("Joining random Room");

    }

    public void OnCancelButtonCLicked(){
    	cancelButton.SetActive(false);
    	battleButton.SetActive(true);
    	this.editButton.SetActive(true);
    	PhotonNetwork.LeaveRoom();
    	GameObject.Find("FeedbackText").GetComponent<Text>().text = "";
    	GameObject.Find("FeedbackText2").GetComponent<Text>().text = "";
    }

    public void OnDebugButtonClicked(){
		MultiplayerSettings.multiplayerSettings.delayStart = !MultiplayerSettings.multiplayerSettings.delayStart;
		debugButton.GetComponentInChildren<Text>().text = MultiplayerSettings.multiplayerSettings.delayStart ? "NORMAL" : "DEBUG";
	}

	public void OnEditButtonClicked()
	{
		this.editVehicleCanvas.SetActive(true);
		this.menuCanvas.SetActive(false);
	}

	public void OnConfirmButtonPressed()
    {
		this.editVehicleCanvas.SetActive(false);
		this.menuCanvas.SetActive(true);
		MultiplayerSettings.multiplayerSettings.tempVehicleData = VehicleData.GetInstance(MultiplayerSettings.multiplayerSettings.vehicleData);
    }

    public void OnGoBackButtonPressed()
    {
		this.editVehicleCanvas.SetActive(false);
		this.menuCanvas.SetActive(true);
		MultiplayerSettings.multiplayerSettings.vehicleData = VehicleData.GetInstance(MultiplayerSettings.multiplayerSettings.tempVehicleData);
		MultiplayerSettings.multiplayerSettings.vehicleData.UpdateUI();
    }
}
