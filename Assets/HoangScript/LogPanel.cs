using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public Vector2 scrollPosition;
    List<string> longString = new List<string>();
	
    void OnGUI() {
		GUILayout.Space (Screen.height - 300);
		GUILayout.BeginHorizontal ("box");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(200), GUILayout.Height(300));
		for (int i=longString.Count - 1;i>=0;i--)
        	GUILayout.Label(longString[i]);
		if (longString.Count > 1000)
			longString.Clear();
		
        GUILayout.EndScrollView();
		GUILayout.EndHorizontal ();
    }
	
	public void AddLLog(string log)
	{
		longString.Add(log);
	}
	
	public void AddBLog(string log)
	{
		AddLLog(log);
		networkView.RPC("BroastcastLog",RPCMode.Others,log);
	}
	
	[RPC]
	public void BroastcastLog(string log)
	{
		longString.Add(log);
	}
}
