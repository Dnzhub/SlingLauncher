using UnityEngine;


public class Bomb : MonoBehaviour
{
    private Explosion explosion;
  

    private void Awake()
    {
        explosion = GetComponent<Explosion>();     
    }
  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Character"))
        {
        
            explosion.InitializeExplosion();
        }
    }

    
}
