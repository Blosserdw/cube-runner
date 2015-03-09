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
		LoadHUD();
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public GameObject HUDPrefab = null;
	public GameObject HUDObject = null;
	public UIHUD_Events HUDScript = null;
	
	public void LoadHUD()
	{
		// Load the main menu UI
		HUDObject = GameObject.Instantiate(Resources.Load("Prefabs/HUD")) as GameObject;
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
}
