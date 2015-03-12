using UnityEngine;
using System.Collections;

public enum CubePattern
{
	Single,
	Double,
	Triple,
	FourSquare
}

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

		if (Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("GETTING INPUT");
			mapLeftContainer.transform.position = Vector3.zero;
			mapLeftContainer.transform.rotation = Quaternion.identity;
		}
	}
	
	public GameObject playerObject = null;
	public GameObject playerPrefab = null;
	public Map currentMap = null;
	public GameObject mapTopContainer = null;
	public GameObject mapBottomContainer = null;
	public GameObject mapLeftContainer = null;
	public GameObject mapRightContainer = null;
	public GameObject mapFrontContainer = null;
	public GameObject mapBackContainer = null;
	public Vector3 playerStartPosition = new Vector3(0.0f, 0.0f, 4.0f);
	
	public void SetUpPlayer(Map thisMap)
	{
		if (thisMap == mapTopContainer.GetComponentInChildren<Map>())
		{
			GameObject newPlayer = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			newPlayer.transform.position = thisMap.Tiles[0,0].transform.position + playerStartPosition;

			playerObject = newPlayer;
			currentMap = thisMap;
		}
	}
}
