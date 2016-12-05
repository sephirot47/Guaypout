using UnityEngine;
using System.Collections;

public class ShipInputController : MonoBehaviour
{
    public enum State
    {
        Moving,
        Hit
    };

    protected State currentState = State.Moving;

    protected Rigidbody rb;
    protected ShipPhysicsController shipPhysicsController;

    public float hitDisableTime;
    private float timeFromLastHit;

    public void Start()
    {
        shipPhysicsController = GetComponent<ShipPhysicsController>();
        rb = shipPhysicsController.GetRigidbody();
    }

    public void Update()
    {
        if (currentState == State.Hit)
        {
            timeFromLastHit += Time.deltaTime;
        }

        if (timeFromLastHit > hitDisableTime)
        {
            timeFromLastHit = 0.0f;
            currentState = State.Moving;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            OnHit();
        }
    }

    public void OnHit()
    {
        currentState = State.Hit;
        shipPhysicsController.SetTurn(0.0f);
        shipPhysicsController.SetThrust(0.0f);
        GetComponentInChildren<Animator>().SetTrigger("Hit");
    }

    public void SetState(State state)
    {
        currentState = state;
    }
}
