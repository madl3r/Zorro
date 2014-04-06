using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {
	
	
	public GameObject elevator;
	private Vector3 startPos, endPos;
	private bool started;
	
	// Use this for initialization
	void Start () {
		started = false;
		startPos = new Vector3 (elevator.transform.position.x, elevator.transform.position.y, elevator.transform.position.z);
		endPos = new Vector3 (startPos.x, -35f, startPos.z);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//When the player enters the foyer close the door behind them.
	void OnTriggerEnter2D(Collider2D other) {
		if (!started)
		{
			started = true;
			StartCoroutine(ElevatorDown());
		}
	}
	
	//Using coroutine so that waits feel good.
	//Lerps the door closed.
	IEnumerator ElevatorDown()
	{
		float i = 0;
		Debug.Log("Entered Elevator");
		yield return new WaitForSeconds(2f);

		Debug.Log("SHOULD BE MOVING DOWN");

		while(i < 1.0f)
		{
			i += Time.deltaTime * 0.05f;
			//Debug.Log(i);
			elevator.transform.position = Vector3.Lerp(startPos, endPos, i);
			yield return new WaitForSeconds(0.005f);
		}
		yield break;
	}
	
	void returnBlockToStart()
	{
		elevator.transform.position = startPos;
	}
	
}