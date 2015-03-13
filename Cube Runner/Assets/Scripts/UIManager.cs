using UnityEngine;
using System.Collections;

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
}
