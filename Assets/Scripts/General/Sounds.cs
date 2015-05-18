using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class Sounds : MonoBehaviour {

	public AudioSource BassSource, MelodySource, ChangeableSource, EffectsSource;

	public List<AudioClip> Basses, Melodies, Changeables;

	public static Sounds Instance {get; private set;}
	
	void Awake () {
		if(Instance == null) {
			//If I am the first instance, make me the Singleton
			Instance = this;
			//DontDestroyOnLoad(this);
		}
		else {
			//If a Singleton already exists and you find another reference in scene, destroy it!
			if(this != Instance)
				Destroy(this.gameObject);
		}
	}

	void Start () {
		StartCoroutine(RandPlay(BassSource, Basses, new float[2] {24f, 28f}, 4f));
		StartCoroutine(RandPlay(MelodySource, Melodies, new float[3] {40f, 44f, 48f}, 4f));
		StartCoroutine(RandPlay(ChangeableSource, Changeables, new float[3] {8f, 12f, 16f}, 8f));
	}

	IEnumerator RandPlay (AudioSource source, List<AudioClip> clips, float[] times, float start) {

		yield return new WaitForSeconds(start);

		while (Application.loadedLevel==1) {

			source.PlayOneShot(clips[Random.Range(0,clips.Count)]);

			yield return new WaitForSeconds(times[Random.Range(0,times.Length)]);
		}
	}

	public void PlaySoundEffect (AudioClip clip) {
		EffectsSource.PlayOneShot(clip);
	}
	
}
