using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int numRoundsToWin = 5;        
    public float startDelay = 3f;         
    public float endDelay = 3f;           
    public CameraControl cameraControl;   
    public Text messageText;              
    public GameObject tankPrefab;  
    public TankManager[] tanks; 
	public HealthSpawnManager healthSpawn;
	public GameObject rocketPrefab;
	public GameObject explosionPrefab;



    private int roundNumber;              
    private WaitForSeconds startWait;     
    private WaitForSeconds endWait;       
    private TankManager roundWinner;
    private TankManager gameWinner; 

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].instance =
                Instantiate(tankPrefab, tanks[i].spawnPoint.position, tanks[i].spawnPoint.rotation) as GameObject;
            tanks[i].playerNumber = i + 1;
            tanks[i].Setup();
        }
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = tanks[i].instance.transform;
        }

        cameraControl.targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
		// As soon as the round starts reset the tanks and make sure they can't move.
		ResetAllTanks ();
		DisableTankControl ();

		//Reset Health & Spawn New Health
		healthSpawn.removeAllHealth();
		healthSpawn.Spawn ();

		//TODO spawn new rocket powerup
		Instantiate(rocketPrefab, new Vector3(0, 0, 0),  tankPrefab.transform.rotation).transform.Rotate(new Vector3(-90f, 0f, 10f));

		// Snap the camera's zoom and position to something appropriate for the reset tanks.
		cameraControl.SetStartPositionAndSize ();

		// Increment the round number and display text showing the players what round it is.
		roundNumber++;
		messageText.text = "ROUND " + roundNumber;

		// Wait for the specified length of time until yielding control back to the game loop.
		yield return startWait;
    }


    private IEnumerator RoundPlaying()
    {
		// As soon as the round begins playing let the players control the tanks.
		EnableTankControl ();

		// Clear the text from the screen.
		messageText.text = string.Empty;

		// While there is not one tank left...
		while (!OneTankLeft())
		{
			
			//Tank Collected Missle
			if(tanks[0].getTankHealth().collectedPowerup) 
			{
				GameObject explosion = Instantiate (explosionPrefab, tanks[1].instance.gameObject.transform.position, tanks[1].instance.gameObject.transform.rotation);
				tanks [0].getTankHealth ().collectedPowerup = false;
				Destroy (explosion, 3);
			}
			if(tanks[1].getTankHealth().collectedPowerup) 
			{
				GameObject explosion = Instantiate (explosionPrefab, tanks[0].instance.gameObject.transform.position, tanks[0].instance.gameObject.transform.rotation);
				tanks[1].getTankHealth ().collectedPowerup = false;
				Destroy (explosion, 3);

			}
		
			yield return null;
		}
    }


    private IEnumerator RoundEnding()
    {
		// Stop tanks from moving.
		DisableTankControl ();

		// Clear the winner from the previous round.
		roundWinner = null;

		// See if there is a winner now the round is over.
		roundWinner = GetRoundWinner ();

		// If there is a winner, increment their score.
		if (roundWinner != null)
			roundWinner.wins++;

		// Now the winner's score has been incremented, see if someone has one the game.
		gameWinner = GetGameWinner ();

		// Get a message based on the scores and whether or not there is a game winner and display it.
		string message = EndMessage ();
		messageText.text = message;

		// Wait for the specified length of time until yielding control back to the game loop.
		yield return endWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                return tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].wins == numRoundsToWin)
                return tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (roundWinner != null)
            message = roundWinner.coloredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < tanks.Length; i++)
        {
            message += tanks[i].coloredPlayerText + ": " + tanks[i].wins + " WINS\n";
        }

        if (gameWinner != null)
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableControl();
        }
    }
}