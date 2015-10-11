using UnityEngine;
using System.Collections;

public class EagleController : BirdController
{
	public Vector3 eagleTarget;
	
	public float eagleTargetOffset = 2f; //Vertical offset

	public void Start()
	{
		base.Start();

		laserDirection = GameManager.Left;
	}

	public void FixedUpdate()
	{
		if(health > 0)
		{
			lastShootTime++;
			
			eagleTarget.y = Mathf.Sin(Time.time)*2f + eagleTargetOffset;

			if (transform.position.y < eagleTarget.y && (lastFlapTime >= maxFlapInterval))
			{
				if(birdBody.velocity.y < maxVelocity)
				{
					flap = true;
				}
			}

			Laser laser = GetMostDangerousLaser(GetDangerousLasers());

			if(laser != null)
			{
				RaycastHit hit;

				if(Physics.Raycast(laser.transform.position, -laser.transform.right, out hit))
				{
					if((hit.point - transform.position).y > 0)
					{
						if(velocity > 1f)
						{
							flap = true;
						}
						else
						{
							flap = false;
						}
					}
					else
					{
						flap = true;
					}
				}
			}

			if(manager.bossInitStage == GameManager.StageFinish)
			{
				RaycastHit enemyHit;

				if(Physics.Raycast(transform.position, transform.right, out enemyHit))
				{
					if(enemyHit.rigidbody == enemy.birdBody)
					{
						if(lastShootTime >= maxShootInterval + Random.Range(-shootIntervalVariance, shootIntervalVariance))
							shoot = true;
					}
				}
			}

			if(flap)
				Flap ();

			if(shoot)
				Shoot ();
		}
		HandleRotation();
	}

	public void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.name.Contains("Laser"))
		{
			//You sunk my battleship!
			
			GameObject explosion = (GameObject)Instantiate(manager.explosionTemplate,other.contacts[0].point, manager.explosionTemplate.transform.rotation);
			explosion.AddComponent(typeof(ExplosionController));
			Destroy(other.gameObject);

			health -= 10f;
		}
	}
}
