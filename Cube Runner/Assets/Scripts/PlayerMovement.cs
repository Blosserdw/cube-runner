using UnityEngine;
using System.Collections;
//using Pathfinding;

public class PlayerMovement : MonoBehaviour {

	//private AstarAI astarAIScript = null;
	
	//The calculated path
	//public Path path;
	
	//The AI's speed per second
	public float speed;
	
	public Vector3 direction = new Vector3(0f, 0f, 0f);

	private Rigidbody mRigidBody = null;

	// Use this for initialization
	void Start ()
	{
		// Grab the rigidbody
		mRigidBody = GetComponent<Rigidbody>();

		// Initialize the direction going up
		direction = new Vector3(speed, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update ()
	{

		//=MOVEMENT CONTROLS WITH WASD====================================================||
		// Move right
		if (GameManager.Instance.currentMap != null)
		{
			// Move Right
			if (Input.GetKeyDown(KeyCode.D))
			{
				// Change direction
				//Debug.Log("Moving Right... (Z Negative)");
				direction = new Vector3(0f, 0f, -speed);
			}
			
			// Move left
			if (Input.GetKeyDown(KeyCode.A))
			{
				// Change direction
				//Debug.Log("Moving Left... (Z Positive)");
				direction = new Vector3(0f, 0f, speed);
			}
			
			// Move up
			if (Input.GetKeyDown(KeyCode.W))
			{
				// Change direction
				//Debug.Log("Moving Up... (X Positive)");
				direction = new Vector3(speed, 0f, 0f);
			}
			
			// Move down
			if (Input.GetKeyDown(KeyCode.S))
			{
				// Change direction
				//Debug.Log("Moving Down... (X Negative)");
				direction = new Vector3(-speed, 0f, 0f);
			}
		}

		// X Check, make sure we don't go off the edge (MIGHT be used later for cube side transferring
		if (transform.position.x >= 9.2f)
		{
			direction = new Vector3(-speed, 0f, 0f);
		}
		else if (transform.position.x <= -0.2f)
		{
			direction = new Vector3(speed, 0f, 0f);
		}

		// Z Check
		if (transform.position.z >= 9.2f)
		{
			direction = new Vector3(0f, 0f, -speed);
		}
		else if (transform.position.z <= -0.2f)
		{
			direction = new Vector3(0f, 0f, speed);
		}

		// After determining direction, move the player
		//mRigidBody.AddForce(direction); // Too slow with momentum
		mRigidBody.velocity = direction;
	}
}
