using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager _pm;
        Transform _cameraObject;
        InputHandler _inputHandler;
        public Vector3 moveDirection;

        [HideInInspector] public Transform myTransform;
        [HideInInspector] public AnimHandler animHandler;

        public Rigidbody rb;
        public GameObject normalCamera;
        public GameObject lockCamera;

        [Header("Raycast Stats")]
        [SerializeField] float groundDetectionRayStartPoint = .5f;
        [SerializeField] float minDistanceNeededForFall = 1f;
        [SerializeField] float groundDirRayDistance = .2f;
        LayerMask ignoreForGroundCheck;
        public float airTimer;

        [Space]
        [Header("Movement Stats")]
        [SerializeField] float _movementSpeed = 5f;
        [SerializeField] float _walkingSpeed = 3f;
        [SerializeField] float _sprintSpeed = 7f;
        [SerializeField] float _rotationSpeed = 10f;
        [SerializeField] float _fallSpeed = 45f;

        void Start()
        {
            _pm = GetComponent<PlayerManager>();

            rb = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            _cameraObject = Camera.main.transform;
            animHandler = GetComponentInChildren<AnimHandler>();
            myTransform = transform;


            animHandler.Initialize();

            _pm.isGrounded = true;
            _pm.isAirborne = false;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement
        public void HandleMove(float delta)
        {
            if (_inputHandler.rollFlag)
                return;
            if (_pm.isInteracting)
                return;

            moveDirection = _cameraObject.forward * _inputHandler.vertical;
            moveDirection += _cameraObject.right * _inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = _movementSpeed;

            if (_inputHandler.sprintFlag && _inputHandler.moveAmount > 0.5)
            {
                speed = _sprintSpeed;
                _pm.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (_inputHandler.moveAmount < .5f)
                    speed = _walkingSpeed;
                moveDirection *= speed;
                _pm.isSprinting = false;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rb.velocity = projectedVelocity;


            animHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _pm.isSprinting);
            if (animHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }


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

        public void HandleBInput(float delta)
        {
            if (animHandler.anim.GetBool("isInteracting"))
                return;


            if (_inputHandler.rollFlag)
            {
                Debug.Log("Got here");
                moveDirection = _cameraObject.forward * _inputHandler.vertical;
                moveDirection += _cameraObject.right * _inputHandler.horizontal;

                if (_inputHandler.moveAmount > 0)
                {
                    animHandler.PlayTargetAnim("Roll", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    animHandler.PlayTargetAnim("Backstep", true);
                    moveDirection.y = 0;
                }
            }
        }

        public void HandleFall(float delta, Vector3 moveDir)
        {
            _pm.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;


            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDir = Vector3.zero;
            }

            if (_pm.isAirborne)
            {
                rb.AddForce(-Vector3.up * _fallSpeed);
                rb.AddForce(moveDir * _fallSpeed / 7f);
            }

            Vector3 dir = moveDir;
            dir.Normalize();
            origin += (dir * groundDirRayDistance);

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minDistanceNeededForFall, Color.red, .1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minDistanceNeededForFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                _pm.isGrounded = true;
                targetPosition.y = tp.y;

                if (_pm.isAirborne)
                {
                    if (airTimer > .5f)
                    {
                        Debug.Log("You were in the air for " + airTimer + " seconds");
                        animHandler.PlayTargetAnim("Land", true);
                        airTimer = 0;
                    }
                    else
                    {
                        animHandler.PlayTargetAnim("Empty", false);
                        airTimer = 0;
                    }

                    _pm.isAirborne = false;
                }
            }
            else
            {
                if (_pm.isGrounded)
                {
                    _pm.isGrounded = false;
                }
                if (!_pm.isAirborne)
                {
                    if (!_pm.isInteracting)
                    {
                        animHandler.PlayTargetAnim("Fall", true);
                    }

                    Vector3 vel = rb.velocity;
                    vel.Normalize();
                    rb.velocity = vel * (_movementSpeed / 2);
                    _pm.isAirborne = true;
                }
            }

            if (_pm.isInteracting || _inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / .1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }

        }
        #endregion
    }
}
