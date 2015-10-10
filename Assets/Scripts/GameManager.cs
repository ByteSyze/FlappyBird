using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static float HighScore = 0;

	public float bossTriggerScore = 300f;
	public bool isBossFight;

	public float score = 0;
	public float difficultyMultiplier = .05f; // Used in pipe controller to change how quickly the game difficulty increases.

	public Text gameOverText, highScoreText, scoreText;

	public bool gameOver;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);

		isBossFight = false;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (gameOver)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Application.LoadLevel(Application.loadedLevel);
				gameOver = false;
			}
		}

	}

	void FixedUpdate()
	{

		if (gameOver)
		{
			gameOverText.text = "GAME OVER";

			if(score > HighScore)
				HighScore = score;

			highScoreText.text = "High Score: " + Mathf.RoundToInt(HighScore);
		}
		else
		{
			if(isBossFight)
			{

			}
			else
			{
				if(score >= bossTriggerScore)
					isBossFight = true;

				score += .05f;
				scoreText.text = "" + Mathf.RoundToInt(score);
			}
		}
	}

	public void OnLevelWasLoaded()
	{
		if (gameOverText == null)
			Destroy (gameObject);
	}
}
