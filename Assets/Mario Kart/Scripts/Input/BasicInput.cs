using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;

namespace RVP
{
    [RequireComponent(typeof(VehicleParent))]
    [DisallowMultipleComponent]
    [AddComponentMenu("RVP/Input/Basic Input", 0)]


    //Class for setting the input with the input manager
    public class BasicInput : MonoBehaviour
    {
        VehicleParent vp;
        public PhotonView PV;
        GameObject powerUpButton;
        int id;
        public int[] data;

        public bool debug;
        public bool useTouch;

        private bool leavingRoom;
 

        public int powerUp; // 0 none, 1 speed boos, 2 spikes, 3 laser
        private bool stunts;

        private int accel, brake, steer;
        private Transform checkPoint;
        private int cpCount;
        public bool isArtificialAgent;

        public string accelAxis;
        public string brakeAxis;
        public string steerAxis;
        public string ebrakeAxis;
        public string boostButton;
        public string upshiftButton;
        public string downshiftButton;
        public string pitchAxis;
        public string yawAxis;
        public string rollAxis;



        void Start()
        {
            this.vp = GetComponent<VehicleParent>();
            this.PV = GetComponent<PhotonView>();
            this.powerUpButton = GameObject.Find("b_PowUp");
            this.isArtificialAgent = this.GetComponent<ArtificialAgent>() != null;
            this.powerUp = 0;
            this.cpCount = 0;
            this.id = this.PV.InstantiationId;

            if(useTouch){
            	this.accel = 1;
                if (!isArtificialAgent)
                {
                    addLeftSteerButtonEvents();
                    addRightSteerButtonEvents();
                    addBreakButtonEvents();
                    GameObject.Find("b_PowUp").GetComponent<Button>().onClick.AddListener(OnPowerUpButtonClicked);
                }
            } else {
            	GameObject.Find("Canvas").SetActive(false);
            }

            this.PV.RPC("RPC_UpdateVehicleData", RpcTarget.All, data, this.id);
        }

        void Update()
        {
           //Get single-frame input presses
            if (!string.IsNullOrEmpty(upshiftButton))
            {
                if (Input.GetButtonDown(upshiftButton))
                {
                    vp.PressUpshift();
                }
            }

            if (!string.IsNullOrEmpty(downshiftButton))
            {
                if (Input.GetButtonDown(downshiftButton))
                {
                    vp.PressDownshift();
                }
            }
        }

        void FixedUpdate()
        {

		if(stunts) { stuntsIde(); return; }
        	


        	if(!PV.IsMine && !debug) return;

        	if(useTouch)
        	{

                vp.SetAccel(this.accel);

                vp.SetBrake(this.brake);

                vp.SetSteer(this.steer);

        	} else {
				//Get constant inputs
            	if (!string.IsNullOrEmpty(accelAxis))
            	{
                vp.SetAccel(Input.GetAxis(accelAxis));
            	}

            	if (!string.IsNullOrEmpty(brakeAxis))
            	{
                vp.SetBrake(Input.GetAxis(brakeAxis));
            	}

            	if (!string.IsNullOrEmpty(steerAxis))
            	{
            	vp.SetSteer(Input.GetAxis(steerAxis));	
            	}

            	if (!string.IsNullOrEmpty(ebrakeAxis))
            	{
                vp.SetEbrake(Input.GetAxis(ebrakeAxis));
            	}

            	if (!string.IsNullOrEmpty(boostButton))
            	{
                vp.SetBoost(Input.GetButton(boostButton));
            	}

            	if (!string.IsNullOrEmpty(pitchAxis))
            	{
                vp.SetPitch(Input.GetAxis(pitchAxis));
            	}

            	if (!string.IsNullOrEmpty(yawAxis))
            	{
                vp.SetYaw(Input.GetAxis(yawAxis));
            	}

            	if (!string.IsNullOrEmpty(rollAxis))
            	{
                vp.SetRoll(Input.GetAxis(rollAxis));
            	}

            	if (!string.IsNullOrEmpty(upshiftButton))
            	{
                vp.SetUpshift(Input.GetAxis(upshiftButton));
            	}

            	if (!string.IsNullOrEmpty(downshiftButton))
            	{
            	    vp.SetDownshift(Input.GetAxis(downshiftButton));
            	}
        	}
        }

