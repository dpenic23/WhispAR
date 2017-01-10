using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FileUpload : MonoBehaviour
{
	string m_LocalFileName;
	string m_URL = "http://edeproduction.fi/agi16/upload.php";
	AudioClip myAudioClip;
	AudioSource audio;
	List<SoundLoc> soundLocs;
	DatabaseReference reference;
	public GameController gameController;

	// Remove these when GPS getting is implemented
	float myLat;
	float myLong;

	public class SoundLoc {
		public float latitude;
		public float longitude;
		public string url;

		public SoundLoc() {
		}

		public SoundLoc(float latitude, float longitude, string url) {
			this.latitude = latitude;
			this.longitude = longitude;
			this.url = url;
		}
	}

	void Start(){

		myLat = 1f;
		myLong = 1f;

		m_LocalFileName = Path.Combine(Application.persistentDataPath, "myfile.wav");
		audio = GetComponent<AudioSource>();

		try{
			// Set up the Editor before calling into the realtime database.
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://whisperofthetown.firebaseio.com/");
		} catch {
			return;
		}

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.GetReference("SoundLocations");
		// Initialize sound location array
		soundLocs = new List<SoundLoc>();

		reference.ChildAdded += (object sender, ChildChangedEventArgs args) => {
			if (args.DatabaseError != null) {
				Debug.LogError(args.DatabaseError.Message);
				return;
			}
			// Save data in sound location array
			soundLocs.Add(new SoundLoc( float.Parse(args.Snapshot.Child("latitude").Value.ToString()), 
										float.Parse(args.Snapshot.Child("longitude").Value.ToString()),
									    args.Snapshot.Child("url").Value.ToString()));
		};


	}

	void Update(){
		return;

		if (!audio.isPlaying) {
			// Iterate all soundlocations and find closest
			string url = null;
			float distance = 1000f;
			foreach (SoundLoc soundLoc in soundLocs) {
				// calculate distance
				float newDistance = DistanceToSound (soundLoc);
				if (newDistance < distance) {
					url = soundLoc.url;
					distance = newDistance;
				}
			}
			if (url != null) {
				Debug.Log ("closest: " + url + " distance: " + distance.ToString ());
				gameController.directionalAudioSource.Stop ();
				StartCoroutine (PlaySoundFromURL (url));
			}
		} else if (!gameController.directionalAudioSource.isPlaying) {
			gameController.directionalAudioSource.Play ();
		}
	}

	float DistanceToSound(SoundLoc soundLoc){
		float distance = Mathf.Sqrt (Mathf.Pow (myLat - soundLoc.latitude, 2) + Mathf.Pow (myLong - soundLoc.longitude, 2));

		return distance;
	}

	IEnumerator UploadFileCo(string localFileName, string uploadURL)
	{
		WWW localFile = new WWW("file:///" + localFileName);
		yield return localFile;
		if (localFile.error == null)
			Debug.Log("Loaded file successfully");
		else
		{
			Debug.Log("Open file error: "+localFile.error);
			yield break; // stop the coroutine here
		}
		WWWForm postForm = new WWWForm();
		postForm.AddBinaryData("theFile",localFile.bytes,localFileName,"text/plain");
		WWW upload = new WWW(uploadURL,postForm);        
		yield return upload;
		if (upload.error == null){

			string url = upload.text;

			SoundLoc soundLoc = new SoundLoc (myLat, myLong, url);
			reference.Push().SetRawJsonValueAsync(JsonUtility.ToJson (soundLoc));
			Debug.Log ("Uploaded!");
		}else{
			Debug.Log("Error during upload: " + upload.error);
		}
	}
	IEnumerator PlaySoundFromURL (string url)
	{
		WWW www = new WWW(url);

		while (!www.isDone)
			yield return null;

		audio.clip = www.audioClip;
		audio.Play();
	}

	void UploadFile(string localFileName, string uploadURL)
	{
		StartCoroutine(UploadFileCo(localFileName, uploadURL));
	}
	void OnGUI()
	{
		if (GUI.Button(new Rect(200,100,120,100),"Record"))
		{ 
			myAudioClip = Microphone.Start ( null, false, 5, 44100 );
		}
		if (GUI.Button(new Rect(200,250,120,100),"Play & Upload"))
		{
			SavWav.Save("myfile.wav", myAudioClip);
			AudioSource audio = GetComponent<AudioSource>();
			audio.clip = myAudioClip;
			audio.Play();
			UploadFile(m_LocalFileName,m_URL);
		}
	}
}
