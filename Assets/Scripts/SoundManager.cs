using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static GameObject soundManagerObject; //Allows other scripts to call functions from SoundManager. 
	public static SoundManager thisSoundManager; //Allows other scripts to call functions from SoundManager. 
	public static SoundManager instance;         //Allows other scripts to call functions from SoundManager. 

	private float sweepTimer = 0F;
	private float sweepTime = 0.25F; //SoundManager culls finished sounds every [sweepTime] seconds.

    public Dictionary<string, SoundGroup> sounds = new Dictionary<string, SoundGroup>();
    public List<SoundInstance> autoFadeGroup = new List<SoundInstance>();

    void Awake () {
	    if(soundManagerObject == null) {
		    soundManagerObject = this.gameObject;
	    }
	    if(thisSoundManager == null) {
		    thisSoundManager = this;
		    instance = this;
	    }
	    else if (thisSoundManager != this) {
		    Destroy (gameObject);
	    }

        if (sounds.Count <= 0) {
            foreach (GameObject GO in Resources.LoadAll("Sounds")) {

                SoundGroup newSoundGroup = new SoundGroup();

                foreach (AudioSource AS in GO.GetComponents<AudioSource>()) {
                    newSoundGroup.sources.Add (AS);
                }

                if (!sounds.ContainsKey (GO.name)) {
                    sounds.Add (GO.name, newSoundGroup);
                }
            }
        }

    }

	void Update () {

		CleanupSweep (); 	//Deletes expired SoundInstances.
		FadeLoops ();

	}

	//Play Sound
	public SoundInstance PlaySound (SoundCall call, GameObject producingObject) {
        if (call.soundUsed != null && (call.soundKey == null || call.soundKey == "")) {
            call.soundKey = call.soundUsed.name;
        }
		if (sounds.ContainsKey(call.soundKey)) {											//Sound must be in sound dictionary
			if (sounds[call.soundKey].instances.Count <= 3 || sounds[call.soundKey].instances[sounds[call.soundKey].instances.Count - 3].source.time > 0.1F) {	//Each sound has a 0.1 second cooldown, with two "ammunition."

				AudioSource newSource = sounds[call.soundKey].RandomSource();				//Grab a random source from that sound's sources.

				if (newSource.loop) { return AddLoopInstance (call, producingObject, newSource); }		//If the sound loops, add it as a loop...
				else { return AddOneShotInstance(call, producingObject, newSource); }					//...otherwise, add it as a one-shot.

			}
		}
        return null;
	}

	//Add Loop Instance
		/* Each gameObject can have only one instance of a looping sound.
		 * When any gameObject calls for a looping sound to be instanced,
		 * SoundManager will check to see if that object already has one
		 * of those loops going. If so, it will just prolong the existing
		 * loop instead of creating a new one. */
	private SoundInstance AddLoopInstance (SoundCall call, GameObject producingObject, AudioSource newSource) {
		bool matchFound = false;
		foreach (SoundInstance si in sounds[call.soundKey].instances) {
			if (si.producingObject == producingObject) {		//This means they share both the sound key and also the same object.
				matchFound = true;								//Therefore, a match has been found.

				si.UpdateLoop (newSource, producingObject, call);
                return si;

			}
		}
		if (!matchFound) {										//However, if no match was found...
			SoundInstance newSoundInstance = new SoundInstance(newSource, producingObject, call);	//Create a new sound instance...
			newSoundInstance.source.playOnAwake = true;
			newSoundInstance.source.loop = true;
			sounds[call.soundKey].instances.Add (newSoundInstance);
			newSoundInstance.source.Play ();					//...And make it play.

			if (call.autoFades) {autoFadeGroup.Add (newSoundInstance);}
            return newSoundInstance;
		}
        return null;
	}

	//AddOneShotInstance
		/* Create an instance of a sound. SoundManager will then watch
		 * that sound and delete it when it is finished. */
	private SoundInstance AddOneShotInstance (SoundCall call, GameObject producingObject, AudioSource newSource) {
		SoundInstance newSoundInstance = new SoundInstance(newSource, producingObject, call);
		sounds[call.soundKey].instances.Add (newSoundInstance);
		newSoundInstance.source.Play ();
        return newSoundInstance;
	}

	private void CleanupSweep () {

		sweepTimer += Time.unscaledDeltaTime;
		if (sweepTimer > sweepTime) {CheckForInstanceDeletion();}

	}

	private void CheckForInstanceDeletion () {
		foreach (KeyValuePair<string, SoundGroup> kvp in sounds) {
			if (kvp.Value.instances.Count > 0) {
				List<SoundInstance> instancesToRemove = new List<SoundInstance>();
				foreach (SoundInstance si in kvp.Value.instances) {
					if (si.source.loop && si.endTime <= Time.time || !si.source.isPlaying) {
						instancesToRemove.Add (si);
					}
				}
				foreach (SoundInstance si in instancesToRemove) {
					Destroy(si.source);
					kvp.Value.instances.Remove (si);
					if (autoFadeGroup.Contains (si)) {autoFadeGroup.Remove (si);}
				}
			}
		}
	}

	private void FadeLoops () {
		foreach (SoundInstance si in autoFadeGroup) {
			si.AutoFade();
			if (si.source.volume == 0F) {
				si.endTime = Time.time;
                si.source.time = 0F;
			}
		}
	}
}

