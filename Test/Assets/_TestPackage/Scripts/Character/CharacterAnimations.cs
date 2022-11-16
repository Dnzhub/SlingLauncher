using DG.Tweening;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void ClimbToSling(Transform climbPoint)
    {
        transform.DOMove(climbPoint.position, 0.8f);

        animator.SetTrigger("climb");
    }
    public void FloatOnAir()
    {
        animator.SetTrigger("floating");
    }

    public float GetAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