        private void addLeftSteerButtonEvents(){
        		EventTrigger trigger = GameObject.Find("b_Left").AddComponent<EventTrigger>();
            	
            	EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            	pointerDownEntry.eventID = EventTriggerType.PointerDown;
          		pointerDownEntry.callback.AddListener( ( data ) => { OnLeftButtonDown(); } );
          		trigger.triggers.Add(pointerDownEntry);

          		EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            	pointerUpEntry.eventID = EventTriggerType.PointerUp;
          		pointerUpEntry.callback.AddListener( ( data ) => { OnLeftButtonUp(); } );
          		trigger.triggers.Add(pointerUpEntry);
        }
        
        private void addRightSteerButtonEvents(){
        		EventTrigger trigger = GameObject.Find("b_Right").AddComponent<EventTrigger>();
            	
            	EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            	pointerDownEntry.eventID = EventTriggerType.PointerDown;
          		pointerDownEntry.callback.AddListener( ( data ) => { OnRightButtonDown(); } );
          		trigger.triggers.Add(pointerDownEntry);

          		EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            	pointerUpEntry.eventID = EventTriggerType.PointerUp;
          		pointerUpEntry.callback.AddListener( ( data ) => { OnRightButtonUp(); } );
          		trigger.triggers.Add(pointerUpEntry);
        }

        private void addBreakButtonEvents(){
        		EventTrigger triggerL = GameObject.Find("b_BreakL").AddComponent<EventTrigger>();
        		EventTrigger triggerR = GameObject.Find("b_BreakR").AddComponent<EventTrigger>();
            	
            	EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            	pointerDownEntry.eventID = EventTriggerType.PointerDown;
          		pointerDownEntry.callback.AddListener( ( data ) => { OnBreakButtonDown(); } );
          		triggerL.triggers.Add(pointerDownEntry);
          		triggerR.triggers.Add(pointerDownEntry);

          		EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            	pointerUpEntry.eventID = EventTriggerType.PointerUp;
          		pointerUpEntry.callback.AddListener( ( data ) => { OnBreakButtonUp(); } );
          		triggerL.triggers.Add(pointerUpEntry);
          		triggerR.triggers.Add(pointerUpEntry);
        }


        public void OnBreakButtonUp(){
        	brake = 0;
        	accel = 1;
        }

        public void OnBreakButtonDown(){
        	brake = 1;
        	accel = 0;

        }

        public void OnLeftButtonUp(){
        	steer = 0;
        }

        public void OnLeftButtonDown(){
        	steer = -1;
        }

        public void OnRightButtonUp(){
        	steer = 0;
        }

        public void OnRightButtonDown(){
        	steer = 1;
        }

        public void getPowerUp(int powerUp)
        {
        	if(this.powerUp != 0) return; // if i have already a power up i keep the first one

            this.powerUp = powerUp; //todo mixing passive power and power up

            if(PV.IsMine && !isArtificialAgent){
            	string s;
            	switch (this.powerUp)
            	{
                case 0:
                    s = "";
                    break;
                case 1:
                    s = "BOOST";
                    break;
                case 2:
                    s = "STONES";        
                    break;
                case 3:
                    s = "SPEAR";
                    break;
                default:
                  	s = "";
                    break;
            	}
            	this.powerUpButton.GetComponentInChildren<Text>().text = s;
        	}
        }

        public void OnPowerUpButtonClicked(){
            if (PV.IsMine || debug)
                this.usePowerUp();
        }

