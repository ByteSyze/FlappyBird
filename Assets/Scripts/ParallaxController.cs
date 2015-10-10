using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	public GameManager manager;

	public bool parallaxEnabled;

	public float speed = -.01f;

	// Points on the X axis to indicate where the movement ends/starts.
	public float end = -6.3f;
	public float start = 6.3f;

	public Vector3 vect;

	// Use this for initialization
	void Start () {
		parallaxEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FixedUpdate()
	{
		if (!manager.gameOver && parallaxEnabled)
		{
			vect = transform.position;
			if (vect.x < end) {
				vect.x = start;
				transform.position = vect;
			} else {
				vect.x += speed;
				transform.position = vect;
			}
		}
	}
}
