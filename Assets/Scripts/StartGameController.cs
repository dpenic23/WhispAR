using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour {

	private Button startButton;

	public Texture2D progressBar;

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
