using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject toFollow;
	private float origMaxSpeed = 5.0f;
	private float currentSpeed;

	// Use this for initialization
	void Start () {
		currentSpeed = origMaxSpeed;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		//Get rid of the normalized here to move fast enough to follow dah player
		transform.position += new Vector3(((toFollow.transform.position - transform.position).x * currentSpeed * Time.deltaTime), ((toFollow.transform.position - transform.position).y * currentSpeed * Time.deltaTime), 0);
	}
}
