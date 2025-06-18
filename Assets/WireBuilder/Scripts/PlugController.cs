using UnityEngine;
using UnityEngine.Events;
using Autohand;

public class PlugController : MonoBehaviour
{
    public bool isConected = false;
    public UnityEvent OnWirePlugged;
    public Transform plugPosition;

    [HideInInspector] public Transform endAnchor;
    [HideInInspector] public Rigidbody endAnchorRB;
    [HideInInspector] public WireController wireController;

    private Quaternion offset = Quaternion.Euler(90, 0, 0);
    private Grabbable grabbable;

    void Start()
    {
        grabbable = endAnchor.GetComponent<Grabbable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == endAnchor.gameObject && !isConected)
        {
            isConected = true;
            FixPlug();
            OnWirePlugged.Invoke();
        }
    }

    private void FixedUpdate()
{
    if (grabbable == null || endAnchorRB == null)
        return;

    bool isGrabbed = grabbable.IsHeld();

    if (isConected && !isGrabbed)
    {
        FixPlug();
    }
    else if (isConected && isGrabbed)
    {
        // Stecker wird gegriffen → sofort freigeben
        isConected = false;
        endAnchorRB.isKinematic = false;
        endAnchorRB.linearVelocity = Vector3.zero;
        endAnchorRB.angularVelocity = Vector3.zero;
    }
    // Debug: Status ausgeben
    Debug.Log($"isKinematic: {endAnchorRB.isKinematic}, isConected: {isConected}, isGrabbed: {isGrabbed}");
}


    private void FixPlug()
    {
        endAnchorRB.isKinematic = true;
        endAnchorRB.linearVelocity = Vector3.zero;
        endAnchorRB.angularVelocity = Vector3.zero;
        endAnchor.position = plugPosition.position;
        endAnchor.rotation = plugPosition.rotation * offset;
    }
}
