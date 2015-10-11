using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public const int StageDisplayText 	= 0;
	public const int StageDestroyPipes 	= 1;
	public const int StageMoveBird		= 2;
	public const int StageDispatchBoss 	= 3;
	public const int StageStartMusic 	= 4;
	public const int StageFinish		= 5;
	
	public const int Left = 1;
	public const int Right = -1;

	public static float HighScore = 0;

	public float bossTriggerScore = 300f;
	public bool isBossFight;

	public int bossInitStage;

	public BirdController bird;
	public BirdController eagle;

	public Vector3 bossBattleBirdPosition; //The position of the bird once a boss battle has begun.
	public Vector3 bossBattleEaglePosition; //The position of the eagle once a boss battle has begun.

	public float score = 0;
	public float difficultyMultiplier = .05f; // Used in pipe controller to change how quickly the game difficulty increases.

	public Text gameOverText, highScoreText, scoreText, bossFightText;

	public bool gameOver;

	public GameObject explosionTemplate;

	public AudioSource bossMusic;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);
		
		isBossFight = false;
		gameOver = false;

		bossInitStage = StageStartMusic;

		bossBattleBirdPosition = bird.transform.position;
		bossBattleBirdPosition.x -= 2f;

		bossBattleEaglePosition = eagle.transform.position;
		bossBattleEaglePosition.x = 1.7f;
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
				if(bossInitStage == StageStartMusic)
				{
					bossMusic.Play();
					bossInitStage = StageDestroyPipes;
				}
				if(bossInitStage == StageDisplayText)
				{
					bossFightText.text = "BOSS FIGHT! Hit SPACE to fire!";
					bird.healthBar.display = true;
					eagle.healthBar.display = true;

					bossInitStage = StageMoveBird;
				}
				else if(bossInitStage == StageDestroyPipes)
				{
					if(GameObject.FindObjectsOfType<PipeController>().Length == 0)
					{
						bossInitStage = StageDisplayText;
					}
				}
				else if(bossInitStage == StageMoveBird)
				{
					if(bird.transform.position.x != bossBattleBirdPosition.x)
					{
						Vector3 newPos = Vector3.MoveTowards(bird.transform.position, bossBattleBirdPosition, .05f);
						bird.transform.position = newPos;
					}
					else
					{
						bossInitStage = StageDispatchBoss;
					}
				}
				else if(bossInitStage == StageDispatchBoss)
				{
					if(eagle.transform.position.x != bossBattleEaglePosition.x)
					{
						Vector3 newPos = Vector3.MoveTowards(eagle.transform.position, bossBattleEaglePosition, .05f);
						eagle.transform.position = newPos;
					}
					else
					{
						bossInitStage = StageFinish;
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
