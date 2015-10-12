using UnityEngine;
using System.Collections;

public class PipeController : ParallaxController
{
	public float min,max; //Minimum and maximum variation in the pipes' Y axis.

	public GameObject slave; // "Slave" refers to the pipe that is a child element of the controlling (or "master") pipe.

	private Vector3 hidePos = Vector3.up * 100f;

	public bool hidden = false;

	private bool hide = false;

	void FixedUpdate()
	{
		base.FixedUpdate ();

		if (vect.x == start)
		{

			if(hide && !manager.bird.useAI)
			{
				transform.position += hidePos;
				hidden = true;
			}
			else
			{
				hidden = false;

				//The pipe has been reset. change its vertical position and increase difficulty.
				vect.y = Random.Range(min,max);
				transform.position = vect;

				slave.transform.Translate(slave.transform.up * -(manager.difficultyMultiplier));
			}
		}
	}

	public void Hide()
	{
		hide = true;
	}

	public void Show()
	{
		hide = false;
	}
}
