using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	float masterVolumePercent = 1;
	float musicVolumePercent = 1;
	float sfxVolumePercent = 1;

	public static AudioManager instance;

	AudioSource playerAS;
	//AudioSource[] musicSources;
	//int activeMusicSourceIndex;

	void Awake(){
		
		instance = this;

		playerAS = new AudioSource();
		GameObject newMusicSource = new GameObject ("Music Source 1");
		newMusicSource.transform.parent = transform;
		//playerAS = GetComponent<AudioSource> ();
		/*
		musicSources = new AudioSource[2];
		for (int i = 0; i < 2; i++) {
			GameObject newMusicSource = new GameObject ("Music source" + (i + 1));
			musicSources[i] = newMusicSource.AddComponent<AudioSource> ();
			newMusicSource.transform.parent = transform;
		}
		*/
	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 1) {
		//activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		//musicSources [activeMusicSourceIndex].clip = clip;
		//musicSources [activeMusicSourceIndex].Play ();

		//StartCoroutine [AnimateMusicCrossfade (fadeDuration)];

		playerAS.Play();
	}

	public void PlaySound(AudioClip clip, Vector2 pos){
		AudioSource.PlayClipAtPoint (clip, pos, sfxVolumePercent * masterVolumePercent);
	}


	/*
	 * IEnumerator AnimateMusicCrossfade(float duration) {
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * 1 / duration;
			musicSources [activeMusicSourceIndex].volume = Mathf.Lerp (0, musicVolumePercent * masterVolumePercent, percent);
			musicSources [1-activeMusicSourceIndex].volume = Mathf.Lerp (0, musicVolumePercent * masterVolumePercent, 0,  percent);
			yield return null;
		}
	}
	*/
}
