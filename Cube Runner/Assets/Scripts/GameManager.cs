using UnityEngine;
using System.Collections;

public enum CubePattern
{
	Single,
	Double,
	Triple,
	FourSquare
}

public enum TransferDirection
{
	Front,
	Back,
	Left,
	Right
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
		checkMoveFinished = false;
	}
	
	// Movement variables
	public bool checkMoveFinished = false;
	public float moveTime = 0.5f;
	public float tileStep = 1.0f;
	public GameObject selectedTile = null;

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
	public GameObject masterCube = null;
	public GameObject mapTopContainer = null;
	public GameObject mapBottomContainer = null;
	public GameObject mapLeftContainer = null;
	public GameObject mapRightContainer = null;
	public GameObject mapFrontContainer = null;
	public GameObject mapBackContainer = null;

	// maps
	public GameObject frontTransferMapTop = null;

	private Vector3 playerStartPosition = new Vector3(-4.5f, 6.0f, -0.5f);
	
	public void SetUpPlayer(Map thisMap)
	{
		if (thisMap == mapTopContainer.GetComponentInChildren<Map>())
		{
			GameObject newPlayer = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			newPlayer.transform.position = playerStartPosition;

			playerObject = newPlayer;
			currentMap = thisMap;

			currentMap.ActivateMap(2.0f);
		}
	}

	public void ProcessTransfer(TransferDirection thisDirection)
	{
		if (thisDirection != null)
		{
			Hashtable rotateHash = new Hashtable();

			if (thisDirection == TransferDirection.Front)
			{
				GameObject tempMapHolder = mapTopContainer;
				mapTopContainer = mapFrontContainer;
				mapFrontContainer = mapBottomContainer;
				mapBottomContainer = mapBackContainer;
				mapBackContainer = tempMapHolder;

				// other two sides stay the same
				//rotateHash.Add("rotation", new Vector3(masterCube.transform.rotation.x, masterCube.transform.rotation.y, masterCube.transform.rotation.z + 90.0f));
				rotateHash.Add("z", 0.25f);
			}
			else if (thisDirection == TransferDirection.Back)
			{
				GameObject tempMapHolder = mapTopContainer;
				mapTopContainer = mapBackContainer;
				mapBackContainer = mapBottomContainer;
				mapBottomContainer = mapFrontContainer;
				mapFrontContainer = tempMapHolder;
				
				// other two sides stay the same
				//rotateHash.Add("rotation", new Vector3(masterCube.transform.rotation.x, masterCube.transform.rotation.y, masterCube.transform.rotation.z - 90.0f));
				rotateHash.Add("z", -0.25f);
			}
			else if (thisDirection == TransferDirection.Left)
			{
				GameObject tempMapHolder = mapTopContainer;
				mapTopContainer = mapLeftContainer;
				mapLeftContainer = mapBottomContainer;
				mapBottomContainer = mapRightContainer;
				mapRightContainer = tempMapHolder;
				
				// other two sides stay the same
				//rotateHash.Add("rotation", new Vector3(masterCube.transform.rotation.x - 90.0f, masterCube.transform.rotation.y, masterCube.transform.rotation.z));
				rotateHash.Add("x", -0.25f);
			}
			else if (thisDirection == TransferDirection.Right)
			{
				GameObject tempMapHolder = mapTopContainer;
				mapTopContainer = mapRightContainer;
				mapRightContainer = mapBottomContainer;
				mapBottomContainer = mapLeftContainer;
				mapLeftContainer = tempMapHolder;
				
				// other two sides stay the same
				//rotateHash.Add("rotation", new Vector3(masterCube.transform.rotation.x + 90.0f, masterCube.transform.rotation.y, masterCube.transform.rotation.z));
				rotateHash.Add("x", 0.25f);
			}
			else
			{
				Debug.LogError("Did not get a proper Direction for transfer!");
			}

			rotateHash.Add("time", 1.0f);
			rotateHash.Add("isLocal", false);
			//rotateHash.Add("easetype", "linear");
			rotateHash.Add("oncomplete", "FinishedRotating");
			rotateHash.Add("oncompletetarget", gameObject);

			iTween.RotateBy(masterCube, rotateHash);

			currentMap = mapTopContainer.GetComponentInChildren<Map>();
			//currentMap.ActivateMap();
		}
	}

	public void FinishedRotating()
	{
		Debug.Log("ROTATING DONE!!");

		Hashtable moveHash = new Hashtable();
		moveHash.Add("position", playerStartPosition);
		moveHash.Add("time", 1.0f);
		moveHash.Add("oncomplete", "FinishedPlacingPlayer");
		moveHash.Add("oncompletetarget", gameObject);

		iTween.MoveTo(playerObject, moveHash);
	}

	public void FinishedPlacingPlayer()
	{
		Debug.Log("PLAYER MOVING DONE!!");
		playerObject.GetComponent<PlayerMovement>().RestartPlayer();
	}
}
