using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
	public static int TotalLasers = 0;
	public static float Lifespan = 5f;

	public int direction = GameManager.Right;

	public bool destroyable = false;
	public bool verbose = false;
	public BirdController source;

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
		transform.position += (transform.right * .5f) * direction;
	}

	public bool IsDangerousTo(Rigidbody rb)
	{
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, transform.right * direction, out hit))
		{
			return hit.rigidbody == rb;
		}

		return false;
	}
	
}
