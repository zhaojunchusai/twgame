
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetParticleSortingLayer:MonoBehaviour
{
    public int sortingOrder = 21;

    void Start()
    {
        UpdateParticleSortingLayer(sortingOrder);
    }
    
    public void UpdateParticleSortingLayer(int newLayer) {
        sortingOrder = newLayer;
        if (transform == null) return;
        ParticleSystem[] childParticles = transform.GetComponentsInChildren<ParticleSystem>(true);
        if (childParticles != null && childParticles.Length > 0)
        {
            int leng = childParticles.Length;
            for (int i = 0; i < leng; i++)
            {
                childParticles[i].renderer.sortingOrder = sortingOrder;
            }
        }
        ParticleSystem[] selfParticles = transform.GetComponents<ParticleSystem>();
        if (selfParticles != null && selfParticles.Length > 0)
        {
            int leng = selfParticles.Length;
            for (int i = 0; i < leng; i++)
            {
                selfParticles[i].renderer.sortingOrder = sortingOrder;
            }
        }
    }

}
