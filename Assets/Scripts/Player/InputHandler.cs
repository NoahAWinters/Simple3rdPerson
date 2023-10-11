using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        public bool rollFlag;
        public bool sprintFlag;

        public Vector2 movementInput;


        PlayerControls _inputActions;
        PlayerManager _manager; Vector2 _cameraInput;


        public bool b_input;
        public float b_inputTimer;
        public bool rb_input;
        public bool rt_input;
        public bool dpad_left;
        public bool dpad_up;
        public bool dpad_down;
        public bool dpad_right;

        void Start()
        {
            _manager = PlayerManager.GetInstance();
        }

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.Locomotion.Movement.performed += _inputActions => movementInput = _inputActions.ReadValue<Vector2>();
                _inputActions.Locomotion.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
            }
            _inputActions.Enable();
        }

        void OnDisable()
        {
            _inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            RollInput(delta);
        }

        void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }

        void RollInput(float delta)
        {
            b_input = _inputActions.Actions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (b_input)
            {
                b_inputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (b_inputTimer > 0 && b_inputTimer < .5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                b_inputTimer = 0;
            }
        }


    }
}