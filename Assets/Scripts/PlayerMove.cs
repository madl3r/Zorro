using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float maxSpeed = 5.0f;
	public float jumpForce = 301f;

	public float maxJumpSpeed = 10;

	public GameObject followPoint;
	public float followDistance;

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

		//Set the follow point to behind the player when they turn around.
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

		//If the player hasn't moved in a little while move the point to x = 0

		//When ducks are close enough to the fox for a little while they spawn hearts.



		// If the player should jump...
		if(jump)// && transform.rigidbody2D.velocity.y < 7) //(Make sure that we can't super jump on a wall)
		{
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			//Maybe put the follow on the player in the air. When grounded return it to behind.

			jump = false;
		}

		transform.rigidbody2D.velocity = new Vector2(transform.rigidbody2D.velocity.x, Mathf.Min(transform.rigidbody2D.velocity.y, maxJumpSpeed));
		
		//Checking if the middle, left and right bottom side of the player are touching the ground.
		grounded = Physics2D.Raycast(transform.position - (new Vector3 (0, transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.05f, 1 << 10) 
			|| Physics2D.Raycast(transform.position - (new Vector3 (-(transform.localScale.x / 1.7f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.05f, 1 << 10)
				|| Physics2D.Raycast(transform.position - (new Vector3 ((transform.localScale.x / 1.7f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.05f, 1 << 10);
	}
	
	

}
