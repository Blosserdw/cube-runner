using UnityEngine;
using System.Collections;

public enum ScreenSwitch
{
	Title,
	Credits,
	Game
}

public class UIManager : MonoBehaviour {

	private static UIManager instance = null;
	public static UIManager Instance
	{
		get { return instance; }
	}
	
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		LoadMainMenu();

		// Grab the rotating cube
		if (GameObject.Find("RotatingCube") != null)
		{
			MainMenuCube = GameObject.Find("RotatingCube");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public GameObject MainMenuPrefab = null;
	public GameObject MainMenuObject = null;
	public MainMenu MainMenuScript = null;
	public GameObject MainMenuCube = null;
	public ScreenSwitch switchTo = ScreenSwitch.Credits;
	
	public void LoadMainMenu()
	{
		// Load the main menu UI
		MainMenuObject = GameObject.Instantiate(MainMenuPrefab) as GameObject;
		MainMenuScript = (MainMenu)MainMenuObject.GetComponent<MainMenu>() as MainMenu;


	}
	
	public void DestroyMainMenu()
	{
		if (MainMenuObject != null)
		{
			if (MainMenuCube != null)
			{
				Destroy(MainMenuCube);
				MainMenuCube = null;
			}

			Destroy (MainMenuObject);
			MainMenuObject = null;
			MainMenuScript = null;
		}
	}

	public void SwitchMenu()
	{
		if (switchTo == ScreenSwitch.Credits)
		{
			if (MainMenuScript != null)
			{
				MainMenuScript.ChangeMenuToCredits();
			}
		}
		else if (switchTo == ScreenSwitch.Title)
		{
			if (MainMenuScript != null)
			{
				MainMenuScript.ChangeMenuToTitle();
			}
		}
		else
		{
			if (MainMenuScript != null)
			{
				UIManager.Instance.DestroyMainMenu();
				UIManager.Instance.LoadHUD();
				GameManager.Instance.StartLevel();
			}
		}
	}

	public GameObject HUDPrefab = null;
	public GameObject HUDObject = null;
	public UIHUD_Events HUDScript = null;
	
	public void LoadHUD()
	{
		// Load the main menu UI
		HUDObject = GameObject.Instantiate(HUDPrefab) as GameObject;
		HUDScript = (UIHUD_Events)HUDObject.GetComponent<UIHUD_Events>() as UIHUD_Events;
	}

	public void DestroyHUD()
	{
		if (HUDObject != null)
		{
			Destroy (HUDObject);
			HUDObject = null;
			HUDScript = null;
		}
	}

	public void AddToScore(int numToAdd)
	{
		if (HUDScript != null)
		{
			HUDScript.AddThisToScore(numToAdd);
		}
	}

	public void CheckScore(int scoreToCheck)
	{
		if (scoreToCheck >= GameManager.Instance.pickupsPerSide * 6)
		{
			Debug.Log("SCORE IS AT MAX");
			// Level over, start transitioning to next with faster rates
			GameManager.Instance.PlayLevelWinSound();
			GameManager.Instance.NextLevel();
			GameManager.Instance.playerObject.GetComponent<PlayerMovement>().stopMovement = true;
		}
		else
		{
			//Debug.Log("Checking score... but nothing out of the ordinary... move along...");
		}
	}

	public void ShowGameOverScreen()
	{
		if (HUDScript != null)
		{
			HUDScript.tryAgainScreen.SetActive(true);
		}
	}

	public void StartGameOverSequence()
	{
		if (HUDScript != null)
		{
			GameManager.Instance.playerObject.GetComponent<Rigidbody>().useGravity = false;

			Hashtable moveHash = new Hashtable();

			moveHash.Add("position", GameManager.Instance.masterCube.transform.localPosition);
			moveHash.Add("time", 10.0f);
			moveHash.Add("isLocal", true);

			Debug.Log("ASSIMILATING!!!");
			GameManager.Instance.PlayGameOverSound();
			iTween.MoveTo(GameManager.Instance.playerObject, moveHash);
		}

		Invoke("ShowGameOverScreen", 1.0f);
	}

	public GameObject LevelNumberPrefab = null;
	public GameObject LevelNumberObject = null;
	public UILevel LevelNumberScript = null;
	
	public void LoadLevelNumber()
	{
		// Load the main menu UI
		LevelNumberObject = GameObject.Instantiate(LevelNumberPrefab) as GameObject;
		LevelNumberScript = (UILevel)LevelNumberObject.GetComponent<UILevel>() as UILevel;
	}
	
	public void DestroyLevelNumber()
	{
		if (LevelNumberObject != null)
		{
			Destroy (LevelNumberObject);
			LevelNumberObject = null;
			LevelNumberScript = null;
		}
	}
}
