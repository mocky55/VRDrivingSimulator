using UnityEngine;
using UnityEngine.InputSystem;


public class CarDrive : MonoBehaviour
{
    public SteeringWheelInput wheel;
    public InputActionProperty forwardInput;
    public InputActionProperty reverseInput;
    public float speed = 5f;
    public float turnSpeed = 30f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float forward = forwardInput.action.ReadValue<float>();
        float reverse = reverseInput.action.ReadValue<float>();
        float netThrottle = forward - reverse;

        Vector3 move = transform.forward * netThrottle * speed;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);

        if (Mathf.Abs(netThrottle) > 0.01f)
        {
            float turn = wheel.steeringValue * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
        }
    }
}

