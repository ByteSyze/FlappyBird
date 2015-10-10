using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
	public static int TotalLasers = 0;
	public static float Lifespan = 5f;

	public bool destroyable = false;

	public int id;

	public float creationTime;

	// Use this for initialization
	void Start () 
	{
		id = TotalLasers++;
		creationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time - creationTime > Lifespan && destroyable)
			Destroy(gameObject);
	}

	void FixedUpdate()
	{
		if(id != 0)
			transform.position -= transform.right * .5f;
	}


}
