using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public struct TrackPath
{
    public Transform[] pathPoints;
    public TrackPath[] nextPaths;
}

public class GameSetUp : MonoBehaviour
{
    public static GameSetUp GS;
	private string[] passivePowerNames = { "Ryana", "Slifis" };
	public Transform[] spawnPoints;
    public TrackPath aiPath;
	public GameObject startText;
	private int players;
	public GameObject AIVehicle;

    void Awake(){
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {

        	GameObject[] pu = GameObject.FindGameObjectsWithTag("PowerUp");
        	for(int i = 0; i < pu.Length; i++)
        	{
        		GameObject o = PhotonNetwork.InstantiateSceneObject( Path.Combine("PhotonPrefabs", "PowerUp"), pu[i].transform.position, Quaternion.identity, 0);
                o.transform.parent = pu[i].transform;

        	}
        }
    }

    // Update is called once per frame
    public void playerReady()
    {
    	this.players++;

        if(this.players == PhotonNetwork.PlayerList.Length)
        {
        	StartCoroutine(startRace());
        }
    }


    private IEnumerator startRace()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 6 - this.players; i++)
            {

		    	int spawnPicker = this.players + i;

      	        GameObject artificialAgent = PhotonNetwork.Instantiate(
                    Path.Combine("PhotonPrefabs", "AIVehicle"),
                    GameSetUp.GS.spawnPoints[spawnPicker].position,
                    GameSetUp.GS.spawnPoints[spawnPicker].rotation,
                    0
                );

                artificialAgent.name = "ID: " + artificialAgent.GetComponent<PhotonView>().ViewID + ", User: " + spawnPicker;
                RVP.BasicInput bi = artificialAgent.GetComponent<RVP.BasicInput>();
                bi.enabled = false;

                int[] data = new int[7];
                data[0] = Mathf.RoundToInt(Random.Range(0f,AssetsMgmt.assetsMgmt.models.Length-1));
                data[1] = 0;
                data[2] = Mathf.RoundToInt(Random.Range(0f,AssetsMgmt.assetsMgmt.hoods.Length-1));
                data[3] = Mathf.RoundToInt(Random.Range(0f,AssetsMgmt.assetsMgmt.wheels.Length-1));
                data[4] = Mathf.RoundToInt(Random.Range(0f,AssetsMgmt.assetsMgmt.paints.Length-1));
                data[5] = Mathf.RoundToInt(Random.Range(0f,AssetsMgmt.assetsMgmt.specials.Length-1));
                data[6] = Mathf.RoundToInt(Random.Range(0f,AssetsMgmt.assetsMgmt.trunk.Length-1));

                bi.data = data;

            }
        }

        this.startText.GetComponent<Text>().text = "3";
    	yield return new WaitForSeconds(1f);
    	this.startText.GetComponent<Text>().text = "2";
    	yield return new WaitForSeconds(1f);
        this.startText.GetComponent<Text>().text = "1";
    	yield return new WaitForSeconds(1f);
    	this.startText.GetComponent<Text>().text = "GO!!!";
    	enableBasicInputOnAllVehicles();
    	yield return new WaitForSeconds(0.5f);
    	this.startText.GetComponent<Text>().text = "";
    }

    private void enableBasicInputOnAllVehicles()
    {
        ArtificialAgent aiScript;
        foreach (string tag in passivePowerNames)
    	{
    		foreach(GameObject o in GameObject.FindGameObjectsWithTag(tag))
        	{
        		o.GetComponent<RVP.BasicInput>().enabled = true;
                aiScript = o.GetComponent<ArtificialAgent>();

                if (aiScript != null)
                    aiScript.enabled = true;
       		}
       	}
    }

    private void OnEnable(){
    	if(GameSetUp.GS == null) GameSetUp.GS = this; 
    }

}
