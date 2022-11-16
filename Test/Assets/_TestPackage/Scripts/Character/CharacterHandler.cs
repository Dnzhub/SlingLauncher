using UnityEngine;
using DG.Tweening;


public class CharacterHandler : MonoBehaviour
{
    
    [SerializeField] private float selfDestroyTime = 5f;
    private bool isDead = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        SetRigidBodyKinematic(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Obstacle")) return;

        GetComponent<Explosion>().InitializeExplosion();
        SelfDestroy();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("LevelTrack"))
        {
            SelfDestroy();
        }
    }
    private void SelfDestroy()
    {
        isDead = true;
        transform.DOKill();
        Destroy(gameObject, selfDestroyTime);
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void SetRigidBodyKinematic(bool isKinematic)
    {
       rb.isKinematic = isKinematic;
    }

    public void ApplyGravity(bool isGravityActive)
    {
        rb.useGravity = isGravityActive;
    }
}
