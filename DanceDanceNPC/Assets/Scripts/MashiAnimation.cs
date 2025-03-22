using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class MashiAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimatorController danceController;
    [SerializeField] private AnimatorController poseController;
    [SerializeField] private Transform vcamParent;
    [SerializeField] private float poseTransitionTime;
    [SerializeField] private float timeBetweenPose;
    private AnimationClip[] poseClips;
    private bool autoPose;
    private int activeCamIndex;

    void Start()
    {
        animator = GetComponent<Animator>();
        activeCamIndex = 0;
    }

    private void ChangeCamera(int index) {
        if (index >= vcamParent.childCount) index = 0;
        else if (index < 0) index = vcamParent.childCount - 1;

        vcamParent.GetChild(activeCamIndex).gameObject.SetActive(false);
        vcamParent.GetChild(index).gameObject.SetActive(true);
        activeCamIndex = index;
    }

    IEnumerator ChangePose()
    {
        while (autoPose)
        {
            yield return new WaitForSecondsRealtime(timeBetweenPose);
            PlayRandomPose();
        }
    }

    public void ResetAllAnimatorTriggers()
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name);
                
            }
        }
    }

    private void PlayRandomPose()
    {
        animator.CrossFadeInFixedTime(poseClips[Random.Range(0, poseClips.Length)].name, poseTransitionTime);
    }

    void Update()
    {
        // set animations
        if (Input.anyKeyDown)
        {
            ResetAllAnimatorTriggers();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("toIdle");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("toHouse");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("toHipHop");
        }
        //

        

        // set animation speed
        if (animator.runtimeAnimatorController == danceController)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                animator.speed += 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (animator.speed > 0.1f) animator.speed -= 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                animator.speed = 1f;
            } else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                animator.speed = 0;
            }
        } else if (animator.runtimeAnimatorController == poseController)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                timeBetweenPose += 0.05f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (timeBetweenPose > 0.1f) timeBetweenPose -= 0.05f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                timeBetweenPose = 0.5f;
            }
        }



        //

        // set camera 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeCamera(activeCamIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeCamera(activeCamIndex + 1);
        }
        //

        // toggle controller
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.runtimeAnimatorController = animator.runtimeAnimatorController == danceController ? poseController : danceController;
            if (animator.runtimeAnimatorController == danceController)
            {
                StopCoroutine(nameof(ChangePose));
                autoPose = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (animator.runtimeAnimatorController == poseController)
            {
                poseClips ??= animator.runtimeAnimatorController.animationClips;
                StopCoroutine(nameof(ChangePose));
                autoPose = false;
                PlayRandomPose();
            }
        }

        // toggle auto
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (animator.runtimeAnimatorController == poseController)
            {
                if (!autoPose)
                {
                    autoPose = true;
                    poseClips ??= animator.runtimeAnimatorController.animationClips;
                    StartCoroutine(nameof(ChangePose));
                } else
                {
                    autoPose = false;
                    StopCoroutine(nameof(ChangePose));
                }
            }
        }

    }
}
