using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float moveSpeed = 10;
	
	private Vector3 previousPosition;
	private float previousTime;
	
	// Called by Seeker on successful path computation
	public void PathComplete(Vector3[] path)
	{
		Debug.Log("Starting move...");
		// Asks iTween to move the player along the path found by Seeker
		/*iTween.MoveTo(gameObject, iTween.Hash
		(
			"path", path,
			"orienttopath", true,
			"axis", "y",
			"speed", moveSpeed,
			"easetype", "linear",
			"oncomplete", " OnMoveToPathComplete"
		));*/
		iTween.MoveTo(gameObject, iTween.Hash
		(
			"path", path,
			"orienttopath", true,
			"looktime", 1.0,
			"lookahead", 0.05,
			"axis", "y",
			"easetype", iTween.EaseType.linear,
			"time", iTween.PathLength(path) / moveSpeed,
			"oncomplete", "OnMoveComplete"
		));
		animation.CrossFade("Run");
	}
	
	// Called by Seeker on path computation failure
	public void PathError()
	{
		Debug.Log("No path found :(");
	}
	
	// Called by iTween when target has been reached
	public void OnMoveComplete()
	{
		Debug.Log("Move complete!");
		animation.CrossFade("Idle");
	}
}
