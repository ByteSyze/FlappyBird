using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public BirdController owner;

	public GameManager manager;

	public RectTransform rTransform;
	public RectTransform healthRect;

	private Vector3 show,hide;

	public bool display = false;

	void Start()
	{
		show = rTransform.localScale;

		hide = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(display)
			rTransform.localScale = show;
		else
			rTransform.localScale = hide;

		healthRect.localScale = new Vector3(owner.health/owner.maxHealth, 1, 1);

		Vector2 pos = owner.transform.position;  // get the game object position
		Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
		
		// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
		rTransform.anchorMin = viewportPoint;  
		rTransform.anchorMax = viewportPoint; 	
	}
}
