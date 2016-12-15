using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// Represents a single story item which will be
	// placed in camera view once its location is reached
	private class StoryItem {

		public string Name;
		public Vector2 Coordinates;

		// Constructor
		public StoryItem(string name, float latitude, float longitude) {
			Name = name;
			Coordinates = new Vector2(latitude, longitude);
		}

		// Calculates distance between this story item and provided location
		public float DistanceFromLocation(float latitude, float longitude) {
			Vector2 location = new Vector2 (latitude, longitude);
			return Vector2.Distance (Coordinates, location);
		}

	}

	// Service used for retreiving the GPS and compass data
	private LocationService locationService;

	// Treshold for calculating is user on the specific spot
	private float DISTANCE_TRESHOLD = 0.00025f;

	// Collection of all story items in the game
	private static List<StoryItem> storyItems;
	// Story item to be found next
	private static StoryItem nextStoryItem = null;
	// Is game finished
	private static bool allItemsCollected = false;

	// Directional sound period
	private int SOUND_PERIOD = 1200;
	private int timer;

	private static bool initialized = false;
	private static bool soundPlaying = false;

	// 0 for directional sounds
	private static int gameState = 1;

	public Rigidbody rbDirection;
	public AudioSource cameraAudioSource;
	public AudioSource directionalAudioSource;

	public AudioClip whistlingSound;
	public List<AudioClip> directionalSounds;
	public List<AudioClip> binauralSounds;

	// Debug text
	public Text text;

	void Start () {
		if (!initialized) {
			InitializeStoryItems ();
			nextStoryItem = storyItems [0];

			initialized = true;
		}

		// GPS
		locationService = Input.location;
		StartCoroutine (StartGPSTracking ());

		// Compass
		Input.compass.enabled = true;

		// Directional sound
		timer = SOUND_PERIOD;
		timer = 100;
	}

	private void InitializeStoryItems() {
		// Initialize collection with story items
		storyItems = new List<StoryItem> ();

		// Add all story items
		storyItems.Add (new StoryItem ("first", 59.346580f, 18.073124f));
		storyItems.Add (new StoryItem ("second", 59.347511f, 18.073634f));
	}

	IEnumerator StartGPSTracking() {
		int maxWait = 10;

		if (!locationService.isEnabledByUser) {
			Debug.Log("Location Services not enabled by user");
			yield break;
		}

		locationService.Start (1f, 1f);

		while (locationService.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		if (maxWait < 1){
			Debug.Log("Timed out");
			yield break;
		}

		if (locationService.status == LocationServiceStatus.Failed) {
			Debug.Log("Unable to determine device location");
			yield break;
		}

	}

	void Update () {
		if (soundPlaying) {
			return;
		}

		if (gameState == 0) {
			TrackPosition ();

			timer--;
			if (timer <= 0) {
			//	if (keyCollected) {
					// Play random directional sound
			//		int soundIndex = Random.Range (0, directionalSounds.Count);
			//		directionalAudioSource.clip = directionalSounds [soundIndex];
			//	} else {
					directionalAudioSource.clip = whistlingSound;
			//	}

				directionalAudioSource.Play ();

				timer = SOUND_PERIOD;
			}
		}

		if (gameState == 1) {
			StartCoroutine (PlaySound (binauralSounds [0]));
			gameState = 0;
		}

		if (gameState == 2) {
			StartCoroutine (PlaySound (binauralSounds [2]));
			gameState = 0;
		}

		if (gameState == 3) {
			StartCoroutine (PlaySound (binauralSounds [4]));
			gameState = 0;
		}

	}

	private void TrackPosition(){
		// Read current GPS location
		float currentLatitude = locationService.lastData.latitude;
		float currentLongitude = locationService.lastData.longitude;

		text.text = "My Location: " + currentLatitude + ", " + currentLongitude;

		if (!allItemsCollected) {
			// Calculate how far is next story item to collect
			float distanceToNextItem = nextStoryItem.DistanceFromLocation (currentLatitude, currentLongitude);
			text.text += "\nDistance: " + distanceToNextItem.ToString ();

			if (distanceToNextItem <= DISTANCE_TRESHOLD) {
				// Prepare for the next scene and load it
				PlayerPrefs.SetString ("storyItem", nextStoryItem.Name);

				// Item is to be collected, remove it
				storyItems.Remove (nextStoryItem);
				if (storyItems.Count > 0) {
					nextStoryItem = storyItems [0];
				} else {
					allItemsCollected = true;
				}

				soundPlaying = true;
				StartCoroutine (ChangeScene ());
			}
		}

		CalculateSoundDirection (currentLatitude, currentLongitude, nextStoryItem.Coordinates.x, nextStoryItem.Coordinates.y);
	}

	IEnumerator PlaySound(AudioClip clip){
		soundPlaying = true;

		cameraAudioSource.clip = clip;
		cameraAudioSource.Play ();
		yield return new WaitForSeconds (clip.length);

		soundPlaying = false;
		gameState = 0;
	}

	IEnumerator ChangeScene(){
		soundPlaying = true;

		string item = PlayerPrefs.GetString ("storyItem");

		if (item == "first") {
			cameraAudioSource.clip = binauralSounds [1];
			cameraAudioSource.Play ();
			yield return new WaitForSeconds (binauralSounds [1].length);
			gameState = 2;
		} else if (item == "second") {
			cameraAudioSource.clip = binauralSounds [3];
			cameraAudioSource.Play();
			yield return new WaitForSeconds (binauralSounds[3].length);
			gameState = 3;
		}

		soundPlaying = false;
		SceneManager.LoadScene ("ARScene");
	}

	private void CalculateSoundDirection(float startLat, float startLong, float endLat, float endLong){

		// Calculate vector from current to the next location
		float x = endLat - startLat;
		float y = endLong - startLong;
		float angle = Mathf.Atan2 (y, x);

		text.text = "Angle: " + angle.ToString ();

		// If vector is in 3rd or 4th quadrant, add PI
		if(angle < 0){
			angle += 2 * Mathf.PI;
		}
			
		// Convert angle from radians to degrees
		angle = angle * (180f / Mathf.PI);

		// Device orientation in degrees
		float deviceOrientation = Input.compass.trueHeading;

		text.text += "\nDevOr: " + deviceOrientation.ToString ();
		//text.text += "\nMy: " + startLat.ToString () + ", " + startLong.ToString ();
		//text.text += "\nTarget: " + endLat.ToString () + ", " + endLong.ToString ();

		float target = angle - deviceOrientation;
		text.text += "\nTA: " + target.ToString ();
		target = filterAngle (target);
		text.text += "\nFTA: " + target.ToString ();

		rbDirection.transform.localEulerAngles = new Vector3 (0f, target, 0f);

	}

	private float filterAngle(float angle){
		float step = 30;

		bool isNegative = (angle < 0);
		angle = Mathf.Abs (angle);

		float minDistance = 0f;
		int index = -1;

		for (int i = 0; i < (360 / step); i ++) {
			float distance = Mathf.Abs (angle - i * step);

			if (index == -1 || distance < minDistance) {
				index = i;
				minDistance = distance;
			}
		}

		float result = index * step;
		if (isNegative) {
			result = -result;
		}

		return result;

	}

}
