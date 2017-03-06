using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {
	[SerializeField] AudioClip[] sounds;

	Dictionary<string, AudioSource> channels = new Dictionary<string, AudioSource>();
	Dictionary<string, AudioClip> _sounds = new Dictionary<string, AudioClip>();

	// Use this for initialization
	void Awake () {
		foreach (AudioClip sound in sounds)
		{
			_sounds.Add(sound.name, sound);
		}
	}

	public void PlaySound(string sound, string channel)
	{
		PlaySound(sound, channel, false);
	}
	public void PlaySound(string sound, string channel, bool loop)
	{
		if(_sounds.ContainsKey(sound))
		{
			if (!channels.ContainsKey(channel))
			{
				channels.Add(channel, gameObject.AddComponent<AudioSource>());
			}
			channels[channel].clip = _sounds[sound];
			channels[channel].loop = loop;
			channels[channel].volume = 0.5f;
			channels[channel].Play();
		}
	}


}
