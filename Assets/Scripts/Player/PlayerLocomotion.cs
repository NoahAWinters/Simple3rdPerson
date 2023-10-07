using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform _cameraObject;
        InputHandler _inputHandler;
        Vector3 _moveDirection;

        [HideInInspector] public Transform myTransform;
        [HideInInspector] public AnimHandler animHandler;

        public new Rigidbody rb;
        public GameObject normalCamera;
        public GameObject lockCamera;



        [Header("Stats")]
        [SerializeField] float _movementSpeed = 5;
        [SerializeField] float _rotationSpeed = 10;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            _cameraObject = Camera.main.transform;
            animHandler = GetComponentInChildren<AnimHandler>();
            myTransform = transform;


            animHandler.Initialize();
        }


        void Update()
        {
            float delta = Time.deltaTime;

            _inputHandler.TickInput(delta);
            _moveDirection = _cameraObject.forward * _inputHandler.vertical;
            _moveDirection += _cameraObject.right * _inputHandler.horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            float speed = _movementSpeed;
            _moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, normalVector);
            rb.velocity = projectedVelocity;


            animHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0);
            if (animHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = _inputHandler.moveAmount;

            targetDir = _cameraObject.forward * _inputHandler.vertical;
            targetDir += _cameraObject.right * _inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = myTransform.forward;

            float rs = _rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }
        #endregion
    }
}
