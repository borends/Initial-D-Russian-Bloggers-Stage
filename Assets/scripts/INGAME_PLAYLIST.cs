using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INGAME_PLAYLIST : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public int clipsToPlay = 3;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomClipsCoroutine());
    }

    IEnumerator PlayRandomClipsCoroutine()
    {
        List<AudioClip> clipsCopy = new List<AudioClip>(audioClips);

        for (int i = 0; i < clipsToPlay; i++)
        {
            if (clipsCopy.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, clipsCopy.Count);
            AudioClip clipToPlay = clipsCopy[randomIndex];
            clipsCopy.RemoveAt(randomIndex);

            audioSource.PlayOneShot(clipToPlay);
            yield return new WaitForSeconds(clipToPlay.length);
        }

        StartCoroutine(PlayRandomClipsCoroutine()); // Перезапускаем проигрывание аудиоклипов после окончания всех
    }
}