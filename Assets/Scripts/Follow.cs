using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public GameObject toFollow;

	private float origMaxSpeed = 5.0f;
	private float maxSpeed;
	//public float moveForce = 365f;	
	private float jumpForce = 280;
	private float followPtDisplace;
	private Vector3 actualFollowPoint;
	private bool wantsToJump;
	private float jumpWaitTime;
	private float timeOfJump;


	public bool grounded;

	// Use this for initialization
	void Start () {
		origMaxSpeed = Random.Range(3f, 5.2f);
		followPtDisplace = Random.Range(-0.5f, 0.5f);
		actualFollowPoint = new Vector3(toFollow.transform.position.x + followPtDisplace, toFollow.transform.position.y, toFollow.transform.position.z);
		jumpWaitTime = Random.Range(0.1f, 2f);
	}
	
	// Update is called once per frame
	void Update () {


		//Randomly jumping cuz is cute
		if (Time.time - timeOfJump > jumpWaitTime)
		{
			wantsToJump = true;
			jumpWaitTime = Random.Range(0.1f, 2f);
		}

		actualFollowPoint = new Vector3(toFollow.transform.position.x + followPtDisplace, toFollow.transform.position.y, toFollow.transform.position.z);

		maxSpeed = Mathf.Min(Mathf.Abs(actualFollowPoint.x  -  transform.position.x) * 2.5f, origMaxSpeed);

		//maxSpeed = Mathf.Min(Mathf.Abs(toFollow.transform.position.x  -  transform.position.x) * 2.5f, origMaxSpeed);

		//Jump sometimes

		//Definitely jump if the player has jumped (if the difference in y is enough between this transform and the other one)
	}

	void FixedUpdate()
	{


		//things to do while we're in the air
		if (!grounded)
		{
			//float downwards
			rigidbody2D.AddForce(new Vector2(0f, jumpForce * 0.01f));
			
			//Always be checking if we're grounded
			grounded = Physics2D.Raycast(transform.position - (new Vector3 (0, transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.02f, 1 << 10)  //layer mask so only jump on ground
				|| Physics2D.Raycast(transform.position - (new Vector3 (-(transform.localScale.x / 2.5f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.02f, 1 << 10)
					|| Physics2D.Raycast(transform.position - (new Vector3 ((transform.localScale.x / 2.5f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.02f, 1 << 10);
		}

	//IF FOLLOWING~~~~
	//~~~~~~~~~~~~~~~~
		//Normalized so that speed is based on only the maximum speed
		//transform.position += new Vector3(((toFollow.transform.position - transform.position).normalized.x * maxSpeed * Time.deltaTime), 0, 0);

		//X following (movement)

		//If this doesn't change in a long enough time (Probably actually a very short time) 
		//stop following and try your own thing (if fay away then move in the opposite direction for some time and then keep following)
		//Otherwise if close enough do some cute stuff (spawn hearts?)
		transform.position += new Vector3(((actualFollowPoint - transform.position).normalized.x * maxSpeed * Time.deltaTime), 0, 0);

		//Jump!
		if((toFollow.transform.position.y - transform.position.y  > 1.0f) && grounded)
		{
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			grounded = false; //so that we only jump once
			wantsToJump = false;
			timeOfJump = Time.time;
			Debug.Log("JUMPING");
			//Maybe put the follow on the player in the air. When grounded return it to behind.
			
			//jump = false;
		}
	
		if ((wantsToJump) && grounded)
		{
			rigidbody2D.AddForce(new Vector2(0f, Random.Range(150, 200)));
			grounded = false; //so that we only jump once
			wantsToJump = false;
			timeOfJump = Time.time;
			Debug.Log("JUMPING");
		}
	//~~~~~~~~~~~~~~~


		//Always if not grounded float downwards slightly

	}
}
