using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public GameObject pickupArtObject = null;
	private BoxCollider myCollider = null;
	public ParticleSystem myParticles = null;

	// Use this for initialization
	void Start ()
	{
		myCollider = gameObject.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnTriggerEnter()
	{
		Debug.Log("YOU GOT AN ITEM!!!! YAY!!");
		pickupArtObject.SetActive(false);
		myParticles.Play();

		Invoke("DestroyMe", 1.0f);
	}

	public void DestroyMe()
	{
		Destroy(gameObject);
	}
}
