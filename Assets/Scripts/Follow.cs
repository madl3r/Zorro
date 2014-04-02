using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public GameObject toFollow;

	public float origMaxSpeed = 3.0f;
	private float maxSpeed;
	//public float moveForce = 365f;	
	public float jumpForce = 200;
	
	private bool jump;
	public bool grounded;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		maxSpeed = Mathf.Min(Mathf.Abs(toFollow.transform.position.x  -  transform.position.x) * 2.5f, origMaxSpeed);

		//transform.position = toFollow.transform.position;



		//Jump sometimes
		//Definitely jump if the player has jumped (if the difference in y is enough between this transform and the other one)
	}

	void FixedUpdate()
	{
		transform.position += new Vector3(((toFollow.transform.position - transform.position).x * maxSpeed * Time.deltaTime), 0, 0);


		if(toFollow.transform.position.y - transform.position.y > 1.0f && grounded)
		{
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			Debug.Log("JUMPING");
			
			//Maybe put the follow on the player in the air. When grounded return it to behind.
			
			//jump = false;
		}
		
		
		//Can jump too much!!! :(
		grounded = Physics2D.Raycast(transform.position - (new Vector3 (0, transform.localScale.y / 2, 0)) , -Vector2.up, 0.05f) 
			|| Physics2D.Raycast(transform.position - (new Vector3 (-(transform.localScale.x / 2), transform.localScale.y / 2, 0)) , -Vector2.up, 0.05f)
				|| Physics2D.Raycast(transform.position - (new Vector3 ((transform.localScale.x / 2), transform.localScale.y / 2, 0)) , -Vector2.up, 0.05f);

	}
}
