using UnityEngine;
using System.Collections;

public class stepOnBlock : MonoBehaviour {

	private GameObject parent;

	// Use this for initialization
	void Start () {
		parent = gameObject.transform.parent.gameObject;
		//Debug.Log("Started, the block is: " + theBlock.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
//		Debug.Log("Triggered");
		//Lights up the object associated with this trigger. For now only flowers.
		parent.BroadcastMessage("lightUp");
	}
}
