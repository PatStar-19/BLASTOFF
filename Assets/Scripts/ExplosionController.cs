using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Subscribe to the animation event to handle the end of the animation
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = "OnAnimationEnd";
        animationEvent.time = animator.GetCurrentAnimatorStateInfo(0).length;

        AnimationClip animationClip = animator.runtimeAnimatorController.animationClips[0];
        animationClip.AddEvent(animationEvent);
    }

    // This function will be called by the animation event
    public void OnAnimationEnd()
    {
        // Destroy the GameObject after the animation completes
        Destroy(gameObject);
    }
}