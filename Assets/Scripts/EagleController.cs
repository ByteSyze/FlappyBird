using UnityEngine;
using System.Collections;

public class EagleController : BirdController
{
	public Vector3 eagleTarget;
	
	public float eagleTargetOffset = 2f; //Vertical offset
	public float eaglePathMultiplier = 2f;

	public void Start()
	{
		base.Start();

		laserDirection = GameManager.Left;
	}

	public void FixedUpdate()
	{
		if(!manager.gameOver)
		{
			if(health > 0)
			{
				lastShootTime++;
				
				eagleTarget.y = Mathf.Sin(Time.time)*eaglePathMultiplier + eagleTargetOffset;

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

				if(manager.bossStage == GameManager.StageInitFinish)
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
			else
			{
				if(Time.time - timeOfDeath > 2f) //Blow up the eagle after 2 seconds.
				{
					GameObject explosion = (GameObject)Instantiate(manager.explosionTemplate, transform.position, manager.explosionTemplate.transform.rotation);
					explosion.AddComponent(typeof(ExplosionController));

					manager.isBossFight = false;
					manager.bossStage = GameManager.StageDisplayWin;

					Destroy(healthBar.gameObject);
					Destroy(gameObject);
				}
			}
			HandleRotation();
		}
	}

	public void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.name.Contains("Laser"))
		{
			//You sunk my battleship!
			OnLaserHit(other);
		}
	}
}
