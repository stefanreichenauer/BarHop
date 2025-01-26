using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SplashAnimator : MonoBehaviour
{
    public GameObject effectParent = null;
    private ParticleSystem[] particleSystems = null;

    private void Awake()
    {
        particleSystems = effectParent.GetComponentsInChildren<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach(ParticleSystem p in particleSystems)
        {
                p.Play();
        }
    }
}
