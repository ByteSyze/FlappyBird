using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour {

	public GameManager manager;
	
	public Collider[] friendlyColliders;

	public PipeController[] pipes;

	Rigidbody birdBody;

	public float velocity;

	public bool enableMaxJumpInterval = false;
	
	public float lastJumpTime;

	public float maxJumpInterval;
	public float minVelocity; // The minimum velocity specifies when the AI must jump, regardless of any other factors.
	public float maxVelocity; // The maximum velocity specifies when the AI must NOT jump.

	public float rotationMagnitude;
	public float rotationOffset;

	public float yRotation, zRotation;

	public float flapHeight = 200f;

	private bool jump;
	private bool shoot;

	public GameObject positionVisualizer;

	public GameObject laserTemplate;
	public Transform laserShootLocation;

	// Use this for initialization
	void Start ()
	{
		birdBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
			jump = true;
		if (Input.GetKeyDown(KeyCode.Space) && manager.isBossFight)
			shoot = true;

		birdBody.isKinematic = manager.gameOver;

		velocity = birdBody.velocity.y;
	}

	void FixedUpdate()
	{
		lastJumpTime++;

		if (manager.useAI) 
		{
			foreach(PipeController p in pipes)
			{
				if(p.transform.position.x > transform.position.x)
				{
					Vector3 target = (p.slave.transform.position + p.transform.position)/2;

					positionVisualizer.transform.position = target;

					if((target.y > transform.position.y) || (birdBody.velocity.y < minVelocity))
					{
						jump = true;
					}
				}
			}
		} 

		if (jump && (lastJumpTime >= maxJumpInterval || !enableMaxJumpInterval))
		{
			if(!manager.useAI || (manager.useAI && birdBody.velocity.y < maxVelocity))
			{
				birdBody.AddForce (Vector3.up * flapHeight);

				jump = false;
				lastJumpTime = 0;
			}
		}

		if (shoot)
		{
			GameObject laser = Instantiate(laserTemplate);

			laser.transform.rotation = laserShootLocation.rotation;
			laser.transform.position = laserShootLocation.position;

			laser.GetComponent<Laser>().destroyable = true;

			shoot = false;
		}

		transform.eulerAngles = new Vector3 (birdBody.velocity.y * rotationMagnitude + rotationOffset, yRotation, zRotation);
	}

	public void OnCollisionEnter(Collision other)
	{
		bool isGoodGuy = false;

		for (int i = 0; i < friendlyColliders.Length; i++)
		{
			if(other.collider == friendlyColliders[i])
			{
				isGoodGuy = true;
				break;
			}
		}

		if (!isGoodGuy)
		{
			manager.gameOver = true;
		}
	}
}
