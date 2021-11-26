/*
StateSystem.cs and State.cs implement a Hierarchical Pushdown Automata.
It is a combination of  Hierarchical States and a Pushdown Automata
as described in Game Programming Patterns' Chapter II.7: State
http://gameprogrammingpatterns.com/state.html
*/

using UnityEngine;
public abstract class State : MonoBehaviour
{
    protected StateSystem stateSystem;

    [Header("State Settings")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected string[] animationSequence;
    enum LoopModeEnum{
        loopLastAnimation,
        loopSequence,
        stopLastFrame
    };
    [SerializeField] private LoopModeEnum loopMode = LoopModeEnum.loopLastAnimation;
    [SerializeField] private string soundEffect;
    [SerializeField] bool loopSound = false;
    private bool needToEndLoop = false;

    private int currentAnimation = 0;

    protected bool animationFinished;


    virtual public void Awake() {
        stateSystem = transform.parent.GetComponent<StateSystem>();
    }

    virtual public void enter()
    {
        enabled = true;
        currentAnimation = 0;
        if (animationSequence.Length > 0) {
            animator.Play(animationSequence[currentAnimation], -1, 0f);
            //Debug.Log(gameObject.name + "   Playing: " + animationSequence[currentAnimation]);
        }

        needToEndLoop = false;
        if (soundEffect.Length > 0) {
            bool notplayed = AudioManager.PlaySFX(soundEffect, loopSound);
            if (loopSound && !notplayed) {
                needToEndLoop = true;
            }
        }

        animationFinished = false;
    }
    virtual public void exit()
    {
        if (needToEndLoop) {
            //Debug.Log("Ending SFX: " + soundEffect);
            AudioManager.StopSFX(soundEffect);
        }
        enabled = false;
    }

    // IMPORTANT: False means keep processing the stateUpdate chain. True tells stateUpdate to break all the way up the chain.
    virtual public bool stateUpdate()
    {
        if (animationSequence.Length > 0 && isAnimationFinished()) // Current animation is finished, so go to next one if there is one
        {
            if (animationSequence.Length - 1 > currentAnimation) { // Go to next animation in sequence
                currentAnimation++;
                animator.Play(animationSequence[currentAnimation], -1, 0f);
            } else if (loopMode == LoopModeEnum.loopLastAnimation) { // manually loop so that normalizedTime doesn't get confused during state changes
                animator.Play(animationSequence[currentAnimation], -1, 0f);
            } else if (loopMode == LoopModeEnum.loopSequence) { // Loop the entire sequence
                currentAnimation = 0;
                animator.Play(animationSequence[currentAnimation], -1, 0f);
            } else if (loopMode == LoopModeEnum.stopLastFrame) {
                animator.StopPlayback();
                animationFinished = true;
            }
        } else if (animationSequence.Length == 0) {
            animationFinished = true;
        }
        return false;
    }

    protected bool isAnimationFinished() // Returns 1 when current animation has finished its first loop
    {
        if (animationSequence.Length > 0) {
            animationFinished = animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
        } else {
            animationFinished = true;
        }
        return animationFinished;
    }
}
