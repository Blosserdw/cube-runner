using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager instance = null;
	public static GameManager Instance
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
		//gameObject.AddComponent<UIManager>(); // Set up the UIManager

		state_Move = true;
		checkMoveFinished = false;
	}
	
	// Movement variables
	public bool checkMoveFinished = false;
	public float moveTime = 0.5f;
	public float tileStep = 1.0f;
	public GameObject selectedTile = null;

	// Game States
	public bool state_Move = false;
	public bool state_Attack = false;

	// Update is called once per frame
	void Update ()
	{
		if (playerObject != null)
		{
			
		}
	}
	
	public GameObject playerObject = null;
	public Map currentMap = null;
	//public GameObject aStarObject = null;
	
	public void SetUpPlayer(GameObject thisPlayerObject, Map thisMap)
	{
		playerObject = thisPlayerObject;
		currentMap = thisMap;
	}
}
