using UnityEngine;
using System.Collections;

public class ServerControls : MonoBehaviour {
	
	Vector3 currentClickedLocation = Vector3.zero;
	Vector3 startingPoint = Vector3.zero;
	Vector3 endingPoint = Vector3.zero;
	Rect windowRect = new Rect(Screen.width/2, Screen.height/2, 220, 30);
	
	// Use this for initialization
	void Start () {
	
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
		Ray ray;
		RaycastHit hit;
		if (Network.peerType == NetworkPeerType.Server)
		{
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit))
				{
					currentClickedLocation = hit.point;
					Debug.Log("Selected Point:" + currentClickedLocation);
				}
			}
		}
		if (currentClickedLocation != Vector3.zero)
		{
			if (Input.GetKeyUp(KeyCode.V))
			{
				startingPoint = currentClickedLocation;
				currentClickedLocation = Vector3.zero;
			}
			else if (Input.GetKeyUp(KeyCode.B))
			{
				endingPoint = currentClickedLocation;
				currentClickedLocation = Vector3.zero;
			}
		}
	}
	
	void OnGUI()
	{
		if (currentClickedLocation != Vector3.zero)
		{
			windowRect = GUI.Window(0, windowRect, DoMyWindow, "Current Point:" + currentClickedLocation);
		}
	}
	
	void DoMyWindow(int windowID) {
        GUI.Label(new Rect(10,10,200,23),"Press 'V' to set Starting Point, 'B' to set Ending Point");
    }
}
