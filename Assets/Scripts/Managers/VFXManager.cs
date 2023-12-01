using UnityEngine;

public class VFXManager : MonoBehaviour
{
    #region Singleton
    
    public static VFXManager Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    #endregion

    public ParticleSystem hitParticle;
    public ParticleSystem confettiParticle;
    public ParticleSystem barrierParticle;
    
    public void PlayHitEffect(ParticleSystem particle, Vector3 position)
    {
        ParticleSystem spawnedParticle = Instantiate(particle, position, Quaternion.identity);
        Destroy(spawnedParticle.gameObject, spawnedParticle.main.duration);
    }
}