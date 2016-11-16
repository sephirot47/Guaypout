using UnityEngine;
using System.Collections;

public class PlayerMoveController : MonoBehaviour 
{
    [Header ("Speed")]
    public Vector3 maxSpeed;
    public Vector3 speedFadeout;
    public Vector3 acceleration;
    private Vector3 speed = Vector3.zero;

    [Header ("Boost")]
    public float boostFadeout;
    private float currentBoost;

    [Header ("Floating")]
    public float floatingHeight;
    public float floatingAmplitude;
    public float floatingSpeed;

    [Header ("In Air")]
    public float airHorizontalMoveForce;

    [Header ("References")]
    public TrackInformer trackInformer;
    private Rigidbody rb;

	void Start () 
    {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () 
    {
        // Get info about the track at the player position
        TrackInformer.TrackInfo trackInfo = GetTrackInfo();

        // Do some physics depending on whether the ship is over the track or not
        if (trackInfo.overTheTrack)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;   // Reset rigidbody's velocity first
            currentBoost = Mathf.Max(currentBoost * boostFadeout, 1.0f); // Fadeout boost

            HandleForwardMovement(trackInfo);
            HandleHorizontalMovement(trackInfo);
            HandleFloating(trackInfo); // Handle the floating over the track
        }
        else
        {
            HandleAirMovement(trackInfo);
            rb.useGravity = true; // When we are not over the track, apply gravity!
        }
	}

    private void HandleForwardMovement(TrackInformer.TrackInfo trackInfo)
    {
        float movForwardSign = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);
        speed.z += movForwardSign * acceleration.z;
        speed.z = Mathf.Clamp(speed.z, -maxSpeed.z, maxSpeed.z);
        if (movForwardSign == 0.0f)
        {
            speed.z *= speedFadeout.z; 
        }
        speed.z *= currentBoost;
        rb.velocity += transform.forward * speed.z;
    }

    private void HandleHorizontalMovement(TrackInformer.TrackInfo trackInfo)
    {
        float movRightSign = Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0);
        speed.x += movRightSign * acceleration.x;
        speed.x = Mathf.Clamp(speed.x, -maxSpeed.x, maxSpeed.x);

        if (movRightSign == 0.0f)
        {
            speed.x *= speedFadeout.x; 
        }
        rb.velocity += transform.right * speed.x;
    }

    private void HandleFloating(TrackInformer.TrackInfo trackInfo)
    {
        Vector3 desiredFloatingPoint = trackInfo.groundPoint;
        desiredFloatingPoint += transform.up * floatingHeight;

        // Floating velocity
        desiredFloatingPoint += transform.up * Mathf.Sin(Time.time * floatingSpeed) * floatingAmplitude;

        // Keep the ship sticked to the ground
        rb.velocity += (desiredFloatingPoint - transform.position);

        //Debug.DrawLine(desiredFloatingPoint, transform.position, Color.cyan, 0.0f, false);
    }

    private void HandleAirMovement(TrackInformer.TrackInfo trackInfo)
    {
        float movRightSign = Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0);

        rb.AddForce(transform.right * movRightSign * airHorizontalMoveForce);
    }

    public void ApplyBoost(float boostAmount)
    {
        currentBoost = Mathf.Max(currentBoost, boostAmount);
    }

    public TrackInformer.TrackInfo GetTrackInfo()
    {
        TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo(
            transform.position,                           // From current position
            transform.right, transform.up,                // My right and up because it needs them
            (floatingHeight + floatingAmplitude) * 3.0f   // In this radius
        ); 

        return trackInfo;
    }
}
