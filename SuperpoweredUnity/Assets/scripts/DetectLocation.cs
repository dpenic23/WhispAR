using UnityEngine;
using System.Collections;
using Kudan.AR.Samples;
using UnityEngine.UI;

public class DetectLocation : MonoBehaviour {

	private Vector2 targetCoordinates;
	private Vector2 deviceCoordinates;
	private float distanceFromTarget = 0.0009f;
	private float proximity = 0.001f;
	private float sLatitude, sLongitude; 
	public float dLatitude = 59.412217f, dLongitude = 17.919450f;
	private bool enableByRequest = true;
	public Text text;
	public SampleApp sa;
	private int counter = 0;
	private int timeCounter = 1000;

	public bool checkByLocation = false;

	void Start(){
		targetCoordinates = new Vector2 (dLatitude, dLongitude);
		StartCoroutine (getLocation ());
		//InvokeRepeating ("getLocation", 1.0f, 2.0f);
	}

	void Update(){
		//timeCounter--;
		if (timeCounter == 0) {
			StartCoroutine (getLocation ());
			timeCounter = 1000;
		}
	}

	IEnumerator getLocation(){
		int maxWait = 10;

		LocationService service = Input.location;
		if (!enableByRequest && !service.isEnabledByUser) {
			Debug.Log("Location Services not enabled by user");
			yield break;
		}

		service.Start();

		while (service.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		if (maxWait < 1){
			Debug.Log("Timed out");
			yield break;
		}

		if (service.status == LocationServiceStatus.Failed) {
			Debug.Log("Unable to determine device location");
			yield break;
		} else {
			text.text = "Target Location : "+dLatitude + ", "+dLongitude+"\nMy Location: " + service.lastData.latitude + ", " + service.lastData.longitude;
			sLatitude = service.lastData.latitude;
			sLongitude = service.lastData.longitude;
		}

		//service.Stop();

		startCalculate ();
	}

	public void startCalculate(){
		if (!checkByLocation) {
			sa.StartClicked ();
			return;
		}

		deviceCoordinates = new Vector2 (sLatitude, sLongitude);
		proximity = Vector2.Distance (targetCoordinates, deviceCoordinates);
		if (proximity <= distanceFromTarget) {
			text.text = text.text + "\nDistance : " + proximity.ToString ();
			text.text += "\nTarget Detected";
			sa.StartClicked ();
		} else {
			text.text = text.text + "\nDistance : " + proximity.ToString ();
			text.text += "\nTarget not detected, too far!";
		}
		text.text += "\nCounter :" + counter;
	}

}
