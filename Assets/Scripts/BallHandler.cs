using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YourNamespace // Replace "YourNamespace" with your desired namespace name
{
    public class BallHandler : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Rigidbody2D pivot;
        [SerializeField] private float DetachDelay;
        [SerializeField] private float respawnDelay;

        
        private Rigidbody2D currentBallRigidbody;
        private SpringJoint2D currentBallSprintJoint;

        private Camera mainCamera;
        private bool isDragging;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;

            SpawnNewBall();
        }

        // Update is called once per frame
        void Update()
        {
            if(currentBallRigidbody == null)
            {
                return;
            }

            if(!Touchscreen.current.primaryTouch.press.isPressed)
            {
                if(isDragging)
                {
                    LaunchBall();
                }

                isDragging = false;
                
                return;
            }

            isDragging = true;

            currentBallRigidbody.isKinematic = true;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            currentBallRigidbody.position = worldPosition;
        }

        private void SpawnNewBall()
        {
            GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

            currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
            currentBallSprintJoint = ballInstance.GetComponent<SpringJoint2D>();

            currentBallSprintJoint.connectedBody = pivot;

        }

        private void LaunchBall()
        {
            currentBallRigidbody.isKinematic = false;
            currentBallRigidbody = null;

            Invoke(nameof(DetachBall), DetachDelay);
        }

        private void DetachBall()
        {

            currentBallSprintJoint.enabled = false;
            currentBallSprintJoint = null;

            Invoke(nameof(SpawnNewBall), respawnDelay);
        }
    }
}