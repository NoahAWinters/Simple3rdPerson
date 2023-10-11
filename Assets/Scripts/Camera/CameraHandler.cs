using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform pivotTransform;
        Vector3 cameraTransformPosition;
        Vector3 cameraPosition;
        LayerMask ignoreLayers;
        Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler _instance;

        [Header("CameraStats")]
        public float lookSpeed = .1f;
        public float followSpeed = .1f;
        public float pivotSpeed = .003f;
        public float minPivot = -35;
        public float maxPivot = 35;
        float defualtPosition;
        float targetPosition;
        float lookAngle;
        float pivotAngle;

        public float camSphereRadius = .2f;
        public float camCollisionOffset = .2f;
        public float minCollisionOffset = .2f;


        void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Found more than 1 CameraHandler in scene");
            }
            _instance = this;


            defualtPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }


        public void FollowTarget(float delta)
        {
            // Vector3 targetPos = Vector3.Lerp(transform.position, targetTransform.position, delta / followSpeed);
            Vector3 targetPos = Vector3.SmoothDamp(targetTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            transform.position = targetPos;
            HandleCameraCollisions(delta);
        }

        public void CameraRotation(float delta, float mouseX, float mouseY)
        {
            lookAngle += (mouseX * lookSpeed) / delta;
            pivotAngle -= (mouseY * lookSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRot = Quaternion.Euler(rotation);
            transform.rotation = targetRot;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRot = Quaternion.Euler(rotation);
            pivotTransform.localRotation = targetRot;
        }

        void HandleCameraCollisions(float delta)
        {
            targetPosition = defualtPosition;
            RaycastHit hit;
            Vector3 dir = (cameraTransform.position - targetTransform.position);
            dir.Normalize();

            if (Physics.SphereCast(pivotTransform.position, camSphereRadius, dir, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(pivotTransform.position, hit.point);
                targetPosition = -(dis - camCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minCollisionOffset)
            {
                targetPosition = -minCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public static CameraHandler GetInstance()
        {
            return _instance;
        }
    }
}
