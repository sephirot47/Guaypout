using UnityEngine;
using System.Collections;

public class PlayerRotationController : MonoBehaviour 
{
    [Header ("Speed")]
    public float rotationConvergenceSpeed;
    public float tilt;
    public float directionRotationSpeed;

    [Header ("References")]
    public TrackInformer trackInformer;
    private Rigidbody rb;

    void Start () 
    {
        rb = GetComponent<Rigidbody> ();
    }

    void Update () 
    {
        float horiVelocity = Vector3.Dot(rb.velocity, transform.right);

        TrackInformer.TrackInfo trackInfo = GetComponent<PlayerMoveController>().GetTrackInfo();
        if (trackInfo.overTheTrack)
        {
            Quaternion rotEnd = Quaternion.identity;

            Quaternion movementRot = Quaternion.LookRotation(transform.forward + transform.right * horiVelocity * directionRotationSpeed, 
                                                             trackInfo.normal);
            Vector3 lookat = trackInfo.forward + transform.right * Vector3.Dot(transform.right, transform.right * horiVelocity * directionRotationSpeed);
            rotEnd = Quaternion.LookRotation(lookat, trackInfo.normal);

            Quaternion tiltRot = Quaternion.AngleAxis(horiVelocity * tilt, transform.forward);
            rotEnd *= tiltRot;

            Debug.DrawLine(transform.position, transform.position + transform.forward + transform.right * horiVelocity * 999.9f, Color.white, 0.0f, true);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotEnd, rotationConvergenceSpeed * Time.deltaTime);
        }
        else
        {
            Quaternion rotEnd = Quaternion.LookRotation(rb.velocity, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotEnd, rotationConvergenceSpeed * Time.deltaTime);
            //transform.rotation *= Quaternion.LookRotation(rb.velocity, transform.up); //horiVelocity * Time.deltaTime, transform.forward);
        }
    }
}
