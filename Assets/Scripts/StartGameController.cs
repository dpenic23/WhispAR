using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour {

	private Button startButton;

	void Start(){
		startButton = GetComponent<Button> ();
		startButton.onClick.AddListener (() => {
			StartButtonClicked ();
		});
	}

	private void StartButtonClicked(){
		SceneManager.LoadScene ("SoundScene");
	}

}
