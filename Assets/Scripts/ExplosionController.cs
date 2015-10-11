using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {

	private SpriteRenderer renderer;

	// Use this for initialization
	void Start ()
	{
		renderer = GetComponent<SpriteRenderer>();

		GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(renderer.sprite.name == "explosion_7")
			Destroy (gameObject);
	}
}
