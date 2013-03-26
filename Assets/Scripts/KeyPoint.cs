using UnityEngine;
using System.Collections;

public class KeyPoint : MonoBehaviour
{
	void Start()
	{
		renderer.material.color = Color.blue;
	}

	void OnMouseEnter()
	{
		renderer.material.color = Color.red;
	}

	void OnMouseExit()
	{
		renderer.material.color = Color.blue;
	}

	// Click on a key point
	void OnMouseDown()
	{
		renderer.material.color = Color.green;

		// Asks player's Seeker to compute the path between player and current key point
		// Once the path will be computed, Seeker will invoke the "PathComplete" method on player
		GameObject player = GameObject.Find("Player");
		(player.GetComponent("Seeker") as Seeker).StartPath(player.transform.position, transform.position);
	}
}
