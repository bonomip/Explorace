using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsMgmt : MonoBehaviour
{

	public static AssetsMgmt assetsMgmt;

	public GameObject ryana, slifis;

	public Mesh[] models;

	public Material[] wheels;

	public GameObject[] hoods;

	public Material[] paints;

	public GameObject[] specials;

    public GameObject[] trunk;

    // Start is called before the first frame update
    void Awake()
    {
        if(assetsMgmt == null)
        	assetsMgmt = this;
    }
}
