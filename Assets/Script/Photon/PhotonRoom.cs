using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks 
{

	public static PhotonRoom room;
	private PhotonView PV;

	public bool isGameLoaded;
	public int currentScene;

	private Player[] photonPlayers;
	public int playersInRoom;
    public int myNumberInRoom;
	
    public int playersInGame; 

	//delayed start
	private bool readyToCount;
	private bool readyToStart;
	public float startingTime;
	private float lessThenMaxPlayers;
	private float atMaxPlayers;
	private float timeToStart;

	public Text feedBackText;
	public Text feedBackText2;

    // Start is called before the first frame update
    void Awake()
    {
        if(PhotonRoom.room == null)
        	PhotonRoom.room = this;
        else if (PhotonRoom.room != this)
        {
        	Destroy(PhotonRoom.room.gameObject);
        	PhotonRoom.room = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable(){
    	base.OnEnable();
    	PhotonNetwork.AddCallbackTarget(this);
    	SceneManager.sceneLoaded +=  OnSceneFinishedLoading;

    }
    public override void OnDisable(){
    	base.OnDisable();
    	PhotonNetwork.RemoveCallbackTarget(this);
    	SceneManager.sceneLoaded -=  OnSceneFinishedLoading;
    }

    void Start()
    {
    	PV = GetComponent<PhotonView>();
    	readyToCount = false;
    	readyToStart = false;
    	lessThenMaxPlayers = startingTime;
    	atMaxPlayers = 6;
    	timeToStart = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(MultiplayerSettings.multiplayerSettings.delayStart)
        {
            if(playersInRoom == 1)
        	   RestartTimer();

           
            if(!isGameLoaded)
            {
              
        	   if(readyToStart)
        	   {
        		  atMaxPlayers -= Time.deltaTime;
        		  lessThenMaxPlayers = atMaxPlayers;
        		  timeToStart = atMaxPlayers;
        	   }
               else if (readyToCount)
        	   {
        		  lessThenMaxPlayers -= Time.deltaTime;
        		  timeToStart = lessThenMaxPlayers;
        	   }
        	   
               if(timeToStart < 10)
               		this.feedBackText.text = "Game will start in: " + timeToStart.ToString("F1");
        	   
               if(timeToStart <= 0)
               {
        	       StartGame();
        	   }
            }
        }	
    }

    public override void OnJoinedRoom(){
    	base.OnJoinedRoom();

    	this.feedBackText.text = "We are now in a room!";

    	photonPlayers = PhotonNetwork.PlayerList;
    	playersInRoom = photonPlayers.Length;
    	myNumberInRoom = playersInRoom;
    	PhotonNetwork.NickName = myNumberInRoom.ToString();

        if(MultiplayerSettings.multiplayerSettings.delayStart)
        {
            this.feedBackText2.text = "players: " + playersInRoom + " / " + MultiplayerSettings.multiplayerSettings.maxPlayers;

            if(playersInRoom > 1) //TODO the game will start if at least 2 players are in the room
            {
                readyToCount = true;
            } 

            if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers) //if the room is full
            {
                readyToStart = true;
                
                if(!PhotonNetwork.IsMasterClient)
                    return;
                else 
                    PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    	base.OnPlayerEnteredRoom(newPlayer);
    	this.feedBackText.text = "A new player has joined the room";
    	photonPlayers = PhotonNetwork.PlayerList;
    	playersInRoom++;


        if(MultiplayerSettings.multiplayerSettings.delayStart)
        {
            this.feedBackText2.text = "players: " + playersInRoom + " / " + MultiplayerSettings.multiplayerSettings.maxPlayers;
            if(playersInRoom > 1)
            {
    		  readyToCount = true;
            }

    	   if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
    	   {
    		  readyToStart = true;
    		  
              if(!PhotonNetwork.IsMasterClient)
    			 return;
    		  else
                PhotonNetwork.CurrentRoom.IsOpen = false;
    	   }
        }
    }



    void StartGame()
    {
    	isGameLoaded = true;
        
        if(!PhotonNetwork.IsMasterClient)
            return;
        if(MultiplayerSettings.multiplayerSettings.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; 
        }

    	PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);

    }

    void RestartTimer(){
    	lessThenMaxPlayers = startingTime;
    	timeToStart = startingTime;
    	atMaxPlayers = 6;
    	readyToCount = false;
    	readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
    	currentScene = scene.buildIndex;
    	if(currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene){
    		
            isGameLoaded = true;

            if(MultiplayerSettings.multiplayerSettings.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }  
            else
            {
                RPC_CreatePlayer();    
            }
    	}
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
    	playersInGame++;
    	if(playersInGame == PhotonNetwork.PlayerList.Length)
        {
    		PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

   	[PunRPC]
   	private void RPC_CreatePlayer() //TODO
   	{
   		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
   	}


























}
