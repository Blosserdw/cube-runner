using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public GameObject pickupArtObject = null;
	private BoxCollider myCollider = null;
	public ParticleSystem myParticles = null;
	public int myWorth = 1;

	// Use this for initialization
	void Start ()
	{
		myCollider = gameObject.GetComponent<BoxCollider>();
		pickupArtObject.GetComponent<Animation>()["pickupBob"].time = Random.Range(0.0f, 1.0f);
		pickupArtObject.GetComponent<Animation>().Play("pickupBob");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnTriggerEnter()
	{
		myCollider.enabled = false; // So we don't hit it twice?
		Debug.Log("YOU GOT AN ITEM!!!! YAY!!");
		pickupArtObject.SetActive(false);
		UIManager.Instance.AddToScore(myWorth);
		myParticles.Play();

		Invoke("DestroyMe", 1.0f);
	}

	public void DestroyMe()
	{
		Destroy(gameObject);
	}
}
