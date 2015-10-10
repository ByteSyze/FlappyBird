using UnityEngine;
using System.Collections;

public class PipeController : ParallaxController
{
	public float min,max; //Minimum and maximum variation in the pipes' Y axis.

	public GameObject slave; // "Slave" refers to the pipe that is a child element of the controlling (or "master") pipe.

	void FixedUpdate()
	{
		base.FixedUpdate ();

		if (vect.x == start)
		{

			if(manager.isBossFight && !manager.useAI)
			{
				Destroy(gameObject);
			}
			else
			{
				//The pipe has been reset. change its vertical position and increase difficulty.
				vect.y = Random.Range(min,max);
				transform.position = vect;

				slave.transform.Translate(slave.transform.up * -(manager.score * manager.difficultyMultiplier));
			}
		}
	}
}
