using UnityEngine;
using System.Collections;

public class spawnForever : MonoBehaviour {

	public GameObject grassBlock;
	public GameObject[] backGroundObjects;
	public GameObject underGroundBlock;

	private GameObject theCharacter;
	private Vector3 spawnPosition;
	private int randNum;
	private float prevYPos;
	private float prevXPos;

	private bool actviated;

	// Use this for initialization
	void Start () {
		actviated = false;
	}
	
	// Update is called once per frame
	void Update () {
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		//		Debug.Log("Triggered");
		if (!actviated)
		{
			spawnBlocks();
			actviated = true;
		}
	}

	void spawnBlocks()
	{
//		Debug.Log("Spawning More Blocks!!");
		//spawn background blocks
		//nine blocks in the y
		//start spawn at -19, to -35 // 380 - 404

		Vector3 thePos = new Vector3(transform.position.x + 100, transform.position.y - 19, 0);
		//Vector3 theRot = new Vector3(0, 0, 0);

		//Spawn the basic land (grass underground, and next blocks trigger.
		thePos.y -= 1;
		Instantiate (underGroundBlock, thePos, Quaternion.identity);
		thePos.y += 1;

		thePos.y += 9.25f;
		Instantiate (grassBlock, thePos, Quaternion.identity);
		thePos.y -= 9.25f;

		thePos.y += 19f;
		Instantiate(gameObject, thePos, Quaternion.identity);
		thePos.y -= 19f;


		//thePos.y += 9.25f;
		prevYPos = thePos.y;
		prevXPos = thePos.x;
		//Just do that hack way ... look at which object you're spawning and add to the Y based on that.
		// Do the x randomly each time between -48 and +48.

		//Spawning background object at hights corresponding to what the objects are.
		for (int i = 0; i < Random.Range (10, 23); i++)
		{
			randNum = Random.Range(0, backGroundObjects.Length);
			if (randNum == 0)
				thePos.y += 9.8f;
			else if (randNum == 1)
				thePos.y += 9.25f;
			else if (randNum == 2)
				thePos.y += 12;
			else if (randNum == 3)
				thePos.y += 11;
			
			thePos.x += Random.Range(-48, 48);

			Instantiate(backGroundObjects[randNum], thePos, Quaternion.identity);
			thePos.y = prevYPos;
			thePos.x = prevXPos;
		}

	}
}
