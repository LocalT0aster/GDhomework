# Introduction to Game Development Homework 1

## by Danil Nesterov ([d.nesterov@innopolis.university](mailto:d.nesterov@innopolis.university))

The report source can be found at [GitHub Repo](https://github.com/LocalT0aster/GDhomework/blob/master/Reports/HW2.md).

## Summary

In this homework I have introduced a dedicated "Player" object (Capsule) along with three new scripts - `MouseLookX` for horizontal view control, `MouseLookY` for vertical view control (with rotation limits to prevent camera flipping), and `FPSInput` that now supports character movement with a sprint mode (doubling the speed when <kbd>L.Shift</kbd> is held) and jumping using <kbd>Space</kbd>. Additionally, it adds a fixed-axis rotation element - self-made, fully rigged, and animated "Oscilating Fan" and a more complex motion behavior is demonstrated with a bouncing, color-changing 3D DVD logo. Other updates include improvements to scene management (such as project structure and asset/material refinements) and quality-of-life enhancements like a CursorLock script for better user control and exit by <kbd>ESC</kbd> implementation.

## Source Code (Available at [GitHub](https://github.com/LocalT0aster/GDhomework))

### [1b MouseLookX.cs](https://github.com/LocalT0aster/GDhomework/blob/HW02/Assets/Scripts/MouseLookX.cs)

```cs
using UnityEngine;

public class MouseLookX : MonoBehaviour {
    public float Sensitivity {
        get => sensitivity;
        set => sensitivity = value;
    }
    [Range(0.1f, 10f)][SerializeField] float sensitivity = 3f;
    private const string mouseXAxis = "Mouse X";
    private float rotation = 0f;

    void Awake() {
        rotation = transform.localRotation.eulerAngles.y;
    }

    private void Update() {
        rotation += Input.GetAxis(mouseXAxis) * sensitivity;
        Quaternion xQuat = Quaternion.AngleAxis(rotation, Vector3.up);
        transform.localRotation = xQuat;
    }
}
```

### [1c MouseLookY.cs](https://github.com/LocalT0aster/GDhomework/blob/HW02/Assets/Scripts/MouseLookY.cs)

```cs
using UnityEngine;

public class MouseLookY : MonoBehaviour {
    public float Sensitivity {
        get => sensitivity;
        set => sensitivity = value;
    }
    [Range(0.1f, 10f)][SerializeField] float sensitivity = 3f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 89.9f;


    private const string mouseYAxis = "Mouse Y";
    private float rotation = 0f;

    private void Awake() {
        rotation = transform.localRotation.eulerAngles.x;
    }
    private void Update() {
        rotation += Input.GetAxis(mouseYAxis) * sensitivity;
        rotation = Mathf.Clamp(rotation, -yRotationLimit, yRotationLimit);
        Quaternion yQuat = Quaternion.AngleAxis(rotation, Vector3.left);
        transform.localRotation = yQuat;
    }
}
```

### [1d FPSInput.cs](https://github.com/LocalT0aster/GDhomework/blob/HW02/Assets/Scripts/FPSInput.cs)

```cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSInput : MonoBehaviour {
    public float Gravity = -9.80665f;
    [Header("Walking")]
    public float WalkingSpeed = 1f;
    public float RunningSpeed = 2f;
    public KeyCode RunKeyCode = KeyCode.LeftShift;
    // Runtime variables
    private float currentSpeed = 1f;
    private float deltaSpeed = 0f;
    private bool isSprinting = false;
    private CharacterController characterController;
    // Axis
    private const string horizontalAxis = "Horizontal";
    private const string verticalAxis = "Vertical";
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    [Header("Jumping")]
    public float JumpHeight = 4f;
    public float CoyoteJumpTime = 0.2f;
    public KeyCode JumpKeyCode = KeyCode.Space;
    private float groundedTimer = 0f;
    private float verticalVelocity = 0f;

    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        // XZ movement
        horizontalInput = Input.GetAxis(horizontalAxis);
        verticalInput = Input.GetAxis(verticalAxis);

        if (Input.GetKey(RunKeyCode)) {
            if (!isSprinting) {
                currentSpeed = RunningSpeed;
                isSprinting = true;
            }
        } else if (isSprinting) {
            currentSpeed = WalkingSpeed;
            isSprinting = false;
        }
        deltaSpeed = currentSpeed * Time.deltaTime;

        // Y movement
        if (characterController.isGrounded) {
            groundedTimer = CoyoteJumpTime;
            if (verticalVelocity < 0f) verticalVelocity = 0f; // slam into ground
        }
        if (groundedTimer > 0f) {
            groundedTimer -= Time.deltaTime;
        }

        verticalVelocity += Gravity * Time.deltaTime;

        if (Input.GetKey(JumpKeyCode) && groundedTimer > 0f) {
            groundedTimer = 0f;
            verticalVelocity += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
        // Final move
        characterController.Move(
            transform.TransformDirection(
                new Vector3(
                    horizontalInput * deltaSpeed,
                    verticalVelocity * Time.deltaTime,
                    verticalInput * deltaSpeed)));
    }
}
```

### [2 RotateByAxis.cs](https://github.com/LocalT0aster/GDhomework/blob/HW02/Assets/Scripts/RotateByAxis.cs)

```cs
using UnityEngine;

public class RotateByAxis : MonoBehaviour {
    public Vector3 RotateDegPerSecond = new(0f, 0f, 0f);
    void Update(){
        transform.Rotate(RotateDegPerSecond * Time.deltaTime);
    }
}
```

### [3 DVDLogo.cs](https://github.com/LocalT0aster/GDhomework/blob/HW02/Assets/Scripts/DVDLogo.cs)

```cs
using UnityEngine;

// The script is designed for cube inside the empty parent.
public class DVDLogo : MonoBehaviour {
    public Vector3 velocity = new Vector3(1f, 1f, 1f);
    public Vector3 Boundary = new Vector3(5f, 5f, 5f);
    public float MaxSpeed = 5f;
    [Tooltip("If default, calulates offset based on the primitive cube local scale.")]
    public Vector3 ShapeOffset = Vector3.zero;
    private new Renderer renderer;

    void Awake() {
        renderer = GetComponent<Renderer>();
        // Calculate offset based on the localScale.
        // This assumes the mesh is a unit cube (1 unit per side). If your mesh has a different size,
        // you might need to adjust the ShapeOffset accordingly.
        if (ShapeOffset == Vector3.zero)
            ShapeOffset = new(transform.localScale.x * 0.5f,
                              transform.localScale.y * 0.5f,
                              transform.localScale.z * 0.5f);
        RandomColor();
    }

    void Update() {
        Vector3 newPos = transform.localPosition + velocity * Time.deltaTime;

        // Check and handle bounce for the x-axis.
        if (newPos.x > (Boundary.x - ShapeOffset.x)) {
            newPos.x = Boundary.x - ShapeOffset.x;
            velocity.x = -1f * Mathf.Sign(velocity.x) * Random.Range(1f, MaxSpeed);
            RandomColor();
        } else if (newPos.x < -(Boundary.x - ShapeOffset.x)) {
            newPos.x = -Boundary.x + ShapeOffset.x;
            velocity.x = -1f * Mathf.Sign(velocity.x) * Random.Range(1f, MaxSpeed);
            RandomColor();
        }

        // Check and handle bounce for the y-axis.
        if (newPos.y > (Boundary.y - ShapeOffset.y)) {
            newPos.y = Boundary.y - ShapeOffset.y;
            velocity.y = -1f * Mathf.Sign(velocity.y) * Random.Range(1f, MaxSpeed);
            RandomColor();
        } else if (newPos.y < -(Boundary.y - ShapeOffset.y)) {
            newPos.y = -Boundary.y + ShapeOffset.y;
            velocity.y = -1f * Mathf.Sign(velocity.y) * Random.Range(1f, MaxSpeed);
            RandomColor();
        }

        // Check and handle bounce for the z-axis.
        if (newPos.z > (Boundary.z - ShapeOffset.z)) {
            newPos.z = Boundary.z - ShapeOffset.z;
            velocity.z = -1f * Mathf.Sign(velocity.z) * Random.Range(1f, MaxSpeed);
            RandomColor();
        } else if (newPos.z < -(Boundary.z - ShapeOffset.z)) {
            newPos.z = -Boundary.z + ShapeOffset.z;
            velocity.z = -1f * Mathf.Sign(velocity.z) * Random.Range(1f, MaxSpeed);
            RandomColor();
        }

        transform.localPosition = newPos;
    }

    void RandomColor() {
        // Random hue between 0 and 1, with full saturation and brightness.
        Color newColor = Color.HSVToRGB(Random.value, 1f, 1f);
        if (renderer != null) {
            renderer.material.color = newColor;
        }
    }
}
```

### [4 FPSInput.cs](https://github.com/LocalT0aster/GDhomework/blob/HW02/Assets/Scripts/FPSInput.cs)

[Look at 1d](#1d-fpsinputcs).

[Summary](#summary)

Video is available on Moodle.
