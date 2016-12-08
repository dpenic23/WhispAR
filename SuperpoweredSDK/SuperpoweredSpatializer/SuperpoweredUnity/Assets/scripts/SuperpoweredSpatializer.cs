using UnityEngine;
using System.Collections;

public class SuperpoweredSpatializer : MonoBehaviour
{
    public bool AlternativeSound = false;
    private AudioListener audioListener = null;
    private float nextUpdateTime = 0.0f;

    void Start()
    {
        audioListener = (AudioListener)FindObjectOfType(typeof(AudioListener));
        if (!audioListener) Debug.Log("No AudioListener instance can be found, occlusion will not work.");
    }

    void Update()
    {
        if (Time.time > nextUpdateTime) { // Update every 150 ms.
          nextUpdateTime = Time.time + 0.15f;

          var source = GetComponent<AudioSource>();
          source.SetSpatializerFloat(0, AlternativeSound ? 1.0f : 0.0f);

          float numObjects = GetNumberOfObjectsBetweenListenerAndAudioSource();
          float occlusion = numObjects < 10.0f ? (numObjects * 0.1f) : 1.0f; // Very simply occlusion logic: 1 object = 0.1, 2 object = 0.2, ...
          occlusion = occlusion * (2.0f - occlusion); // Linear doesn't sound well, apply a simple logarithmic function.
          source.SetSpatializerFloat(1, occlusion);
        }
    }

    private float GetNumberOfObjectsBetweenListenerAndAudioSource() {
      if (!audioListener) return 0.0f;
      float numberOfObjectsBetweenListenerAndAudioSource = 0.0f;
      Transform listenerTransform = audioListener.transform;
      Vector3 listenerPosition = listenerTransform.position;
      Vector3 sourceFromListener = transform.position - listenerPosition;
      RaycastHit[] hits = Physics.RaycastAll(listenerPosition, sourceFromListener, sourceFromListener.magnitude);
      foreach (RaycastHit hit in hits) {
        if ((hit.transform != listenerTransform) && (hit.transform != transform)) numberOfObjectsBetweenListenerAndAudioSource += 1.0f;
      }
      return numberOfObjectsBetweenListenerAndAudioSource;
    }
}
