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
        TrackInformer.TrackInfo trackInfo = GetComponent<PlayerMoveController>().GetTrackInfo();
        if (trackInfo.overTheTrack)
        {
            Quaternion rotEnd = Quaternion.identity;

            Quaternion alignWithTrack = Quaternion.LookRotation(Vector3.Cross(transform.right, trackInfo.normal), trackInfo.normal);
            rotEnd *= alignWithTrack;

            float horiVelocity = Vector3.Dot(rb.velocity, transform.right);
            Quaternion tiltRot = Quaternion.AngleAxis(horiVelocity * tilt, transform.forward);
            rotEnd *= tiltRot;

            Quaternion movementRot = Quaternion.AngleAxis(horiVelocity * directionRotationSpeed, transform.up);
            rotEnd *= movementRot;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotEnd, rotationConvergenceSpeed * Time.deltaTime);
        }
    }
}
