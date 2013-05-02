using UnityEngine;
using System.Collections;

public class AddFire : MonoBehaviour {

	public GameObject firepref;
	
	void Start () {
	
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
					GameObject newf = (GameObject)Instantiate(firepref);
					newf.transform.position = hit.point;
					newf.layer = 9;
				}
			}
		}
	}
}
