using UnityEngine;
using System.Collections.Generic;

public class ServerControls : MonoBehaviour {
	
	Vector3 currentClickedLocation = Vector3.zero;
	Vector3 last_startingPoint = Vector3.zero;
	Vector3 last_endingPoint = Vector3.zero;
	Vector3 startingPoint = Vector3.zero;
	Vector3 endingPoint = Vector3.zero;
	public GameObject firepref;
	int fireid = 1;
	Rect windowRect = new Rect(Screen.width/2 - 480/2, Screen.height/2, 480, 30);
	Vector2 menuPosition = Vector2.zero;
	PathFindingController pathControl;
	LogPanel guilog;
	GameObject selectedFire = null;
	public GameObject startedPoint;
	public GameObject endedPoint;
	
	//context menu
	int selectedListEntry = 0;
	List<string> contextlist = new List<string>();
	bool showList = false;
	bool contextPicked = false;
	
	string CTX_SET_STARTED_POINT = "Set started point";
	string CTX_SET_END_POINT = "Set ended point";
	string CTX_SET_FIRE = "Set fire";
	string CTX_UNSET_FIRE = "Remove fire";
	string CTX_NO_ACTION = "No available action";
			
	// Use this for initialization
	void Start () {
		pathControl = GetComponent<PathFindingController>();
		guilog = GetComponent<LogPanel>();
	}
	
	public Vector3 GetStartingPoint()
	{
		return startingPoint;
	}
	
	public Vector3 GetEndingPoint()
	{
		return endingPoint;
	}
	
	public void PathFindingFinished()
	{
		startingPoint = Vector3.zero;
		endingPoint = Vector3.zero;
	}
	
	public bool IsLocationSelected()
	{
		if (startingPoint != Vector3.zero && endingPoint != Vector3.zero)
			return true;
		else
			return false;
	}
	
	// Update is called once per frame
	void Update () {
		
		startedPoint.transform.position = startingPoint;
		endedPoint.transform.position = endingPoint;
		
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{	
			if (menuPosition != Vector2.zero)
			{
				menuPosition = Vector2.zero;
			}
			else
			{
				menuPosition = new Vector2(Input.mousePosition.x,Screen.height - Input.mousePosition.y);
				showList = true;
			}
		}
		
		Ray ray;
		RaycastHit hit;
		if (Network.peerType == NetworkPeerType.Server)
		{
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject != null)
					{
						switch(hit.collider.gameObject.layer)
						{
						case 8:
							currentClickedLocation = hit.point;
							guilog.AddLLog("Selected Walkable Location:" + currentClickedLocation);
							break;
						case 10:
							selectedFire = hit.collider.gameObject;
							break;
						default:
							currentClickedLocation = Vector3.zero;
							selectedFire = null;
							break;
						}
					}
					
				}
			}
			
			/*if (Input.GetKeyUp(KeyCode.Mouse1))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject != null && 
				hit.collider.gameObject.layer == 10)
					{
						networkView.RPC("DestroyFire",RPCMode.All,hit.collider.gameObject.name);
					}
				}
			}*/
		}
		/*if (currentClickedLocation != Vector3.zero)
		{
			if (Input.GetKeyUp(KeyCode.V))
			{
				startingPoint = currentClickedLocation;
				last_startingPoint = startingPoint;
				currentClickedLocation = Vector3.zero;
			}
			else if (Input.GetKeyUp(KeyCode.B))
			{
				endingPoint = currentClickedLocation;
				last_endingPoint = endingPoint;
				currentClickedLocation = Vector3.zero;
			}
			else if (Input.GetKeyUp(KeyCode.J))
			{
				networkView.RPC("CreateFire",RPCMode.All,fireid);
				fireid++;
			}
		}*/
		
		
		if (contextPicked)
		{
			menuPosition = Vector2.zero;
			contextPicked = false;
			
			if (contextlist[selectedListEntry] == CTX_SET_END_POINT)
			{
				endingPoint = currentClickedLocation;
				last_endingPoint = endingPoint;
				currentClickedLocation = Vector3.zero;
			}
			else if (contextlist[selectedListEntry] == CTX_SET_STARTED_POINT)
			{
				startingPoint = currentClickedLocation;
				last_startingPoint = startingPoint;
				currentClickedLocation = Vector3.zero;
			}
			else if (contextlist[selectedListEntry] == CTX_SET_FIRE)
			{
				networkView.RPC("CreateFire",RPCMode.All,fireid);
				fireid++;
			}
			else if (contextlist[selectedListEntry] == CTX_UNSET_FIRE)
			{
				networkView.RPC("DestroyFire",RPCMode.All,selectedFire.name);
			}
			selectedListEntry = -1;
		}
	}
	
	[RPC]
	public void CreateFire(int id)
	{
		GameObject newf = (GameObject)Instantiate(firepref);
		newf.AddComponent<BoxCollider>();
		newf.transform.position = currentClickedLocation;
		newf.layer = 10;
		newf.name = "fire"+id;
		if (Network.peerType == NetworkPeerType.Server)
		{
			pathControl.Rescan();
			startingPoint = last_startingPoint;
		}
	}
	
	
	[RPC]
	public void DestroyFire(string id)
	{
		GameObject go = GameObject.Find(id);
		if (go)
		{
			Destroy(go);
			if (Network.peerType == NetworkPeerType.Server)
			{
				pathControl.Rescan();
				endingPoint = last_endingPoint;
			}
		}
	}
	
	void OnGUI()
	{
		
		//show menu
		contextlist.Clear();
		GUIStyle listStyle = new GUIStyle();
		Texture2D tex = new Texture2D(2, 2);
		Color[] colors = new Color[4];
		if (currentClickedLocation != Vector3.zero)
		{
			contextlist.Add(CTX_SET_STARTED_POINT);
			contextlist.Add(CTX_SET_END_POINT);
			contextlist.Add(CTX_SET_FIRE);
		}
		if (selectedFire != null)
		{
			contextlist.Add(CTX_UNSET_FIRE);
		}
		
		if (contextlist.Count == 0)
			contextlist.Add(CTX_NO_ACTION);
		
		listStyle.normal.textColor = Color.white;
		for (int i=0;i<colors.Length;i++) 
			colors[i] = Color.white;
		tex.SetPixels(colors);
		tex.Apply();
		listStyle.hover.background = tex;
		listStyle.onHover.background = tex;
		listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
		
		if (Popup.List (new Rect(menuPosition.x, menuPosition.y, 180, 20), ref showList, 
			ref selectedListEntry, new GUIContent("Click me!"), contextlist.ToArray(), listStyle,SelectedContextItem))
			contextPicked = true;
	}
	
	void SelectedContextItem()
	{
		
	}
	
	[ContextMenu ("Do Something")]
    void DoSomething () {
        Debug.Log ("Perform operation");
    }
	
	void DoMyWindow(int windowID) {
        GUI.Label(new Rect(10,10,500,23),"Press 'V' to set Starting Point, 'B' to set Ending Point, 'J' to set fire");
    }
}
