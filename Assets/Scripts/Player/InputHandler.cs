using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        PlayerControls _inputActions;
        CameraHandler _cameraHandler;
        Vector2 _movementInput;
        Vector2 _cameraInput;




        void Start()
        {
            _cameraHandler = CameraHandler.GetInstance();
        }

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.Player.Movement.performed += _inputActions => _movementInput = _inputActions.ReadValue<Vector2>();
                _inputActions.Player.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
            }

            _inputActions.Enable();
        }

        void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.CameraRotation(delta, mouseX, mouseY);
            }
        }

        void OnDisable()
        {
            _inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        void MoveInput(float delta)
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }


    }
}


