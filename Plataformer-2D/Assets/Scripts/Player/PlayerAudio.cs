using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;

    //sons
    public AudioClip coinSound;
    public AudioClip jumpSound;
    public AudioClip attackSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Função responsavel por tocar o som
    public void PlaySFX(AudioClip sfx)
    {
        audioSource.PlayOneShot(sfx);
    }


}
