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

        Debug.DrawLine(transform.position, transform.position + rb.velocity * 1.0f, Color.cyan, 0.0f, true);
        Debug.DrawLine(transform.position, transform.position + horiVelocity * transform.right * 1.0f, Color.black, 0.0f, true);
        TrackInformer.TrackInfo trackInfo = GetComponent<PlayerMoveController>().GetTrackInfo();
        if (trackInfo.overTheTrack)
        {
            Quaternion rotEnd = Quaternion.identity;

            Vector3 newForward = Quaternion.FromToRotation(transform.up, trackInfo.normal) * transform.forward ;
            Quaternion alignWithTrack = Quaternion.LookRotation(newForward, trackInfo.normal);
            //rotEnd *= alignWithTrack;

            //Quaternion movementRot = Quaternion.AngleAxis(horiVelocity * directionRotationSpeed, transform.up);
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
            //transform.rotation *= Quaternion.LookRotation(rb.velocity, transform.up); //horiVelocity * Time.deltaTime, transform.forward);
        }
    }
}
