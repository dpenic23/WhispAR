using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kudan.AR.Samples;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARController : MonoBehaviour {

	public GameObject first;
	public GameObject second;

	private int gameState = 0;

	private string itemName;

	public Text text1;
	public Text text2;

	public SampleApp sa;

	void Start(){
		itemName = PlayerPrefs.GetString ("storyItem");
		if (itemName == "first") {
			text1.text = "KEY";
		} else if (itemName == "second") {
			text1.text = "CHEST";
			PlayerPrefs.SetInt ("chest", 1);
		}
		//text1.text = itemName;
	}

	void Update(){
		if (gameState == 0) {
			text2.text = "Tap to discover!";
			gameState = 1;
		}

		if (gameState == 1) {
			if (Input.touchCount > 0) {
				setActive(itemName, true);
				sa.StartClicked ();
				gameState = 2;
				text2.text = "Tap to collect!";
			}
		}

		if (gameState == 2) {
			if (Input.touchCount == 0) {
				gameState = 3;
			}
		}

		if (gameState == 3) {
			if (Input.touchCount > 0) {
				setActive (itemName, false);
				text2.text = "Collected!";
				gameState = 4;
			}
		}

		if (gameState == 4) {
			if (Input.touchCount == 0) {
				SceneManager.LoadScene ("SoundScene");
			}
		}

	}

	private void setActive(string itemName, bool isActive){

		switch (itemName) {
		case "first":
			first.SetActive (isActive);
			break;
		case "second":
			second.SetActive (isActive);
			break;
		default:
			return;
		}

	}

}
