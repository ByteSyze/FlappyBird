using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour
{
	public float maxHealth = 100f;
	public float health;
	
	public HealthBar healthBar;

	public BirdController enemy;

	public GameManager manager;

	public bool useAI;
	
	public Collider[] friendlyColliders;

	public PipeController[] pipes;

	public Rigidbody birdBody;

	public float velocity;

	public bool enableMaxFlapInterval = false;
	
	public float lastFlapTime;

	public float maxFlapInterval;
	public float minVelocity; // The minimum velocity specifies when the AI must flap, regardless of any other factors.
	public float maxVelocity; // The maximum velocity specifies when the AI must NOT flap.
	
	public float maxShootInterval = 2f;
	public float shootIntervalVariance = 1f;
	public float lastShootTime;

	public float rotationMagnitude;
	public float rotationOffset;

	public float yRotation, zRotation;

	public float flapHeight = 200f;

	public bool flap;
	public bool shoot;

	public GameObject positionVisualizer;

	public GameObject laserTemplate;
	public Transform laserShootLocation;

	public int laserDirection = GameManager.Right;

	public AudioSource flapSource;

	// Use this for initialization
	public void Start ()
	{
		birdBody = GetComponent<Rigidbody> ();
		
		health = maxHealth;
	}
	
	// Update is called once per frame
	public void Update ()
	{
		if(health <= 0)
			manager.gameOver = true;

		if(!useAI)
		{
			if (Input.GetMouseButtonDown (0))
				flap = true;
			if (Input.GetKeyDown(KeyCode.Space) && manager.isBossFight)
				shoot = true;
		}

		birdBody.isKinematic = manager.gameOver;

		velocity = birdBody.velocity.y;
	}

	void FixedUpdate()
	{
		lastFlapTime++;
		lastShootTime++;

		if (useAI) 
		{
			foreach(PipeController p in pipes)
			{
				if(p.transform.position.x > transform.position.x)
				{
					Vector3 target = (p.slave.transform.position + p.transform.position)/2;

					positionVisualizer.transform.position = target;

					if((target.y > transform.position.y) || (birdBody.velocity.y < minVelocity))
					{
						flap = true;
					}
				}
			}
		} 

		if (flap && (lastFlapTime >= maxFlapInterval || !enableMaxFlapInterval))
		{
			if(!useAI || (useAI && birdBody.velocity.y < maxVelocity))
			{
				Flap();
			}
		}

		if (shoot)
		{
			Shoot();
		}

		HandleRotation();
		//GetDangerousLasers();
	}

	public void Flap()
	{
		birdBody.AddForce (Vector3.up * flapHeight);

		flapSource.Play();
		
		flap = false;
		lastFlapTime = 0;
	}

	public void Shoot()
	{
		GameObject laserObj = Instantiate(laserTemplate);
		
		laserObj.transform.rotation = laserShootLocation.rotation;
		laserObj.transform.position = laserShootLocation.position;

		Laser laser = laserObj.GetComponent<Laser>();

		laser.destroyable = true;
		laser.source = this;
		laser.direction = laserDirection;
		//laser.verbose = true;
		
		shoot = false;
		lastShootTime = 0;
	}

	/**
	 *	Returns a list of lasers that might actually hit this bird.
	 *
	 *	The list only returns lasers that are noticeably dangerous,
	 *	i.e. there is no foresight.
	 **/
	public ArrayList GetDangerousLasers()
	{
		Laser[] lasers = GameObject.FindObjectsOfType<Laser>();
		ArrayList dangerousLasers = new ArrayList();

		foreach(Laser laser in lasers)
		{
			//Check that the laser wasn't shot by us.
			if(laser.source != this)
			{
				if(laser.IsDangerousTo(birdBody))
				{
					dangerousLasers.Add(laser);
				}
			}
		}

		return dangerousLasers;
	}

	public Laser GetMostDangerousLaser(ArrayList lasers)
	{
		
		ArrayList dangerousLasers = GetDangerousLasers();
		
		float smallestDifference = Mathf.Infinity;
		Laser mostDangerousLaser = null;
		
		foreach(Laser laser in dangerousLasers)
		{
			//Avoid the lasers!
			Vector3 difference = laser.transform.position - transform.position;
			
			if(difference.sqrMagnitude < smallestDifference)
			{
				smallestDifference = difference.sqrMagnitude;
				mostDangerousLaser = laser;
			}
		}

		return mostDangerousLaser;
	}

	public void HandleRotation()
	{
		transform.eulerAngles = new Vector3 (birdBody.velocity.y * rotationMagnitude + rotationOffset, yRotation, zRotation);
	}

	public void OnLaserHit(Collision laser)
	{
		GameObject explosion = (GameObject)Instantiate(manager.explosionTemplate, laser.contacts[0].point, manager.explosionTemplate.transform.rotation);
		explosion.AddComponent(typeof(ExplosionController));
		Destroy(laser.gameObject);
		
		health -= 10f;
	}

	public void OnCollisionEnter(Collision other)
	{
		if(other.collider.name.Contains("Laser"))
		{
			OnLaserHit(other);
		}
		else
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
}
