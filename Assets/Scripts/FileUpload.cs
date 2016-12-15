using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class FileUpload : MonoBehaviour
{
	private string m_LocalFileName = Path.Combine(Application.dataPath, "myfile.wav");
	private string m_URL = "http://edeproduction.fi/upload.php";
	AudioClip myAudioClip;

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
		// version 1
		//postForm.AddBinaryData("theFile",localFile.bytes);
		// version 2
		postForm.AddBinaryData("theFile",localFile.bytes,localFileName,"text/plain");
		WWW upload = new WWW(uploadURL,postForm);        
		yield return upload;
		if (upload.error == null)
			Debug.Log("upload done :" + upload.text);
		else
			Debug.Log("Error during upload: " + upload.error);
	}

	IEnumerator PlaySoundFromURL (string url)
	{
		WWW www = new WWW(url);

		while (!www.isDone)
			yield return null;

		AudioSource audio = GetComponent<AudioSource>();
		audio.clip = www.audioClip;
		audio.Play();
	}

	void UploadFile(string localFileName, string uploadURL)
	{
		StartCoroutine(UploadFileCo(localFileName, uploadURL));
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(10,10,60,50),"Record"))
		{ 
			myAudioClip = Microphone.Start ( null, false, 5, 44100 );
		}
		if (GUI.Button(new Rect(10,70,60,50),"Save"))
		{
			SavWav.Save("myfile.wav", myAudioClip);
		}
		if (GUI.Button(new Rect(10,150,60,50),"Upload"))
		{ 
			UploadFile(m_LocalFileName,m_URL);
		}
		if (GUI.Button(new Rect(10,200,60,50),"Download"))
		{ 
			StartCoroutine(PlaySoundFromURL("http://edeproduction.fi/myfile.wav"));
		}
	}
}
