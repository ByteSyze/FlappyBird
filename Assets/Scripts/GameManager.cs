using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	private const int StageDisplayText 	= 0;
	private const int StageDestroyPipes = 1;
	private const int StageMoveBird		= 2;
	private const int StageDispatchBoss = 3;
	
	public bool useAI = false;

	public static float HighScore = 0;

	public float bossTriggerScore = 300f;
	public bool isBossFight;

	private int bossInitStage;

	public BirdController bird;

	public Vector3 bossBirdPosition; //The position of the bird once a boss battle has begun.

	public float score = 0;
	public float difficultyMultiplier = .05f; // Used in pipe controller to change how quickly the game difficulty increases.

	public Text gameOverText, highScoreText, scoreText, bossFightText;

	public bool gameOver;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);
		
		isBossFight = false;
		gameOver = false;

		bossInitStage = 0;

		bossBirdPosition = bird.transform.position;
		bossBirdPosition.x -= 2f;
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
				if(bossInitStage == StageDisplayText)
				{
					bossFightText.text = "BOSS FIGHT! Hit SPACE to fire!";
					bossInitStage = StageDestroyPipes;
				}
				else if(bossInitStage == StageDestroyPipes)
				{
					if(GameObject.FindObjectsOfType<PipeController>().Length == 0)
					{
						bossInitStage = StageMoveBird;
					}
				}
				else if(bossInitStage == StageMoveBird)
				{
					if(bird.transform.position.x != bossBirdPosition.x)
					{
						Vector3 newPos = Vector3.MoveTowards(bird.transform.position, bossBirdPosition, .05f);
						print (newPos);
						bird.transform.position = newPos;
					}
					else
					{
						bossInitStage = StageDispatchBoss;
					}
				}
			}
			else
			{
				if(score >= bossTriggerScore)
					isBossFight = true;
			}

			score += .05f;
			scoreText.text = "" + Mathf.RoundToInt(score);
		}
	}

	public void OnLevelWasLoaded()
	{
		if (gameOverText == null)
			Destroy (gameObject);
	}
}
