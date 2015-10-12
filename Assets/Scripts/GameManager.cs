using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	//Boss start stages
	public const int StageDisplayText 	= 0;
	public const int StageHidePipes 	= 1;
	public const int StageMoveBird		= 2;
	public const int StageDispatchBoss 	= 3;
	public const int StageStartMusic 	= 4;
	public const int StageInitFinish	= 5;

	//Boss end stages
	public const int StageDisplayWin	= 6;
	public const int StageStopMusic		= 7;
	public const int StageMoveBirdBack 	= 8;
	public const int StageShowPipes		= 9;
	public const int StageHideText		= 10;
	public const int StageDefeatFinish 	= 11;
	
	public const int Left = 1;
	public const int Right = -1;

	public static float HighScore = 0;

	public float bossTriggerScore = 300f;
	public bool isBossFight;

	public int bossStage;

	public BirdController bird;
	public BirdController eagle;

	public Vector3 normalBirdPosition, bossBattleBirdPosition; //The position of the bird once a boss battle has begun.
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

		bossStage = StageStartMusic;

		bossBattleBirdPosition = bird.transform.position;
		bossBattleBirdPosition.x -= 2f;

		bossBattleEaglePosition = eagle.transform.position;
		bossBattleEaglePosition.x = 1.7f;

		normalBirdPosition = bird.transform.position;
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
				if(bossStage == StageStartMusic)
				{
					bossMusic.Play();
					bossStage = StageHidePipes;
				}
				if(bossStage == StageDisplayText)
				{
					bossFightText.text = "BOSS FIGHT! Hit SPACE to fire!";
					bird.healthBar.display = true;
					eagle.healthBar.display = true;

					bossStage = StageMoveBird;
				}
				else if(bossStage == StageHidePipes)
				{
					bool pipesHidden = true;
					foreach(PipeController pipe in GameObject.FindObjectsOfType<PipeController>())
					{
						if(!pipe.hidden)
						{
							pipe.Hide ();
							pipesHidden = false;
							break;
						}
					}
					if(pipesHidden)
					{
						bossStage = StageDisplayText;
					}
				}
				else if(bossStage == StageMoveBird)
				{
					if(bird.transform.position.x != bossBattleBirdPosition.x)
					{
						Vector3 newPos = Vector3.MoveTowards(bird.transform.position, bossBattleBirdPosition, .05f);
						bird.transform.position = newPos;
					}
					else
					{
						bossStage = StageDispatchBoss;
					}
				}
				else if(bossStage == StageDispatchBoss)
				{
					if(eagle.transform.position.x != bossBattleEaglePosition.x)
					{
						Vector3 newPos = Vector3.MoveTowards(eagle.transform.position, bossBattleEaglePosition, .05f);
						eagle.transform.position = newPos;
					}
					else
					{
						bossStage = StageInitFinish;
					}
				}
			}
			else
			{
				if(score >= bossTriggerScore && bossStage == StageStartMusic)
					isBossFight = true;

				if(bossStage == StageDisplayWin)
				{
					bossFightText.text = "BOSS DEFEATED! You may return to your pointless life of inevitable doom!";

					bossStage = StageStopMusic;
				}
				else if(bossStage == StageStopMusic)
				{
					if(bossMusic.isPlaying)
					{
						//Fade the music out.
						if(bossMusic.volume > .05f)
							bossMusic.volume -= .002f;
						else
						{
							bossMusic.Stop();
							bossStage = StageMoveBirdBack;
						}
					}
				}
				else if(bossStage == StageMoveBirdBack)
				{
					bird.healthBar.display = false;

					if(bird.transform.position.x != normalBirdPosition.x)
					{
						Vector3 newPos = Vector3.MoveTowards(bird.transform.position, normalBirdPosition, .05f);
						bird.transform.position = newPos;
					}
					else
					{
						bossStage = StageShowPipes;
					}
				}
				else if(bossStage == StageShowPipes)
				{
					bool pipesShown = true;
					foreach(PipeController pipe in GameObject.FindObjectsOfType<PipeController>())
					{
						if(pipe.hidden)
						{
							pipe.Show ();
							pipesShown = false;
							break;
						}
					}
					if(pipesShown)
					{
						bossStage = StageHideText;
					}
				}
				else if(bossStage == StageHideText)
				{
					bossFightText.text = "";
					bossStage = StageDefeatFinish;
				}
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
