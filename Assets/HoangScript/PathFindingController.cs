using UnityEngine;
using System.Collections;
using Pathfinding;

public class PathFindingController : MonoBehaviour {
	
	Seeker sk;
	public AstarPath path;
	Rect windownNotFound = new Rect(Screen.width/2 - 200, Screen.height/2, 420, 90);
	bool IsShowWindownNotFound = false;
	ServerControls serverControl;
	GameObject SinglePathReander;
	// Use this for initialization
	void Start () {
		serverControl = GetComponent<ServerControls>();
		sk = GetComponent<Seeker>();
		SinglePathReander = new GameObject("LineRenderer_Single", typeof(LineRenderer));
	}
	
	// Update is called once per frame
	void Update () {
		if (serverControl.IsLocationSelected() == true)
		{
			sk.StartPath (serverControl.GetStartingPoint(),serverControl.GetEndingPoint(),PathFindingCompleted);
			serverControl.PathFindingFinished();
		}
	}
	
	void PathFindingCompleted(Path p)
	{
		if (p.error)
		{
			IsShowWindownNotFound = true; 
		}
		else
		{
			LineRenderer lr = SinglePathReander.GetComponent<LineRenderer>();
			lr.SetVertexCount(p.vectorPath.Count);
			lr.SetWidth(0.1f, 0.1f);
			for (int i=0;i<p.vectorPath.Count;i++)
			{
				Vector3 pv = new Vector3(p.vectorPath[i].x,p.vectorPath[i].y+0.5F,p.vectorPath[i].z);
				lr.SetPosition(i, pv);
			}
		}
		
	}
	
	void OnGUI()
	{
		if (IsShowWindownNotFound == true)
			windownNotFound = GUI.Window(0, windownNotFound, ShowWindownNotFound, "Error");
	}
	
	void ShowWindownNotFound(int windowID) {
        GUI.Label(new Rect(10,10,400,23),"Shortest Path cannot be found. Please try again");
		if (GUI.Button(new Rect(10,35,200,23),"OK"))
		{
			IsShowWindownNotFound = false;
		}
    }
}
