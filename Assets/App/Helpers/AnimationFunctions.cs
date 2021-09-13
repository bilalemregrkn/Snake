using System.Collections;
using UnityEngine;

namespace App.Helpers
{
    public class AnimationFunctions : MonoBehaviour
    {
        public void Play(Animator animator, string state, float delay = 0f)
        {
            StartCoroutine(_Play(animator, state, delay));
        }

        public IEnumerator _Play(Animator animator, string state, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            animator.Play(state);
        }

        public void ResumeAnimation(Animator myAnimator, string param, float speed)
        {
            AnimatorStateInfo animationState = myAnimator.GetCurrentAnimatorStateInfo(0);
            float time = animationState.normalizedTime;

            //Animation Limit Control
            time = time > 1 ? 1 : time;
            time = time < 0 ? 0 : time;

            myAnimator.Play(animationState.fullPathHash, -1, time);

            //Set Speed Param
            myAnimator.SetFloat(param, speed);
        }

        public void JumpToTime(Animator animator, float toTime)
        {
            string nameToCurrentAnimation = GetNameToCurrentAnimation(animator);
            animator.Play(nameToCurrentAnimation, 0, toTime);
        }

        private string GetNameToCurrentAnimation(Animator animator)
        {
            var nameAnimation = "";
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(clip.name))
                {
                    nameAnimation = clip.name;
                }
            }

            return nameAnimation;
        }
    }
}