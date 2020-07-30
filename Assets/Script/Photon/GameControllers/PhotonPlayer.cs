using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.IO;
using RVP;

public class PhotonPlayer : MonoBehaviour
{

	private PhotonView PV;
	public GameObject myAvatar;

	private RVP.CameraControl camCtrl;
	private RVP.BasicInput bi;
	private GameObject cam;


	void Awake(){
	}

    // Start is called before the first frame update
    void Start()
    {
		this.PV = GetComponent<PhotonView>();
    
        int spawnPicker = PhotonRoom.room.myNumberInRoom - 1;
        
        if(this.PV.IsMine)
        {
        	this.myAvatar = PhotonNetwork.Instantiate(
        			Path.Combine("PhotonPrefabs", "Vehicle"),
        			GameSetUp.GS.spawnPoints[spawnPicker].position,
        			GameSetUp.GS.spawnPoints[spawnPicker].rotation,
        			0
        		); //TODO SELECT THE RIGHT AVATar */

        	this.myAvatar.name = "ID: " + this.PV.ViewID + ", User: " + spawnPicker;
        	this.gameObject.name = "PhotonNetworkPlayer: " + spawnPicker;

        	this.cam = GameObject.Find("Main Camera");
			this.camCtrl = this.cam.GetComponent<RVP.CameraControl>();
        	this.camCtrl.setTarget( this.myAvatar.gameObject.transform );
        	this.camCtrl.Initialize();

        	this.myAvatar.GetComponent<RVP.BasicInput>().enabled = false;
        	
        	this.PV.RPC("RPC_PlayerIsReady", RpcTarget.All);

        	this.bi = this.myAvatar.GetComponent<RVP.BasicInput>();
        	this.bi.data = VehicleData.toArray(MultiplayerSettings.multiplayerSettings.vehicleData);

        	//ADD ARROW
        	GameObject arrow = (GameObject) GameObject.Instantiate(
        			Resources.Load("Prefabs/DirArrow"),
        			new Vector3( 	this.myAvatar.transform.position.x, 
        							this.myAvatar.transform.position.y + 1,
        							this.myAvatar.transform.position.z
        						),
        			Quaternion.identity,
        			this.myAvatar.transform
        			);

            arrow.GetComponent<Arrow>().setVehicle(this.myAvatar);

        	//APPLY CHANGES TO VEHICLE
        	MultiplayerSettings.multiplayerSettings.vehicleData.ApplyTo(this.myAvatar);
        	this.PV.RPC("RPC_updateVehicleData", RpcTarget.All, this.myAvatar.name, VehicleData.toArray(MultiplayerSettings.multiplayerSettings.vehicleData) );

        }
    }


    [PunRPC]

    private void RPC_PlayerIsReady() //TODO
   	{
   		GameSetUp.GS.playerReady();
   	}

   	[PunRPC]

   	private void RPC_updateVehicleData(string vehicleName, int[] data)
   	{
   		new VehicleData(data).ApplyTo(GameObject.Find(vehicleName));
   	}
}
