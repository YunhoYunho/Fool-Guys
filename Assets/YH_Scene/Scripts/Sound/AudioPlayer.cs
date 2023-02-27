using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    public List<AudioClip> clipList;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("PlayList");
    }

    private IEnumerator PlayList()
    {
        audioSource.clip = clipList[0];
        audioSource.Play();

        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            if (!audioSource.isPlaying)
            {
                audioSource.clip = clipList[1];
                audioSource.Play();
                audioSource.loop = true;
            }
        }
    }
}
