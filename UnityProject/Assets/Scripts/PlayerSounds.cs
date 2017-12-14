using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour {

    private Animator anim;
	private HashIDs hash;               // Reference to the HashIDs.
	public float SoundTimer;
	public float PlaySoundTimer =  0.0f;
	public float PlaySoundTimerMin = 5.0f;
	public float PlaySoundTimerMax = 20.0f;
	private bool CarrySoundPlayed = false ;
	
	public AudioClip SoundIdle1;
	public AudioClip SoundIdle2;
	public AudioClip SoundIdle3;
	public AudioClip SoundIdle4;
	public AudioClip SoundIdle5;
	public AudioClip SoundIdle6;
	public AudioClip SoundIdle7;
	public AudioClip SoundIdle8;
	public AudioClip SoundIdle9;
	public AudioClip SoundIdle10;
	
	void Awake () 
	{
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
	}

	void PlaySound(string sound, float length)
	{
		if(SoundTimer == 0)
		{
			var rand = Random.value;
			switch(sound)
			{
				case("Idle"):
				if(rand >= 0 && rand < 0.1)
				{
					SoundTimer = SoundIdle1.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle1);
				}
				if(rand >= 0.1 && rand < 0.2)
				{
					SoundTimer = SoundIdle2.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle2);
				}
				if(rand >= 0.2 && rand < 0.3)
				{
					SoundTimer = SoundIdle3.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle3);
				}	
				if(rand >= 0.3 && rand < 0.4)
				{
					SoundTimer = SoundIdle4.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle4);
				}
				if(rand >= 0.4 && rand < 0.5)
				{
					SoundTimer = SoundIdle5.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle5);
				}
				if(rand >= 0.5 && rand < 0.6)
				{
					SoundTimer = SoundIdle6.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle6);
				}	
				if(rand >= 0.6 && rand < 0.7)
				{
					SoundTimer = SoundIdle7.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle7);
				}
				if(rand >= 0.7 && rand < 0.8)
				{
					SoundTimer = SoundIdle8.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle8);
				}
				if(rand >= 0.8 && rand < 0.9)
				{
					SoundTimer = SoundIdle9.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle9);
				}
				if(rand >= 0.9 && rand < 1.0)
				{
					SoundTimer = SoundIdle10.length;
					GetComponent<AudioSource>().PlayOneShot(SoundIdle10);
				}				
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		SoundTimer -= Time.deltaTime;
		if (SoundTimer < 0)
		{
			SoundTimer = 0;
		}

		PlaySoundTimer -= Time.deltaTime;
		if (PlaySoundTimer < 0)
		{
			PlaySoundTimer = 0;
		}

// play sound if idle
//		if (SoundTimer == 0 && PlaySoundTimer == 0)	
		if (anim.GetFloat(hash.speedFloat) == 0.0 && SoundTimer == 0 && PlaySoundTimer == 0)	
		{
			PlaySoundTimer = (Random.Range(PlaySoundTimerMin, PlaySoundTimerMax));
			PlaySound("Idle", 1.0f);
		}
		
// play sound if picked up
		if (anim.GetBool(hash.PlayerCarryObjectState) == true && SoundTimer == 0 && CarrySoundPlayed == false)	
		{
			CarrySoundPlayed = true;
			PlaySoundTimer = (Random.Range(PlaySoundTimerMin, PlaySoundTimerMax));
			PlaySound("Idle", 1.0f);
		}
		if (anim.GetBool(hash.PlayerCarryObjectState) == false)
		{
			CarrySoundPlayed = false;
		}
	}
}
