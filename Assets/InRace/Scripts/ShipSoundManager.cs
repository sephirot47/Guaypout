using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSoundManager : MonoBehaviour 
{
    [Range (0.0f, 1.0f)]
    public float boostAudioPlayProbability;
    [Range (0.0f, 1.0f)]
    public float damageAudioPlayProbability;
    [Range (0.0f, 1.0f)]
    public float hitAudioPlayProbability;
    [Range (0.0f, 1.0f)]
    public float weaponAudioPlayProbability;

    public List<AudioClip> boostAudioClips;
    public List<AudioClip> damageAudioClips;
    public List<AudioClip> hitAudioClips;
    public List<AudioClip> weaponAudioClips;

    private AudioSource audioSource;

	void Start () 
    {
        audioSource = GetComponent<AudioSource>();
	}
	
    public void OnBoost()
    {
        if (boostAudioClips.Count <= 0 || Random.Range(0.0f, 1.0f) > boostAudioPlayProbability)
            return;

        AudioClip clip = boostAudioClips[Random.Range(0, boostAudioClips.Count)];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnDamaged()
    {
        if (damageAudioClips.Count <= 0 || Random.Range(0.0f, 1.0f) > damageAudioPlayProbability)
            return;

        AudioClip clip = damageAudioClips[Random.Range(0, damageAudioClips.Count)];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnHit()
    {
        if (hitAudioClips.Count <= 0 || Random.Range(0.0f, 1.0f) > hitAudioPlayProbability)
            return;

        AudioClip clip = hitAudioClips[Random.Range(0, hitAudioClips.Count)];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnWeaponPicked()
    {
        if (hitAudioClips.Count <= 0 || Random.Range(0.0f, 1.0f) > weaponAudioPlayProbability)
            return;

        AudioClip clip = weaponAudioClips[Random.Range(0, weaponAudioClips.Count)];
        audioSource.clip = clip;
        audioSource.Play();
    }
}
