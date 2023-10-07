using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP
{
    public class AnimHandler : MonoBehaviour
    {
        public Animator anim;
        int vertical;
        int horizontal;
        public bool canRotate = true;
        public float vv;
        public float hh;



        public void Initialize()
        {
            anim.GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
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


            vv = v;
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

            hh = h;

            #endregion

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
    }
}