public class SoundGroup {
    public List<AudioSource> sources;
    public List<SoundInstance> instances;

    public AudioSource RandomSource () {
        if (sources.Count > 0) {
            return sources[Random.Range(0, sources.Count)];
        } else {
            return null;
        }
    }

    public SoundGroup () {
        sources = new List<AudioSource>();
        instances = new List<SoundInstance>();

    }
}

public class SoundInstance {
	
	public float endTime;
	public AudioSource source;
	public float defaultVolume;
	public GameObject producingObject;
	public float fadeRate = 1.25F;
    public float basePitch = 1F;
	
	public string loopName = "";
	
	public SoundInstance (AudioSource newAudioSource, GameObject newGameObject, SoundCall newCall) {
		//source = newGameObject.AddComponent<AudioSource>();
		source = SoundManager.soundManagerObject.AddComponent<AudioSource>();
		source.clip = newAudioSource.clip;
		source.playOnAwake = false;
		source.loop = false;
		source.priority = newCall.priority;
		source.volume = newAudioSource.volume * newCall.volume;
		source.pitch = newAudioSource.pitch * newCall.pitch * (1F + Random.Range (-newCall.pitchVariation, newCall.pitchVariation));
        basePitch = source.pitch;
		source.panStereo = newAudioSource.panStereo;
		source.outputAudioMixerGroup = newAudioSource.outputAudioMixerGroup;
        
		float newPanLevel = 0f;
		source.panStereo = newPanLevel;

		producingObject = newGameObject;
		
		defaultVolume = source.volume;
		fadeRate = newCall.autoFadeRate;
		if(source.clip != null) {
			endTime = Time.time + source.clip.length;
		}
	}

	public SoundInstance (AudioSource newAudioSource, GameObject newGameObject) {
		//source = newGameObject.AddComponent<AudioSource>();
		source = SoundManager.soundManagerObject.AddComponent<AudioSource>();
		source.clip = newAudioSource.clip;
		source.playOnAwake = false;
		source.loop = false;
		source.priority = newAudioSource.priority;
		source.volume = newAudioSource.volume;
		source.pitch = newAudioSource.pitch;
        basePitch = source.pitch;
        source.panStereo = newAudioSource.panStereo;
		source.outputAudioMixerGroup = newAudioSource.outputAudioMixerGroup;
		
		float newPanLevel = 0f;
		source.panStereo = newPanLevel;
		
		producingObject = newGameObject;
		
		defaultVolume = source.volume;
		endTime = Time.time + source.clip.length;
	}

	public void UpdateLoop (AudioSource newAudioSource, GameObject newGameObject, SoundCall newSoundCall) {
		endTime = Time.time + newAudioSource.clip.length;		//Extend the duration of the playtime.
		
		source.volume = newAudioSource.volume * newSoundCall.volume;
		source.pitch = newAudioSource.pitch * newSoundCall.pitch * (1F + Random.Range (-newSoundCall.pitchVariation, newSoundCall.pitchVariation));

		float newPanLevel = 0f;
		source.panStereo = newPanLevel;
	}

	public void AutoFade () {
		if (source.volume > 0.01F) {source.volume *= (1F - fadeRate * Time.deltaTime);}
		else {source.volume = 0F;}
	}
}

[System.Serializable]
public class SoundCall {

	[Header ("Identifier (only one required)")]
	public string soundKey;
	public GameObject soundUsed;

	[Header ("Attributes")]
	[Range (0, 256)] public int priority = 128;
	[Range (0F, 1F)] public float volume = 1F;
	[Range (-3F, 3F)] public float pitch = 1F;
	[Range (0F, 0.25F)] public float pitchVariation = 0.05F;
	[Range (0F, 100F)] public float autoFadeRate = 1.25F;
	public bool autoFades = false;

	public SoundCall (string newSoundKey, int newPriority, float newVolume, float newPitch, float newPitchVariation) {
		soundKey = newSoundKey;
		priority = newPriority;
		volume = newVolume;
		pitch = newPitch;
		pitchVariation = newPitchVariation;
	}

}