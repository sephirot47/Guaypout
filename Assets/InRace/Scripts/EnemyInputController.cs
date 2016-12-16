using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyInputController : ShipInputController
{
	public float turnSpeed = 1.5f;
    public float tilt = 30f;
    public float turnSmoothing = 5.0f;
    public float nextWPOrthogonalityTresh = 0.98f;
    public int numWPForward = 6;
    public float lane = 0;

	private TrackInformer trackInformer;
    private TrackInformer.TrackInfo info;
	private Timer timer;

    new void Start() 
    {
        base.Start();
		trackInformer = GameObject.Find("TrackInformer").GetComponent<TrackInformer>();
		timer = gameObject.AddComponent<Timer> ();
	}

    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);
        float angle = Vector3.Angle(dirA, dirB);
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }

	void Update() {
		// If a enemy has a mine, just throws it
		if (!timer.Enabled() && weaponController.MineEnabled ())
			timer.Set (0.5f);

		if (timer.Enabled() && timer.Ended()) {
			timer.Disable ();

			RaycastHit hitRight, hitLeft;
			Physics.Raycast (transform.position, transform.right, out hitRight, 100f);
			Physics.Raycast (transform.position, -transform.right, out hitLeft, 100f);
			int s = hitRight.distance < hitLeft.distance ? -1 : 1;

			Vector3 direction = transform.forward + transform.up;
			direction = Quaternion.Euler (0, s*Random.Range(20,60), 0) * direction;
			Debug.DrawRay(transform.position, direction * 10f, Color.magenta, 5f);
			weaponController.ThrowMine (direction);
		}

		// If any other ship is aligned with the vertical plane of the enemy's forward, fire!
        ShipPhysicsController[] otherShips = GameObject.FindObjectsOfType<ShipPhysicsController>();
        foreach (ShipPhysicsController ship in otherShips)
        {
            if (ship.gameObject == gameObject) continue;
            
            float dot = Vector3.Dot(transform.forward, (ship.transform.position - transform.position).normalized);
            if (dot > 0.9f)
            {
                if (Vector3.Distance(transform.position, ship.transform.position) < 500.0f)
                    weaponController.FireProjectile();
            }
        }/*
		RaycastHit hit;
		for (int a = -45; a <= 45; a += 3) {
			Vector3 rayDirection = Quaternion.Euler(a,0,0) * transform.forward;
            Debug.DrawRay(transform.position, rayDirection * 10f, Color.magenta, 0);
			if (Physics.Raycast (transform.position, rayDirection, out hit, 500f)) {
				string tag = hit.collider.transform.root.tag;
				if (tag == "Player" || tag == "Enemy")
					weaponController.FireProjectile ();
			}
		}
          */
	}

    void FixedUpdate()
    {
        base.Update();
        if (currentState == ShipInputController.State.Moving)
        {
            info = trackInformer.GetTrackInfo(transform.position, transform.right, transform.up, 100f);
            if (!info.overTheTrack) return;

            List<Waypoint> waypointsAfter = trackInformer.GetNPointsAfter(transform.position, numWPForward);
            if (waypointsAfter.Count < numWPForward) return;

            Waypoint targetAfterWP = waypointsAfter[numWPForward - 1];
            Waypoint targetAfterAfterWP = waypointsAfter[numWPForward - 2];
            Vector3 targetAfter = targetAfterWP.transform.position;
            Vector3 targetAfterAfter = targetAfterAfterWP.transform.position;
            Vector3 forwardedPosition = Vector3.Lerp(targetAfter, targetAfterAfter, 0.5f);

            if (Vector3.Distance(targetAfter, targetAfterAfter) < 0.5f)
            {
                targetAfterAfterWP = trackInformer.GetPointAfter(targetAfterAfterWP);
                targetAfterAfter = targetAfterAfterWP.transform.position;
            }

            Vector3 v1 = Vector3.ProjectOnPlane(targetAfterAfter - targetAfter, Vector3.up).normalized;
            Vector3 v2 = Vector3.ProjectOnPlane(targetAfter - forwardedPosition, Vector3.up).normalized;
            float nextWPOrthogonality = Vector3.Dot(v1, v2);

            Vector3 target = nextWPOrthogonality < nextWPOrthogonalityTresh ? targetAfterAfter : targetAfter;
            Waypoint targetWP = nextWPOrthogonality < nextWPOrthogonalityTresh ? targetAfterAfterWP : targetAfterWP;
            target += lane * targetWP.transform.right;
            Vector3 direction = target - transform.position;
            direction.y = transform.forward.y;
            direction.Normalize();
            //Debug.DrawLine(transform.position, transform.position + v1 * 5f, Color.red, 0.0f, false);
            //Debug.DrawLine(transform.position, transform.position + v2*5f, Color.green, 0.0f, false);
            Debug.DrawLine(transform.position, target, Color.blue, 0.0f, false);

            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.right, out hitInfo, 999.9f, shipPhysicsController.trackLayer))
                rb.AddForce(hitInfo.normal * 8.0f * (1.0f / hitInfo.distance));
            if (Physics.Raycast(transform.position, -transform.right, out hitInfo, 999.9f, shipPhysicsController.trackLayer))
                rb.AddForce(hitInfo.normal * 8.0f * (1.0f / hitInfo.distance));

            float prevTurn = shipPhysicsController.GetTurn();
            float s = Mathf.Sign(AngleAroundAxis(transform.forward, direction, transform.up));
            float endTurn = s * (1.0f - Vector3.Dot(direction, transform.forward)) * 15.0f;
            float turn = Mathf.Lerp(prevTurn, endTurn, Time.fixedDeltaTime * turnSmoothing);
            shipPhysicsController.SetTurn(turn);

            shipPhysicsController.SetThrust(1f);
        }
    }

    public void OnGoalPassed()
    {
        GetComponentInChildren<Animator>().SetTrigger("Boosting");
        this.enabled = false;
    }
}