        private IEnumerator useBoost(float time)
        {
        	yield return new WaitForSeconds(time);

            //camera returns in the right position
            //GameObject.Find("Main Camera").GetComponent<CameraControl>().isBoosting = false;

            vp.SetBoost(false);
        }

        private void SpawnSpike()
        {

        	GameObject o = this.gameObject;

        	for(int i = 0; i < 3; i++)
        	{
				Vector3 p = o.transform.position - o.transform.forward * 3f;
        		GameObject t = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spike"), p + new Vector3(0,1,0), o.transform.rotation, 0);
        	}
        }

        private void ShootLaser()
        {

            Vector3 p = this.gameObject.transform.position + this.gameObject.transform.forward * 3f;
            GameObject t = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Laser"), p + new Vector3(0, 0.3f, 0), this.gameObject.transform.rotation, 0);

            t.transform.Rotate(0, 90, 0);
            t.GetComponent<Rigidbody>().velocity = t.transform.TransformDirection(new Vector3(-100, 0, 0));
        }

        private void stuntsIde()
        {
        	if(GetComponent<Rigidbody>().velocity.magnitude > 1){
        		vp.SetBrake(1); 
        		vp.SetAccel(0);
        	} else {
        		GetComponent<Rigidbody>().velocity = Vector3.zero;
        		vp.SetBrake(0);
        		vp.SetAccel(0);
        	}
        }

        public void onSpikeCollision()
        {
        	this.stunts = true;
        	StartCoroutine(waitStunts(1f));
        }

        public void onLaserCollision()
        {
            this.stunts = true;
            StartCoroutine(waitStunts(1f));
        }

        private IEnumerator waitStunts(float time)
        {
        	yield return new WaitForSeconds(time);
        	this.stunts = false;
        }

        public void leaveRoom()
        {
        	if(this.PV.IsMine && !this.leavingRoom)
        	{
        		this.leavingRoom = true;
        		Destroy(GameObject.Find("RoomController"));
        		PhotonNetwork.LeaveRoom(true);
        		SceneManager.LoadScene(0);
        	}
        }

        public void turnRight()
        {
            steer = 1;
        }
        
        public void turnLeft()
        {
            steer = -1;
        }

        public void goStraight()
        {
            steer = 0;
        }

        public void accelerate()
        {
            accel = 1;
            brake = 0;
        }

        public void brakeV()
        {
            accel = 0;
            brake = 1;
        }

        public void usePowerUp()
        {
            switch (powerUp)
            {
                case 0:
                    //Debug.Log("none");

                    if (!isArtificialAgent)
                        this.cpCount++;

                    if (this.cpCount == 3) //USE THIS FOR RESET
                    {
                        this.transform.position = this.checkPoint.position;
                        this.transform.rotation = this.checkPoint.rotation;
                        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        this.cpCount = 0;
                    }
                    break;
                case 1:
                    
                    this.cpCount = 0;
                    vp.SetBoost(true);

                    //camera begins to go close
                    //GameObject.Find("Main Camera").GetComponent<CameraControl>().isBoosting = true;

                    StartCoroutine(useBoost(4f));
                    break;
                case 2:
                    
                    this.cpCount = 0;
                    SpawnSpike();
                    break;
                case 3:
                    
                    this.cpCount = 0;
                    ShootLaser();
                    break;
            }

            powerUp = 0;

            if (!isArtificialAgent)
            	this.powerUpButton.GetComponentInChildren<Text>().text = "";
        }

        public void setCheckPoint(Transform cp)
        {
            this.checkPoint = cp;
        }

    [PunRPC]

    private void RPC_UpdateVehicleData(int[] data, int id)
    {
    	Debug.Log(data.Length);
    	if(id == this.id)
    		new VehicleData(data[0], data[1], data[2], data[3], data[4], data[5], data[6], null).ApplyTo(this.gameObject);
    }
}
}