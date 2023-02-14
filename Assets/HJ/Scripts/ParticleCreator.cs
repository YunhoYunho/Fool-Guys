using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreator : MonoBehaviour
{
    [SerializeField] private ParticleSystem baseParticle;
    [SerializeField] private List<ParticleSystem> randParticles = new List<ParticleSystem>();

    private void LateUpdate()
    {
    }

    public void PlayParticle()
    {
        baseParticle.Play();
        int rand = Random.Range(0, randParticles.Count);

        randParticles[rand].Play();
    }
}
