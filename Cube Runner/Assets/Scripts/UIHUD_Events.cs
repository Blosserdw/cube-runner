using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHUD_Events : MonoBehaviour {

	public CanvasGroup fadeCanvasGroup; // Because text field alphas don't animate well? What?
	public GameObject turnLabel;

	// Use this for initialization
	void Start ()
	{
		// Make turn label invisible at the start of the game
	}

	public void FadeInTurnText(string turnEntity)
	{
		turnLabel.GetComponent<Text>().text = turnEntity + "'s Turn...";
		fadeCanvasGroup.gameObject.GetComponent<Animation>().Play("canvasFadeIn");

		StartCoroutine(FadeOutTurnText(turnEntity));
	}

	public IEnumerator FadeOutTurnText(string turnEntity)
	{
		yield return new WaitForSeconds(3.0f);

		turnLabel.GetComponent<Text>().text = turnEntity + "'s Turn...";
		fadeCanvasGroup.gameObject.GetComponent<Animation>().Play("canvasFadeOut");
	}
}
