using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float radius = 5.0F;
    [SerializeField] private float power = 10.0F;
    [SerializeField] private ParticleSystem explosionEffect;

    public void InitializeExplosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            if(hit.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                ActivateExplosionEffect();
            }
          
        }
    }
    public void ActivateExplosionEffect()
    {
        if (!explosionEffect.isPlaying)
        {
            explosionEffect.Play();
        }
      
    }
}
