using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    [SerializeField] AudioClip swordHit;
    [SerializeField] AudioClip enemyArrowShot;
    [SerializeField] AudioClip[] shieldBlock;
    [SerializeField] AudioClip weaponPickUp;
    [SerializeField] AudioClip enemyDeath;
    [SerializeField] AudioClip respawnPlayer;
    [SerializeField] AudioClip healthBonus;
    [SerializeField] AudioClip archerDead;
    [SerializeField] AudioClip[] playerHurt;
    [SerializeField] AudioClip playerArrowHit;
    [SerializeField] AudioClip playerArrowLaunch;
    float defaultLevel = 0.5f;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioManager = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AudioEnemyArrowShot()
    {
        audioSource.PlayOneShot(enemyArrowShot, 1f);
    }
    public void AudioArcherDead()
    {
        audioSource.PlayOneShot(archerDead, 1f);
    }
    public void AudioPlayerHurt()
    {
        audioSource.PlayOneShot(playerHurt[0], 1f);
    }
    public void AudioPlayerArrowHit()
    {
        audioSource.PlayOneShot(playerArrowHit, 1f);
    }
    public void AudioPlayerArrowLaunch()
    {
        audioSource.PlayOneShot(playerArrowLaunch, 1f);
    }
    public void AudioSwordHit()
    {
        audioSource.PlayOneShot(swordHit, 1f);
    }
    public void AudioShieldBlock()
    {
        audioSource.PlayOneShot(shieldBlock[0], 1f);
    }
    public void AudioWeaponPickUp()
    {
        audioSource.PlayOneShot(weaponPickUp, 1f);
    }
    public void AudioHealthBonus()
    {
        audioSource.PlayOneShot(healthBonus, 1f);
    }
    public void AudioEnemyDeath()
    {
        audioSource.PlayOneShot(enemyDeath, 1f);
    }
    public void AudioRespawnPlayer()
    {
        audioSource.PlayOneShot(respawnPlayer, 1f);
    }
}
