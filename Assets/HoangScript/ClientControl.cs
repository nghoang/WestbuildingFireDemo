using UnityEngine;
using System.Collections;

public class ClientControl : MonoBehaviour {
	
	
	public GameObject mainCharPref;
	public GameObject mainCam;
	GameObject mainChar;
	string myid;
	// Use this for initialization
	void Start () {
		myid = System.Guid.NewGuid().ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.peerType == NetworkPeerType.Client)
		{
			networkView.RPC("ClientMoving",RPCMode.Others,
				myid,
				mainChar.transform.position,
				mainChar.transform.rotation);
		}
	}
	
	public void AddCharacter()
	{
		if (Network.peerType == NetworkPeerType.Client)
		{
			mainChar = (GameObject)Instantiate(mainCharPref);
			mainChar.name = myid;
			mainChar.transform.position = new Vector3(-15.88798F,1.013724F,-3.551169F);
			mainChar.GetComponent<ThirdPersonCamera>().cameraTransform = mainCam.transform;
			networkView.RPC("SpawnChar",RPCMode.Others,
				myid);
		}
	}
	
	[RPC]
	public void SpawnChar(string id)
	{
		GameObject go = (GameObject)Instantiate(mainCharPref);
		go.name = id;
		go.transform.position = new Vector3(-15.88798F,1.013724F,-3.551169F);
		Destroy(go.GetComponent<ThirdPersonController>());
		Destroy(go.GetComponent<ThirdPersonCamera>());
	}
	
	[RPC]
	public void ClientMoving(string id,Vector3 newPos, Quaternion newRo)
	{
		Debug.Log("Moving: " + id);
		GameObject c = GameObject.Find(id);
		c.transform.position = newPos;
		c.transform.rotation = newRo;
	}
	
	public void OnConnectedToServer() {
        Debug.Log("Connected to server");
		AddCharacter();
    }
}
