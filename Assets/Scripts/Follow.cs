using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public GameObject toFollow;

	private float origMaxSpeed = 5.0f;
	private float currentSpeed;
	//public float moveForce = 365f;	
	private float jumpForce = 280;
	private float followPtDisplace;
	private Vector3 actualFollowPoint;

	private bool wantsToJump;
	private bool following;
	private bool isIdle;
	private bool isAnnealing;
	private bool overGap;
	private bool seriousGapBuisinessTime;
	private bool gapIsClose;
	private bool inAirFromGap;

	private float jumpWaitTime;
	private float timeOfJump;
	private float idleWaitTime = 2.0f; //5 seconds
	private float timeOfLastMove;
	private float seriousJumpWaitTime;

	private float annealingTime = 1.0f;
	private float updateWhileNotAnnealing;


	private float prevX;


	public bool grounded;

	// Use this for initialization
	void Start () {
		origMaxSpeed = Random.Range(4f, 5.2f);
		followPtDisplace = Random.Range(-0.5f, 0.5f);
		actualFollowPoint = new Vector3(toFollow.transform.position.x + followPtDisplace, toFollow.transform.position.y, toFollow.transform.position.z);
		jumpWaitTime = Random.Range(0.1f, 2f);
		jumpForce += Random.Range(-10, 5);
		following = true;
		isIdle = false;
		isAnnealing = false;
		overGap = false;
		seriousGapBuisinessTime = false;
		gapIsClose = false;
		seriousJumpWaitTime = 0.21f;
		
	}
	
	// Update is called once per frame
	void Update () {

		//DO ALWAYS

		//Randomly jumping cuz is cute
		if (Time.time - timeOfJump > jumpWaitTime)
		{
			wantsToJump = true;
			jumpWaitTime = Random.Range(0.3f, 2f);
		}


		//Follow Point and Speed Calculations
		actualFollowPoint = new Vector3(toFollow.transform.position.x + followPtDisplace, toFollow.transform.position.y, toFollow.transform.position.z);
		currentSpeed = Mathf.Min(Mathf.Abs(actualFollowPoint.x  -  transform.position.x) * 2.5f, origMaxSpeed);

		//Used to be in fixed update
		//If parent is close enough stop annealing and follow!
		if ((actualFollowPoint - transform.position).magnitude < 1)
		{
			seriousGapBuisinessTime = false;
			
			if (!following)
			{
				following = true;
				isAnnealing = false;
				//Move a little faster the first frame that we meet the parent
				FollowPoint();
				FollowPoint();
			}
		}


		//Checking if Idle

		//If we're moving fast enough then update our last move time
		if (Mathf.Abs(prevX - transform.position.x) > 0.02f)
		{
			timeOfLastMove = Time.time;
		}
	
		//If it's been too long since a detectable move then we're in Idle mode!
		if (Time.time - timeOfLastMove > idleWaitTime)
		{
			isIdle = true;
//			Debug.Log("Is IDLE");
		}
		else
		{
			isIdle = false;
		}

		//Setting the previous x position
		prevX = transform.position.x;

		//Kill if low enough
		if (transform.position.y < -100)
		{
			Debug.Log("Die");
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		//Movement

		//things to do while we're in the air
		//Always if not grounded float downwards slightly
		if (!grounded)
		{
			GlideDown();
			checkIsGrounded();
		}





		//If we're idle and far enough away try to move bakcwards a bit and then go back to following.
		if(isIdle && (actualFollowPoint - transform.position).magnitude > 2 && !isAnnealing)
		{

			following = false;
			isAnnealing = true;
			annealingTime = Random.Range(0.3f, 1.3f);
//			Debug.Log("WHERE'S PARENT FREAK OUT!");
		}

	

		//IF FOLLOWING~~~~
		//~~~~~~~~~~~~~~~~
		if (following)
		{
			isAnnealing = false;

			FollowPoint();
		//	Debug.Log("FOLLOWING");

			//Jump!
			if((toFollow.transform.position.y - transform.position.y  > 1.0f) && grounded && !seriousGapBuisinessTime && (Time.time - timeOfJump > seriousJumpWaitTime))//&& !gapIsClose // && isIdle)
			{
				Jump(jumpForce);
				//Debug.Log("JUMPING, trying");
				//Debug.Log(gapIsClose);
			}
			
			if ((wantsToJump) && grounded && !seriousGapBuisinessTime)// && !gapIsClose)
			{
				Jump(Random.Range(150, 190));
				//Debug.Log("JUMPING, cute");
			}
		}

		//Can't get to parent, so try going back a second and then continue following!
		if (isAnnealing && !following)
		{
			//while we should be running away run away
			if (Time.time - updateWhileNotAnnealing < annealingTime)
			{
				//Debug.Log("SHOULD BE RUNNING AWAY");
				//Minus equals so that running away.
				RunFromPoint();

			}
			else
			{
				//Debug.Log("~~~~~~~RUNNING ELSE~~~~~~~");
				//Jump(jumpForce);
				isAnnealing = false;
				following = true;
				updateWhileNotAnnealing = Time.time;
			}
		}
		else if (!isAnnealing)
		{
			updateWhileNotAnnealing = Time.time;
		}
		//~~~~~~~~~~~~~~~



		//Now we should do smart jumping
		//Have two raycasts, if either one returns false try jumping at full power.
		//If not grounded then stop following, anneal and then move in the direction again for one jump
		//While in the air here never anneal!
		//If we don't make it then that sucks... :'(

		//Also maybe add some other raycast and a bool "gapIsClose"
		//If a gap is close enough turn off all follow and for fun jumping!
		gapIsClose = !(Physics2D.Raycast(transform.position - (new Vector3 (Random.Range(-5.0f, 0.0f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 4, 1 << 10)
		               && Physics2D.Raycast(transform.position - (new Vector3 (Random.Range(0f, 5f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 4, 1 << 10));

		//If we sense a close gap reset the time so that we can't jump for funsies
		if (gapIsClose && !isIdle)
		{
			//reset the want to jump times
			timeOfJump = Time.time;
		}

		overGap = !(Physics2D.Raycast(transform.position - (new Vector3 (-(transform.localScale.x / 2f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 4, 1 << 10)
			&& Physics2D.Raycast(transform.position - (new Vector3 ((transform.localScale.x / 2f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 4, 1 << 10));

		if (overGap && grounded)
		{
			Debug.Log("Trying to jump the gap");
			Jump(jumpForce);
			seriousGapBuisinessTime = true;
		}
		else if(overGap && !grounded && !seriousGapBuisinessTime)
		{
			annealingTime = Random.Range(0.8f, 1.0f);//(0.3f, 0.5f);
			following = false;
			isAnnealing = true;
			seriousGapBuisinessTime = true;
			inAirFromGap = true;
		}

		//Smarter running back, this makes it so that when they hit the ground they don't keep going backwards
		if(inAirFromGap && grounded && isAnnealing)
		{
			inAirFromGap = false;
			isAnnealing = false;
			following = true;
		}

	}


	private void Jump(float jumpForce)
	{
		//Double check to see if we're grounded
		if (grounded)
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		grounded = false; //so that we only jump once
		wantsToJump = false;
		timeOfJump = Time.time;
	}

	private void GlideDown()
	{
		//float downwards
		rigidbody2D.AddForce(new Vector2(0f, jumpForce * 0.01f));
	}

	private void checkIsGrounded()
	{
		//Always be checking if we're grounded
		grounded = Physics2D.Raycast(transform.position - (new Vector3 (0, transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.02f, 1 << 10)  //layer mask so only jump on ground
			|| Physics2D.Raycast(transform.position - (new Vector3 (-(transform.localScale.x / 2.5f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.02f, 1 << 10)
				|| Physics2D.Raycast(transform.position - (new Vector3 ((transform.localScale.x / 2.5f), transform.localScale.y / 1.99f, 0)) , -Vector2.up, 0.02f, 1 << 10);
	}


	//Normalized so that speed is based on only the maximum speed
	//transform.position += new Vector3(((toFollow.transform.position - transform.position).normalized.x * maxSpeed * Time.deltaTime), 0, 0);
	
	//X following (movement)
	
	//If this doesn't change in a long enough time (Probably actually a very short time) 
	//stop following and try your own thing (if fay away then move in the opposite direction for some time and then keep following)
	//Otherwise if close enough do some cute stuff (spawn hearts?)
	private void FollowPoint()
	{
		transform.position += new Vector3(((actualFollowPoint - transform.position).normalized.x * currentSpeed * Time.deltaTime), 0, 0);
	}
	private void RunFromPoint()
	{
		transform.position -= new Vector3(((actualFollowPoint - transform.position).normalized.x * currentSpeed * Time.deltaTime), 0, 0);
	}	
}
