using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class AnimHandler : MonoBehaviour
    {
        PlayerManager _pm;
        InputHandler _inputHandler;
        PlayerLocomotion _locomotion;


        public Animator anim;

        int vertical;
        int horizontal;
        public bool canRotate = true;



        public void Initialize()
        {
            _pm = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            _inputHandler = GetComponentInParent<InputHandler>();
            _locomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting = false)
        {
            #region Vertical

            float v = 0f;
            if (verticalMovement > 0 && verticalMovement < 0.55f)
                v = .5f;
            else if (verticalMovement > 0.55f)
                v = 1f;
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
                v = -.5f;
            else if (verticalMovement < -0.55f)
                v = -1f;

            #endregion

            #region Horizontal

            float h = 0f;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
                h = .5f;
            else if (horizontalMovement > 0.55f)
                h = 1f;
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
                h = -.5f;
            else if (horizontalMovement < -0.55f)
                h = -1f;

            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            if (h == 0 && v == 0)
                anim.SetBool("idle", true);
            else
                anim.SetBool("idle", false);
            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void SetRotate(bool check)
        {
            canRotate = check;
        }

        public void PlayTargetAnim(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, .2f);
        }

        void OnAnimatorMove()
        {
            if (_pm.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            _locomotion.rb.drag = 0;
            Vector3 deltaPos = anim.deltaPosition;
            deltaPos.y = 0;
            Vector3 vel = deltaPos / delta;
            _locomotion.rb.velocity = vel;
        }


        #region Animation Events

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }
        #endregion
    }
}