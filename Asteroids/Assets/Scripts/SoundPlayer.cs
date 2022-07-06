using System;
using System.Threading.Tasks;
using UnityEngine;

public class SoundPlayer
{
    public AudioSource audioSource { get; private set; }
    private bool canPlay = true;

    public SoundPlayer(GameObject creator)
    {
        audioSource = creator.AddComponent<AudioSource>();
    }    

    public void PlaySound(AudioClip[] audioClips, int timer = 0)
    {
        if (!audioSource.isPlaying && audioClips.Length != 0 && canPlay)
        {
            int audioIndex = UnityEngine.Random.Range(0, audioClips.Length);
            audioSource.PlayOneShot(audioClips[audioIndex]);

            WaitToPlayMusic(timer);
        }
    }

    public void PlaySound(AudioClip audioClip, int timer = 0)
    {
        if (!audioSource.isPlaying && audioClip != null)
        {            
            audioSource.PlayOneShot(audioClip);

            WaitToPlayMusic(timer);
        }        
    }

    private async void WaitToPlayMusic(float timer)
    {
        canPlay = false;
        await Task.Delay(TimeSpan.FromSeconds(timer));
        canPlay = true;
    }
}
