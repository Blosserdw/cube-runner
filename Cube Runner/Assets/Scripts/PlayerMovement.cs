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

	public bool stopMovement = false;

	// Use this for initialization
	void Start ()
	{
		// Grab the rigidbody
		mRigidBody = GetComponent<Rigidbody>();

		// Initialize the direction going up
		direction = new Vector3(speed, mRigidBody.velocity.y, 0f);
	}
	
	// Update is called once per frame
	void Update ()
	{

		//=MOVEMENT CONTROLS WITH WASD====================================================||
		// Move right
		if (GameManager.Instance.currentMap != null)
		{
			if (!stopMovement)
			{
				// Move Right
				if (Input.GetKeyDown(KeyCode.D))
				{
					// Change direction
					//Debug.Log("Moving Right... (Z Negative)");
					direction.z = -speed;
					direction.x = 0.0f;
				}
				
				// Move left
				if (Input.GetKeyDown(KeyCode.A))
				{
					// Change direction
					//Debug.Log("Moving Left... (Z Positive)");
					direction.z = speed;
					direction.x = 0.0f;
				}
				
				// Move up
				if (Input.GetKeyDown(KeyCode.W))
				{
					// Change direction
					//Debug.Log("Moving Up... (X Positive)");
					direction.x = speed;
					direction.z = 0.0f;
				}
				
				// Move down
				if (Input.GetKeyDown(KeyCode.S))
				{
					// Change direction
					//Debug.Log("Moving Down... (X Negative)");
					direction.x = -speed;
					direction.z = 0.0f;
				}



				// X Check, make sure we don't go off the edge (MIGHT be used later for cube side transferring
				if (transform.position.x >= 9.2f)
				{
					direction.x = -speed;
					direction.z = 0.0f;
				}
				else if (transform.position.x <= -0.2f)
				{
					direction.x = speed;
					direction.z = 0.0f;
				}
				
				// Z Check
				if (transform.position.z >= 9.2f)
				{
					direction.z = -speed;
					direction.x = 0.0f;
				}
				else if (transform.position.z <= -0.2f)
				{
					direction.z = speed;
					direction.x = 0.0f;
				}
				
				// After determining direction, move the player
				//mRigidBody.AddForce(direction); // Too slow with momentum
				mRigidBody.velocity = direction;
			}
			else
			{
				gameObject.GetComponent<CharacterController>().enabled = false;
				gameObject.GetComponentInChildren<SphereCollider>().enabled = false;

				// Movement is stopped
				direction.x = 0.0f;
				direction.z = 0.0f;
				mRigidBody.velocity = direction;
			}
		}
	}
}
