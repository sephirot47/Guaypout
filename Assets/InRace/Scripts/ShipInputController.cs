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
    protected WeaponController weaponController;

    public void Start()
    {
        shipPhysicsController = GetComponent<ShipPhysicsController>();
        rb = shipPhysicsController.GetRigidbody();
        weaponController = GetComponent<WeaponController>();
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
    }

    public void OnHit(GameObject originShip)
    {
        if (weaponController.ShieldEnabled()) return;

        currentState = State.Hit;
        shipPhysicsController.SetTurn(0.0f);
        shipPhysicsController.SetThrust(0.0f);
        GetComponent<ShipSoundManager>().OnDamaged();
        originShip.GetComponent<ShipSoundManager>().OnHit();
        GetComponentInChildren<Animator>().SetTrigger("Hit");
    }

    public void SetState(State state)
    {
        currentState = state;
    }
}
