using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {
	
	int portip = 9192;
	string ip = "127.0.0.1";
	public GameObject mainCam;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			GUI.Label(new Rect(Screen.width/2,Screen.height/2 - 50,200,23),"Create Host");
			GUI.Label(new Rect(Screen.width/2,Screen.height/2 + 50,200,23),"Join A Host");
			GUI.Label(new Rect(Screen.width/2 - 70,Screen.height/2,100,23),"Port:");
			portip = int.Parse(GUI.TextField(new Rect(Screen.width/2 - 30,Screen.height/2,100,23),portip.ToString()));
			if (GUI.Button(new Rect(Screen.width/2 + 100,Screen.height/2,100,23),"Create Host"))
			{
				Network.InitializeServer (10,portip);
				mainCam.AddComponent<FlyCam>();
			}
			
			
			GUI.Label(new Rect(Screen.width/2 - 70,Screen.height/2 + 90,100,23),"Port:");
			portip = int.Parse(GUI.TextField(new Rect(Screen.width/2 - 30,Screen.height/2 + 90,100,23),portip.ToString()));
			
			
			GUI.Label(new Rect(Screen.width/2 - 230,Screen.height/2 + 90,100,23),"IP:");
			ip = GUI.TextField(new Rect(Screen.width/2 - 210,Screen.height/2 + 90,130,23),ip);
			if (GUI.Button(new Rect(Screen.width/2 + 100,Screen.height/2 + 90,100,23),"Join A Host"))
			{
				Network.Connect (ip,portip);
			}
		}
	}
}
