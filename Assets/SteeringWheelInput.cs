using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheelInput : MonoBehaviour
{
    public float steeringValue;
    public float maxAngle = 90f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private Transform grabbingHand;
    private Quaternion initialHandRot;
    private float initialWheelAngle;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void Update()
    {
        if (grabbingHand)
        {
            Quaternion handRot = grabbingHand.rotation;
            float angleOffset = Quaternion.Angle(initialHandRot, handRot);

            Vector3 cross = Vector3.Cross(initialHandRot * Vector3.forward, handRot * Vector3.forward);
            float signedAngle = cross.x < 0 ? -angleOffset : angleOffset;

            float newAngle = Mathf.Clamp(initialWheelAngle + signedAngle, -maxAngle, maxAngle);

            transform.localRotation = Quaternion.Euler(0f, newAngle, 0f);

            steeringValue = newAngle / maxAngle;
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        grabbingHand = args.interactorObject.transform;
        initialHandRot = grabbingHand.rotation;

        float x = transform.localEulerAngles.x;
        initialWheelAngle = x > 180f ? x - 360f : x;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        grabbingHand = null;
    }
}
