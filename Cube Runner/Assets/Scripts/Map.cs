using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public GameObject myContainer = null;

	public GameObject[,] Tiles;
	public int numTilesX = 10;
	public int numTilesZ = 10;
	public GameObject tilePrefab;
	public int amountOfPickups = 3;

	// Use this for initialization
	void Start ()
	{
		Tiles = new GameObject[numTilesX, numTilesZ];
		
		GenerateMap();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void GenerateMap()
	{
		for (int i = 0; i < numTilesX; i++)
		{
			for (int j = 0; j < numTilesZ; j++)
			{
				// Set up random height
				float height = 0.0f;
//				int randomHeight = Random.Range(0, 100);
//				if (randomHeight <= 20)
//					height = 0.0f;
//				else if (randomHeight > 20 && randomHeight <= 70)
//					height = 0.2f;
//				else if (randomHeight > 70 && randomHeight <= 90)
//					height = 0.4f;
//				else
//					height = 0.6f;


				// Create Tiles
				//Debug.Log("Instantiating a tile at: " + i + ", " + j);
				GameObject newTile = GameObject.Instantiate(tilePrefab, new Vector3((float)i, height, (float)j), Quaternion.identity) as GameObject;
				
				// Set up this individual tile when placed
				newTile.transform.parent = this.gameObject.transform;
				newTile.transform.localPosition = new Vector3((float)i, 0.0f, (float)j);
				newTile.transform.localRotation = Quaternion.identity;
				newTile.layer = 8; // Ground layer for A*
				Tiles[i, j] = newTile;
				transformableTiles.Add(Tiles[i, j]);
			}
		}

		GeneratePickups(amountOfPickups);
		
		// Set the map to the right rotation (should already be set in the inspector, so 0,0,0 SHOULD work here for everything
		////Debug.Log("Setting Map: " + this.gameObject.name + " to 0,0,0");
		//this.gameObject.transform.position = Vector3.zero;
		//this.gameObject.transform.rotation = Quaternion.identity;

		// Set parent to zeros now too? huh?
		//myContainer.transform.position = Vector3.zero;
		//myContainer.transform.rotation = Quaternion.identity;
		
		// Add the player
		SetupPlayer();
	}



	public void GeneratePickups(int numOfPickupsToGenerate)
	{
		List<int> numsAlreadyChosen = new List<int>();

		for (int i = 0; i < amountOfPickups; i++)
		{
			// Pick random placement for this pickup
			int randomX = Random.Range(1, numTilesX - 1); // Adding minus 1 and starting at 1 so they're not near the edges
			int randomZ = Random.Range(1, numTilesZ - 1);

			// Set up random height
			float height = 0.0f;

			// Create Tiles
			//Debug.Log("Instantiating a tile at: " + i + ", " + j);
			GameObject newPickup = GameObject.Instantiate(GameManager.Instance.pickupPrefab, new Vector3((float)randomX, height, (float)randomZ), Quaternion.identity) as GameObject;
			
			// Set up this individual tile when placed
			newPickup.transform.parent = this.gameObject.transform;
			newPickup.transform.localPosition = new Vector3((float)randomX, 0.0f, (float)randomZ);
			newPickup.transform.localRotation = Quaternion.identity;
			newPickup.layer = 9; // Pickups layer
			Tiles[randomX, randomZ].GetComponent<Tile>().hasPickupOnIt = true;
			transformableTiles.Remove(Tiles[randomX, randomZ]);
		}
	}
	
	public void SetupPlayer()
	{
		GameManager.Instance.SetUpPlayer(this);
	}

	public void ActivateMap(float delay)
	{
		canThisMapTransform = true;
		Invoke("StartCubeTransformations", delay);
	}

	public void DeactivateMap(float delay)
	{
		canThisMapTransform = false;
	}

	public float cubeMovementTime = 2.0f;
	public float cubeTransformTime = 4.0f;
	public List<GameObject> transformableTiles = new List<GameObject>();
	private bool canThisMapTransform = false;

	public void StartCubeTransformations()
	{
		if (canThisMapTransform)
		{
			//Debug.Log("Started transformations on map: " + this.gameObject.name + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

			// Choose the cube to transform
			GameObject randomStartTile = transformableTiles[Random.Range(0, transformableTiles.Count)];
			//Debug.Log("How many transformable tiles do we have for " + this.gameObject.name + "? Well, it's: " + transformableTiles.Count);
			//Debug.Log("Random Starting tile for this group is: Tile(" + randomStartTile.transform.localPosition.x + "," + randomStartTile.transform.localPosition.z + ") on Map:" + this.gameObject.name);
			int tilesToCheck = 0;

			// Is this cube transformation going up or down?
			bool cubeGoingUp;
			if (Random.value >= 0.5f)
				cubeGoingUp = true;
			else
				cubeGoingUp = false;

			// Which cube transformation is it?
			CubePattern thisPattern = (CubePattern)Random.Range(0, (int)CubePattern.Triple + 1);
			//Debug.Log("Going to try Block Pattern: " + thisPattern.ToString());

			//Debug.Log("Setting up new tile group list...");
			List<GameObject> tileGroup = new List<GameObject>();

			// Grab all the cubes connected in the pattern
			if (thisPattern == CubePattern.Single)
			{
				//===============================||
				// SINGLE CUBE
				//===============================||

				// Add to the group that will get transformed
				tileGroup.Add(randomStartTile);
				//Debug.Log("Adding Tile(" + randomStartTile.transform.localPosition.x + "," + randomStartTile.transform.localPosition.z + ") to the group. Should be initial random tile. IN SINGLE LOGIC");
				tilesToCheck = 1;

				// Start to blink this tile
				randomStartTile.GetComponentInChildren<Animation>().Play("tileColorBlink");

				// Then remove it from the list so it can't get chosen again in the meantime
				transformableTiles.Remove(randomStartTile);

				//Debug.Log("transformed a SINGLE tile");
			}
			else if (thisPattern == CubePattern.Double)
			{
				//===============================||
				// DOUBLE CUBES
				//===============================||

				// Add to transforming tile group
				tileGroup.Add(randomStartTile);
				//Debug.Log("Adding Tile(" + randomStartTile.transform.localPosition.x + "," + randomStartTile.transform.localPosition.z + ") to the group. Should be initial random tile. IN DOUBLE LOGIC");
				tilesToCheck = 1;

				// Find the other tile in this configuration
				// Choose Up/Down or Left/Right to check first
				bool checkUpDown;
				if (Random.value >= 0.5f)
					checkUpDown = true;
				else
					checkUpDown = false;

				// Check the direction for another cube
				if (checkUpDown)
				{
					//Debug.Log("Trying to add Up/Down");
					// See if there's one above/below this cube that isn't transformed at the moment
					if (CheckUpDownFirst(randomStartTile) != null && CheckUpDownFirst(randomStartTile) != randomStartTile)
					{
						tileGroup.Add(CheckUpDownFirst(randomStartTile));
						//Debug.Log("Adding Second Tile(" + CheckUpDownFirst(randomStartTile).transform.localPosition.x + "," + CheckUpDownFirst(randomStartTile).transform.localPosition.z + ") to the group. Should be initial random tile. IN DOUBLE LOGIC");
						tilesToCheck++;
					}
				}
				else // Check Left/Right for another cube
				{
					//Debug.Log("Trying to add Left/Right");
					// See if there's one to the left/right of this cube that isn't transformed at the moment
					if (CheckLeftRightFirst(randomStartTile) != null && CheckLeftRightFirst(randomStartTile) != randomStartTile)
					{
						tileGroup.Add(CheckLeftRightFirst(randomStartTile));
						//Debug.Log("Adding Second Tile(" + CheckLeftRightFirst(randomStartTile).transform.localPosition.x + "," + CheckLeftRightFirst(randomStartTile).transform.localPosition.z + ") to the group. Should be initial random tile. IN DOUBLE LOGIC");
						tilesToCheck++;
					}
				}

				//Debug.Log("transformed a DOUBLE tile");
			}
			else if (thisPattern == CubePattern.Triple)
			{
				//===============================||
				// TRIPLE CUBES
				//===============================||
				
				// Add to transforming tile group
				tileGroup.Add(randomStartTile);
				//Debug.Log("Adding Tile(" + randomStartTile.transform.localPosition.x + "," + randomStartTile.transform.localPosition.z + ") to the group. Should be initial random tile. IN TRIPLE LOGIC");
				tilesToCheck = 1;
				
				// Find the other tile in this configuration
				// Choose Up/Down or Left/Right to check first
				bool checkUpDown;
				if (Random.value >= 0.5f)
					checkUpDown = true;
				else
					checkUpDown = false;

				// Check the direction for another cube
				if (checkUpDown)
				{
					for (int i = 0; i < (int)CubePattern.Triple; i++)
					{
						if (CheckUpDownFirst(tileGroup[i]) != null && CheckUpDownFirst(tileGroup[i]) != tileGroup[i])
						{
							tileGroup.Add(CheckUpDownFirst(tileGroup[i]));
							//Debug.Log("Adding + " + i + "Tile(" + CheckUpDownFirst(tileGroup[i]).transform.localPosition.x + "," + CheckUpDownFirst(tileGroup[i]).transform.localPosition.z + ") to the group. Should be initial random tile. IN TRIPLE LOGIC");
							tilesToCheck++;
						}
						else
						{
							//Debug.LogWarning("Breaking out of the up/down for loop because number " + i + " in the tile group will be null");
							break;
						}
					}
				}
				else // Check Left/Right for another cube
				{
					// See if there's one to the left/right of this cube that isn't transformed at the moment
					for (int i = 0; i < (int)CubePattern.Triple; i++)
					{
						if (CheckLeftRightFirst(tileGroup[i]) != null && CheckLeftRightFirst(tileGroup[i]) != tileGroup[i])
						{
							tileGroup.Add(CheckLeftRightFirst(tileGroup[i]));
							//Debug.Log("Adding + " + i + "Tile(" + CheckLeftRightFirst(tileGroup[i]).transform.localPosition.x + "," + CheckLeftRightFirst(tileGroup[i]).transform.localPosition.z + ") to the group. Should be initial random tile. IN TRIPLE LOGIC");
							tilesToCheck++;
						}
						else
						{
							//Debug.LogWarning("Breaking out of the left/right for loop because number " + i + " in the tile group will be null");
							break;
						}
					}
				}

				//Debug.Log("transformed a TRIPLE tile");
			}
			else
			{
				// Wrong pattern?
			}

			// Run operations on all tiles in the group now
			//Debug.Log("Number of tiles in tile group that should play the blink anim is: " + tileGroup.Count);
			//Debug.Log("But the tiles to check count is: " + tilesToCheck);
			for (int i = 0; i < tilesToCheck; i++)
			{
				//Debug.Log("Tile" + i + " is at: " + tileGroup[i].transform.localPosition.x + "," + tileGroup[i].transform.localPosition.z + ")");

				// Start to blink this tile
				tileGroup[i].GetComponentInChildren<Animation>().Play("tileColorBlink");

				// Then remove it from the list so it can't get chosen again in the meantime
				//Debug.Log("Removing this tile from the transformables. Tile(" + tileGroup[i].transform.localPosition.x + "," + tileGroup[i].transform.localPosition.z + ")");
				transformableTiles.Remove(tileGroup[i]);
			}

			StartCoroutine(ActivateTransformation(tileGroup, cubeGoingUp));
		}
	}

	public IEnumerator ActivateTransformation(List<GameObject> tileGroup, bool cubeIsGoingUp)
	{
		yield return new WaitForSeconds(0.8f);

		for (int i = 0; i < tileGroup.Count; i++)
		{
			tileGroup[i].GetComponent<Tile>().isTransforming = true;

			Vector3 destination;
			if (cubeIsGoingUp)
			{
				destination = new Vector3(tileGroup[i].transform.localPosition.x, tileGroup[i].transform.localPosition.y + 1.0f, tileGroup[i].transform.localPosition.z);
				//Debug.Log("Moving Tile:" + tileGroup[i].transform.localPosition.x + "," + tileGroup[i].transform.localPosition.z + " to local Y position:" + (tileGroup[i].transform.localPosition.y + 1.0f));
			}
			else
			{
				destination = new Vector3(tileGroup[i].transform.localPosition.x, tileGroup[i].transform.localPosition.y - 1.0f, tileGroup[i].transform.localPosition.z);
				//Debug.Log("Moving Tile:" + tileGroup[i].transform.localPosition.x + "," + tileGroup[i].transform.localPosition.z + " to local Y position:" + (tileGroup[i].transform.localPosition.y - 1.0f));
			}

			//Debug.Log("Moving Tile:" + tileGroup[i].transform.localPosition.x + "," + tileGroup[i].transform.localPosition.z + " to local Y position:" + (tileGroup[i].transform.localPosition.y + 1.0f));

			// Set up Hashtable for move (NEED TO MAKE MOVEMENT IN LOCAL SPACE)
			Hashtable moveHash = new Hashtable();
			//moveHash.Add("easetype", "linear"); // Make movement smooth
			moveHash.Add("position", destination);
			moveHash.Add("time", cubeMovementTime);
			moveHash.Add("delay", 0.0f);
			moveHash.Add("isLocal", true);
			//moveHash.Add("oncomplete", "MovePlayerToPosition");
			//moveHash.Add("oncompletetarget", this.gameObject);

			iTween.MoveTo(tileGroup[i], moveHash);
			StartCoroutine(ReturnCubeToDefaultPosition(tileGroup, cubeIsGoingUp));
		}

		Invoke("StartCubeTransformations", 1.0f);
	}

	public IEnumerator ReturnCubeToDefaultPosition(List<GameObject> tileGroup, bool cubeWentUp)
	{
		yield return new WaitForSeconds(cubeTransformTime);

		for (int i = 0; i < tileGroup.Count; i++)
		{
			Vector3 destination = new Vector3(tileGroup[i].transform.localPosition.x, tileGroup[i].transform.localPosition.y, tileGroup[i].transform.localPosition.z);
			if (cubeWentUp)
			{
				// Cubes that go up must come down
				destination = new Vector3(tileGroup[i].transform.localPosition.x, tileGroup[i].transform.localPosition.y - 1.0f, tileGroup[i].transform.localPosition.z);

				// Set up Hashtable for move (NEED TO MAKE MOVEMENT IN LOCAL SPACE)
				Hashtable moveHash = new Hashtable();
				//moveHash.Add("easetype", "linear"); // Make movement smooth
				moveHash.Add("position", destination);
				moveHash.Add("time", cubeMovementTime);
				moveHash.Add("delay", 0.0f);
				moveHash.Add("isLocal", true);
				//moveHash.Add("oncomplete", "MovePlayerToPosition");
				//moveHash.Add("oncompletetarget", this.gameObject);
				
				iTween.MoveTo(tileGroup[i], moveHash);

				StartCoroutine(ResetTileStatus(tileGroup[i]));
			}
			else
			{
				// Cubes that go down never come back up
				//destination = new Vector3(thisTile.transform.position.x, 1.0f, thisTile.transform.position.z);
			}
		}
	}

	public IEnumerator ResetTileStatus(GameObject thisTile)
	{
		yield return new WaitForSeconds(cubeMovementTime);

		// Tile is done moving, reset it's status
		thisTile.GetComponent<Tile>().isTransforming = false;
		// Add it back to the list for tiles available for transformation
		//Debug.Log("Adding this tile back to the transformables. Tile(" + thisTile.transform.localPosition.x + "," + thisTile.transform.localPosition.z + ")");
		transformableTiles.Add(thisTile);
	}

	public GameObject CheckUpDownFirst(GameObject fromThisTile)
	{
		if ((int)fromThisTile.transform.localPosition.x + 1 <= 9
		    && !Tiles[(int)fromThisTile.transform.localPosition.x + 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().isTransforming
		    && !Tiles[(int)fromThisTile.transform.localPosition.x + 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is above
			return(Tiles[(int)fromThisTile.transform.localPosition.x + 1, (int)fromThisTile.transform.localPosition.z]);
		}
		else if ((int)fromThisTile.transform.localPosition.x - 1 >= 0
		         && !Tiles[(int)fromThisTile.transform.localPosition.x - 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().isTransforming
		         && !Tiles[(int)fromThisTile.transform.localPosition.x - 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is below
			return(Tiles[(int)fromThisTile.transform.localPosition.x - 1, (int)fromThisTile.transform.localPosition.z]);
		}

		// If we didn't find one above or below, check left/right
		// See if there's one to the left/right of this cube that isn't transformed at the moment
		if ((int)fromThisTile.transform.localPosition.z - 1 >= 0
		    && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z - 1].GetComponent<Tile>().isTransforming
		    && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z - 1].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is left
			return(Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z - 1]);
		}
		else if ((int)fromThisTile.transform.localPosition.z + 1 <= 9
		         && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z + 1].GetComponent<Tile>().isTransforming
		         && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z + 1].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is right
			return(Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z + 1]);
		}

		//Debug.Log("Going to guess we didn't find one in left/right either");

		return null;
	}

	public GameObject CheckLeftRightFirst(GameObject fromThisTile)
	{
		if ((int)fromThisTile.transform.localPosition.z - 1 >= 0
		    && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z - 1].GetComponent<Tile>().isTransforming
		    && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z - 1].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is left
			return(Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z - 1]);
		}
		else if ((int)fromThisTile.transform.localPosition.z + 1 <= 9
		         && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z + 1].GetComponent<Tile>().isTransforming
		         && !Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z + 1].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is right
			return(Tiles[(int)fromThisTile.transform.localPosition.x, (int)fromThisTile.transform.localPosition.z + 1]);
		}

		// If we didn't find one left or right, check up/down
		// See if there's one above/below this cube that isn't transformed at the moment
		if ((int)fromThisTile.transform.localPosition.x + 1 <= 9
		    && !Tiles[(int)fromThisTile.transform.localPosition.x + 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().isTransforming
		    && !Tiles[(int)fromThisTile.transform.localPosition.x + 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is above
			return(Tiles[(int)fromThisTile.transform.localPosition.x + 1, (int)fromThisTile.transform.localPosition.z]);
		}
		else if ((int)fromThisTile.transform.localPosition.x - 1 >= 0
		         && !Tiles[(int)fromThisTile.transform.localPosition.x - 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().isTransforming
		         && !Tiles[(int)fromThisTile.transform.localPosition.x - 1, (int)fromThisTile.transform.localPosition.z].GetComponent<Tile>().hasPickupOnIt)
		{
			// One is below
			return(Tiles[(int)fromThisTile.transform.localPosition.x - 1, (int)fromThisTile.transform.localPosition.z]);
		}

		//Debug.Log("Going to guess we didn't find one in up/down either");

		return null;
	}
}






