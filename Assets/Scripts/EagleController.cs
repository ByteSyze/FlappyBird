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

		if(flap)
			Flap ();

		if(shoot)
			Shoot ();

		HandleRotation();
	}

	public void OnCollisionEnter(Collision other)
	{

	}
}
