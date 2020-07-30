using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleData
{
	public int model, nova, hood, wheels, paint, special, trunk;
	public GameObject UI;

	/* TYPE /*
		0 model
		1 nova
		2 hood
		3 wheels
		4 paint
		6 special
		5 trunk
	*/

	/* NOVA /*
		slifis
	*/

	public VehicleData(GameObject vehicleUI)
	{
		this.UI = vehicleUI;
		this.UpdateUI();
	}

	public VehicleData(int[] data)
	{
		this.model = data[0];
		this.nova = data[1];
		this.hood = data[2];
		this.wheels = data[3];
		this.paint = data[4];
		this.special = data[5];
		this.trunk = data[6];
	}

	public VehicleData(int model, int nova, int hood, int wheels, int paint, int special, int trunk, GameObject UI)
	{
		this.model = model;
		this.nova = nova;
		this.hood = hood;
		this.wheels = wheels;
		this.paint = paint;
		this.special = special;
		this.trunk = trunk;
		this.UI = UI;
	}

	public static VehicleData GetInstance(VehicleData VD)
	{
		return new VehicleData(VD.model, VD.nova, VD.hood, VD.wheels, VD.paint, VD.special, VD.trunk, VD.UI );
	}

	public void change(int type, int index)
	{
		switch(type)
		{
			case 0: this.model = index; break;
			case 1: this.nova = index; break;
			case 2: this.hood = index; break;
			case 3: this.wheels = index; break;
			case 4: this.paint = index; break;
			case 6: this.special = index; break;
			case 5: this.trunk = index; break;
			default: break;
		}

		UpdateUI();
	}

	public void UpdateUI()
	{
		GameObject body = this.UI.transform.Find("body").gameObject;
		GameObject[] tires = getTires(this.UI);

		this.applyNovaTo(this.UI, body);
		this.applyWheelsTo(tires);
		this.applyHoodTo(this.UI);
		this.applyModelTo(body);
		this.applyPaintTo(body);
		this.applySpecialTo(this.UI);
        this.applyTrunkTo(this.UI);


        //this.checkThemeUI(this.UI, tires, body);
    }

	public void ApplyTo(GameObject vehicle)
	{
		//APPLY CHANGES TO REAL GAMEOBJECT VEHICLE (only when race is begin start)
		Debug.Log("apply changes to: " + vehicle.name);

		GameObject body = vehicle.transform.Find("body").gameObject;
		GameObject[] tires = this.getTires(vehicle);

		this.applyNovaTo(vehicle, body);
		this.applyWheelsTo(tires);
		this.applyHoodTo(vehicle);
		this.applyModelTo(body);
		this.applyPaintTo(body);
		this.applySpecialTo(vehicle);
        this.applyTrunkTo(vehicle);

        //check if the customization follows a particular theme to activate the particle effect
        checkTheme(vehicle, tires, body);

	}

	private void applyNovaTo(GameObject vehicle, GameObject body )
	{
		if(vehicle.GetComponent<RyanaPP>() != null) GameObject.Destroy(vehicle.GetComponent<RyanaPP>());
		if(vehicle.GetComponent<SlifisPP>() != null) GameObject.Destroy(vehicle.GetComponent<SlifisPP>());
		if(vehicle.GetComponent<Collider>() != null) GameObject.Destroy(vehicle.GetComponent<Collider>());
		if(vehicle.transform.Find("nova") != null)
			GameObject.Destroy(vehicle.transform.Find("nova").gameObject);

		switch(this.nova)
		{
			case 0: 
			vehicle.AddComponent<RyanaPP>();
			GameObject.Instantiate(AssetsMgmt.assetsMgmt.ryana, vehicle.transform).name = "nova";
			break;

			case 1: 
			vehicle.AddComponent<SlifisPP>();
			GameObject.Instantiate(AssetsMgmt.assetsMgmt.slifis, vehicle.transform).name = "nova";
			break;

			default:
			vehicle.AddComponent<RyanaPP>();
			GameObject.Instantiate(AssetsMgmt.assetsMgmt.ryana, vehicle.transform).name = "nova";
			break;
		}
	}

	private void applyWheelsTo(GameObject[] tires )
	{
		for(int i = 0; i < tires.Length; i++)
			tires[i].GetComponent<Renderer>().material = AssetsMgmt.assetsMgmt.wheels[this.wheels];
	}

	private void applyHoodTo(GameObject vehicle)
	{
		if(vehicle.transform.Find("hood") != null)
			GameObject.Destroy(vehicle.transform.Find("hood").gameObject);

		GameObject.Instantiate(AssetsMgmt.assetsMgmt.hoods[this.hood], vehicle.transform).name = "hood";
	}

	private void applyModelTo(GameObject body)
	{
		body.GetComponent<MeshFilter>().mesh = AssetsMgmt.assetsMgmt.models[this.model];	
	}

	private void applyPaintTo(GameObject body)
	{
		body.GetComponent<Renderer>().material = AssetsMgmt.assetsMgmt.paints[this.paint];
	}

	private void applySpecialTo(GameObject vehicle)
	{
		if(vehicle.transform.Find("special") != null)
			GameObject.Destroy(vehicle.transform.Find("special").gameObject);
			
		GameObject.Instantiate(AssetsMgmt.assetsMgmt.specials[this.special], vehicle.transform).name = "special";
	}

    private void applyTrunkTo(GameObject vehicle)
    {
        if (vehicle.transform.Find("trunk") != null)
            GameObject.Destroy(vehicle.transform.Find("trunk").gameObject);

        GameObject.Instantiate(AssetsMgmt.assetsMgmt.trunk[this.trunk], vehicle.transform).name = "trunk";
    }

    private GameObject[] getTires(GameObject kart)
	{
		GameObject[] tires = new GameObject[4];

		tires[0] = kart.transform.Find("suspensionFL/wheel/rim/tire").gameObject;
		tires[1] = kart.transform.Find("suspensionFR/wheel/rim/tire").gameObject;
		tires[2] = kart.transform.Find("suspensionRL/wheel/rim/tire").gameObject;
		tires[3] = kart.transform.Find("suspensionRR/wheel/rim/tire").gameObject;

		return tires;
	}

    private void checkTheme(GameObject vehicle, GameObject[]tires, GameObject body) //hard coded per via di come sono stati messi gli item
    {
        vehicle.GetComponent<ParticleSystem>().enableEmission = false;

        //Debug.Log("check 0");

        if (tires[0].GetComponent<Renderer>().material.name.Equals("AzurePalette (Instance)"))
        {
            //Debug.Log("first check");
            if (vehicle.transform.Find("hood").GetComponent<MeshFilter>() != null)
            {
                if (vehicle.transform.Find("hood").GetComponent<MeshFilter>().mesh.name.Equals("Crystal_2 Instance"))
                {
                    //Debug.Log("second check");
                    if (body.GetComponent<Renderer>().material.name.Equals("YellowPhantom (Instance)"))
                    {
                        //Debug.Log("third check");
                        if (vehicle.transform.Find("special").childCount != 0)
                        {
                            if(vehicle.transform.Find("special").GetChild(0).GetComponent<MeshFilter>().mesh.name.Equals("Detached_Rim_Col Instance"))
                            {
                                //Debug.Log("fourth check");
                                if (vehicle.transform.Find("trunk").GetComponent<MeshFilter>() != null)
                                {
                                    if (vehicle.transform.Find("trunk").GetComponent<MeshFilter>().mesh.name.Equals("Spoiler Instance"))
                                    {

                                    }
                                    Debug.Log("theme activated");
                                    vehicle.GetComponent<ParticleSystem>().enableEmission = true;
                                }
                            }
                            
                        }
                    }
                }
                
            }
        }
    }

    //private void checkThemeUI(GameObject vehicle, GameObject[] tires, GameObject body) //hard coded per via di come sono stati messi gli item
    //{

    //    //Debug.Log("check 0");

    //    if (tires[0].GetComponent<Renderer>().material.name.Equals("AzurePalette (Instance)"))
    //    {
    //        //Debug.Log("first check");
    //        if (vehicle.transform.Find("hood").GetComponent<MeshFilter>() != null)
    //        {
    //            if (vehicle.transform.Find("hood").GetComponent<MeshFilter>().mesh.name.Equals("Crystal_2 Instance"))
    //            {
    //                //Debug.Log("second check");
    //                if (body.GetComponent<Renderer>().material.name.Equals("YellowPhantom (Instance)"))
    //                {
    //                    //Debug.Log("third check");
    //                    if (vehicle.transform.Find("special").childCount != 0)
    //                    {
    //                        if (vehicle.transform.Find("special").GetChild(0).GetComponent<MeshFilter>().mesh.name.Equals("Detached_Rim_Col Instance"))
    //                        {
    //                            //Debug.Log("fourth check");
    //                            if (vehicle.transform.Find("trunk").GetComponent<MeshFilter>() != null)
    //                            {
    //                                if (vehicle.transform.Find("trunk").GetComponent<MeshFilter>().mesh.name.Equals("Spoiler Instance"))
    //                                {

    //                                }
    //                                Debug.Log("theme activated UI");
    //                            }
    //                        }

    //                    }
    //                }
    //            }

    //        }
    //    }
    //}

    public static int[] toArray(VehicleData data)
    {
    	return new int[] { data.model, data.nova, data.hood, data.wheels, data.paint, data.special, data.trunk };
    }









































}
