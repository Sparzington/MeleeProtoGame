using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //Sound(s)
    [SerializeField] private Sound SwordImpact;
    private AudioSource s;

    //Fighter
    private Fighter _fighter;
    
    private void Awake()
    {
        _fighter = GetComponent<Fighter>();
        _fighter.OnWeaponContact += _fighter_OnWeaponContact;

        if (SwordImpact != null)
        {
            s = gameObject.AddComponent<AudioSource>();
            s.clip = SwordImpact.clip;
        }
    }

    private void _fighter_OnWeaponContact(object sender, System.EventArgs e)
    {
        PlaySound(s);
    }

    private void InitSourde(AudioSource source, Sound newSound)
    {
        source.clip = newSound.clip;
        source.volume = newSound.Volume;
        source.loop = newSound.Loop;
        source.spatialBlend = newSound.SpatialBlend;
        source.maxDistance = newSound.MaxRange;
    }

    private void PlaySound(AudioSource source)
    {
        source.Play();
    }
}
