using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScrollViewAdapter : MonoBehaviour
{

	public RectTransform prefab;
	public ScrollRect scrollView;
	public RectTransform content;
	public List<ItemView> views = new List<ItemView>();

	public static int SCROLL_VIEW_ITEM_WIDTH = 655;
	public static int SCROLL_VIEW_ITEM_HEIGHT = 200;

	/*public Button B_Nova;
	public Button B_Model;
	public Button B_Paint, B_Hood, B_Wheels, B_Trunk, B_Special;*/

	private string[] nova_list = new string[] { "Ryana", "Slifis" };
	private string[] model_list = new string[] { "Basic", "Fancy"};
	
	private string[] paint_list;// = new string[] { "paint 1", "paint 2", "paint 3", "paint 4", "paint 5"};
	private string[] hood_list; // = new string[] { "hood 1", "hood 2", "hood 3", "hood 4"};
    private string[] trunk_list; //= new string[] { "truck 1", "truck 2", "truck 3", "truck 4"};
	private string[] wheels_list; // = new string[] { "wheels 1", "wheels 2", "wheels 3", "wheels 4"};
    private string[] special_list; // = new string[] { "special 1", "special 2", "special 3", "special 4"};

	//private string[][] vehicle_features = new string[][] { nova_list, model_list };

	/* TYPE /*
		0 model
		1 nova
		2 hood
		3 wheels
		4 paint
		6 special
		5 trunk
	*/



    // Start is called before the first frame update
    void Start()
    {
    	this.wheels_list = new string[AssetsMgmt.assetsMgmt.wheels.Length];
    	for(int i = 0; i < this.wheels_list.Length; i++)
    	{
    		this.wheels_list[i] = AssetsMgmt.assetsMgmt.wheels[i].name;
    	}

    	this.hood_list = new string[AssetsMgmt.assetsMgmt.hoods.Length];
    	for(int i = 0; i < this.hood_list.Length; i++)
    	{
    		this.hood_list[i] = AssetsMgmt.assetsMgmt.hoods[i].name;
    	}

    	this.paint_list = new string[AssetsMgmt.assetsMgmt.paints.Length];
        for(int i = 0; i < this.paint_list.Length; i++)
    	{
    		this.paint_list[i] = AssetsMgmt.assetsMgmt.paints[i].name;
    	}

    	this.special_list = new string[AssetsMgmt.assetsMgmt.specials.Length];
        for(int i = 0; i < this.special_list.Length; i++)
    	{
    		this.special_list[i] = AssetsMgmt.assetsMgmt.specials[i].name;
    	}

        this.trunk_list = new string[AssetsMgmt.assetsMgmt.trunk.Length];
        for (int i = 0; i < this.trunk_list.Length; i++)
        {
            this.trunk_list[i] = AssetsMgmt.assetsMgmt.trunk[i].name;
        }

    }

    private void clearListView()
    {
    	foreach( Transform child in content.transform)
    	{
    		Destroy(child.gameObject);
    	}

    	views.Clear(); 
    }

    public void updateItems(string[] itemList, int type)
    {
    	ItemModel[] results = new ItemModel[itemList.Length];

        for(int i = 0; i < itemList.Length; i++)
        {
        	results[i] = new ItemModel(itemList[i], type, i );
        }

       this.clearListView();

    	foreach( ItemModel r in results)
    	{
    		var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
    		instance.transform.SetParent(this.content, false);

    		RectTransform rct = instance.GetComponent<RectTransform>();
    		rct.sizeDelta = new Vector2(SCROLL_VIEW_ITEM_WIDTH, SCROLL_VIEW_ITEM_HEIGHT);

    		ItemView view = InitializeItemView(instance, r);
    		
    		views.Add(view);
    	}
    }

    ItemView InitializeItemView(GameObject viewGO, ItemModel model)
    {
    	ItemView view = new ItemView(viewGO.transform, model.title, model.type, model.index);

    	return view;
    }

    public void OnDisable()
    {
    	this.clearListView();
    }

    public void OnModelButtonPressed()
    {
    	updateItems(model_list, 0 );
    }

    public void OnNovaButtonPressed()
    {
    	updateItems(nova_list, 1);
    }

    public void OnHoodButtonPressed()
    {
    	updateItems(hood_list, 2);
    }

    public void OnWheelsButtonPressed()
    {
    	updateItems(wheels_list, 3);
    }

    public void OnPaintButtonPressed()
    {
    	updateItems(paint_list, 4);
    }

    public void OnTrunkButtonPressed()
    {
    	updateItems(trunk_list, 5);
    }

    public void OnSpecialButtonPressed()
    {
    	updateItems(special_list, 6);
    }

    public class ItemView
    {
    	public Text titleText;
    	public Button button;

    	public ItemView(Transform root, string name, int type, int index)
    	{
    		this.titleText = root.Find("TitleText").GetComponent<Text>();
    		this.titleText.text = name;
    		this.button =  root.gameObject.GetComponent<Button>();
    		this.button.onClick.AddListener( delegate { onButtonPressed(type, index); } );
    		//add action listener to button that action listener will modify multiplayersettings.vheicleData
    		//mybe also update Vehicle_UI 
    	}

    	private void onButtonPressed(int type, int index)
    	{
    		MultiplayerSettings.multiplayerSettings.vehicleData.change(type,index);
    	}
    }

    public class ItemModel
    {
    	public string title;
    	public int type;
    	public int index;

    	public ItemModel(string name, int type, int index)
    	{
    		this.title = name;
    		this.type = type;
    		this.index = index;
    	}
    }
}
