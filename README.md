# BALL LAUNCHER by Axlope

Experience dynamic physics interactions, touch-based controls, and an engaging game loop. Explore features like drag-and-release mechanics, continuous ball spawning, and device-friendly design for various resolutions.

## Gameplay Video
![](https://github.com/Axlope/ball-launcher/blob/main/Media/ball-launcher-gameplay.gif)



## Key Features:
- Dynamic ball responding to gravity
- Touchscreen input with coordinate tracking
- Drag-and-release mechanics for launching
- Continuous game loop with seamless ball spawning
- Device-friendly gameplay on various resolutions using Cinemachine

## Editor View

<img src="uk_logo.png" alt="en_logo" width="100"/>
<img src="tr_logo.png" alt="tr_logo" width="100"/>

# SCRIPTS

## BallHandler.cs
```csharp
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
```


