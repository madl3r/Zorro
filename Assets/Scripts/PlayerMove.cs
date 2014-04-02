using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	public float maxSpeed = 5.0f;
	//public float moveForce = 365f;	
	public float jumpForce = 301f;

	public GameObject followPoint;

	private bool jump;
	public bool grounded;
	
	// Use this for initialization
	void Start () {
		jump = false;
		grounded = true;
	}

	
	
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		//		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
		}

	}
	
	void FixedUpdate ()
	{
		//Movement
		transform.Translate(new Vector3(Input.GetAxis("Horizontal") * maxSpeed * Time.deltaTime, 0, 0));

		if (Input.GetAxis("Horizontal") < 0)
		{
			//put the thing on the right
			followPoint.transform.position =  new Vector3 (transform.position.x + 0.75f, followPoint.transform.position.y, followPoint.transform.position.z);
		}
		else if (Input.GetAxis("Horizontal") > 0)
		{
			//put the thing on the left
			followPoint.transform.position =  new Vector3 (transform.position.x - 0.75f, followPoint.transform.position.y, followPoint.transform.position.z);
		}

		// If the player should jump...
		if(jump)
		{
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			//Maybe put the follow on the player in the air. When grounded return it to behind.

			jump = false;
		}
		
		
		//Can jump too much!!! :(
		grounded = Physics2D.Raycast(transform.position - (new Vector3 (0, transform.localScale.y / 1.9f, 0)) , -Vector2.up, 0.05f) 
			|| Physics2D.Raycast(transform.position - (new Vector3 (-(transform.localScale.x / 2), transform.localScale.y / 1.9f, 0)) , -Vector2.up, 0.05f)
				|| Physics2D.Raycast(transform.position - (new Vector3 ((transform.localScale.x / 2), transform.localScale.y / 1.9f, 0)) , -Vector2.up, 0.05f);
	}
	
	

}
