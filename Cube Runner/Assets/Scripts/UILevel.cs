using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILevel : MonoBehaviour {

	public Text levelNumberText = null;

	// Use this for initialization
	void Start ()
	{
		SetLevel(GameManager.Instance.levelNumber);
		gameObject.GetComponent<Animation>().Play("levelNumberFade");
		StartCoroutine(DelayedDestroy());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void SetLevel(int levelNum)
	{
		levelNumberText.text = "Level " + levelNum.ToString();
	}

	public IEnumerator DelayedDestroy()
	{
		yield return new WaitForSeconds(3.0f);

		// After animation of level is done, destroy this object
		UIManager.Instance.DestroyLevelNumber();
	}
}
