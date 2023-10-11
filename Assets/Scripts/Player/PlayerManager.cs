using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class PlayerManager : MonoBehaviour
    {
        static PlayerManager _instance;


        InputHandler _inputHandler;
        CameraHandler _cameraHandler;
        PlayerLocomotion _locomotion;
        Animator _anim;


        [Header("Flags")]
        public bool isInteracting;
        public bool isSprinting;
        public bool isAirborne;
        public bool isGrounded;
        public bool canDoCombo;
        public int comboIndex = 0;


        void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Found more than 1 PlayerManager in scene");
            }
            _instance = this;
        }


        void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _anim = GetComponentInChildren<Animator>();
            _cameraHandler = CameraHandler.GetInstance();
            _locomotion = GetComponent<PlayerLocomotion>();
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = _anim.GetBool("isInteracting");

            canDoCombo = _anim.GetBool("canDoCombo");

            _inputHandler.TickInput(delta);
            _locomotion.HandleMove(delta);
            _locomotion.HandleBInput(delta);
            _locomotion.HandleFall(delta, _locomotion.moveDirection);
        }

        void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;



            if (_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.CameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
            }
        }

        void LateUpdate()
        {
            _inputHandler.rollFlag = false;
            _inputHandler.sprintFlag = false;

            _inputHandler.rb_input = false;
            _inputHandler.rt_input = false;
            _inputHandler.dpad_left = false;
            _inputHandler.dpad_right = false;


            if (isAirborne)
            {
                _locomotion.airTimer += Time.deltaTime;
            }
        }



        public static PlayerManager GetInstance()
        {
            return _instance;
        }
    }
}