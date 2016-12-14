using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Projectile projectile;
    public Mine mine;
    public float fireTime = 5f;
    public float shieldTime = 5f;

    private Rigidbody rb;
    
    private bool fireEnabled = false;
    private Timer timer;
	private WeaponTimerBarController bar;

    private bool mineEnabled = false;

    private bool shieldEnabled = false;
    private Timer shieldTimer;
    private ParticleSystem ps;
    private Light shieldLight;
    private ItemOrb lastPickedOrb = null;

    public AudioClip shieldAudioClip;
    private ItemFrameController itemFrameController;

	void Start () 
    {
        rb = GetComponent<Rigidbody>();
        timer = gameObject.AddComponent<Timer>();
        shieldTimer = gameObject.AddComponent<Timer>();

        ps = transform.FindChild("Shield").gameObject.GetComponentInChildren<ParticleSystem>();
        shieldLight = transform.FindChild("Shield").gameObject.GetComponentInChildren<Light>();

        itemFrameController = GameObject.Find("HUD_InGame/ItemFrame").GetComponent<ItemFrameController>();

        DisableShield();
	}
	
	void Update () 
    {
        if (timer.Ended() && FireEnabled()) DisableFire();
        if (shieldTimer.Ended() && ShieldEnabled()) DisableShield();
	}

    public bool CanPickWeapon()
    {
        return !shieldEnabled && !mineEnabled && !fireEnabled && !itemFrameController.IsScrambling(); 
    }

    public void OnOrbPicked(ItemOrb pickedOrb)
    {
        if (lastPickedOrb != null)
        {
            Destroy(lastPickedOrb.gameObject);
        }
        lastPickedOrb = pickedOrb;

        DisableFire();
        DisableMine();
        DisableShield();
    }

    public void EnableFire()
    {
        fireEnabled = true;
        timer.Set(fireTime);
        if (tag == "Player")
        {
			bar = GameObject.Find ("WeaponTimerBar").GetComponent<WeaponTimerBarController> ();
            bar.SetTimer(timer);
        }
    }

    public void DisableFire()
    {
        fireEnabled = false;
        GameObject.Find ("WeaponTimerBar").GetComponent<WeaponTimerBarController>().SetEnabled(false);
        if (tag == "Player") { itemFrameController.RemoveItemIcon(); } 
    }

	public bool FireEnabled() 
    {
		return fireEnabled;
	}

    public void FireProjectile()
    {
        if (!fireEnabled) return;

        Vector3 spawn = transform.position + transform.forward * 1.5f;
        Projectile proj = Instantiate(projectile, spawn, Quaternion.identity) as Projectile;
        proj.originShip = gameObject;
        proj.transform.forward = transform.forward;
        proj.GetComponent<Rigidbody>().velocity = rb.velocity;
    }

    public void EnableMine()
    {
        mineEnabled = true;
    }

    public void DisableMine()
    {
        mineEnabled = false;
        if (tag == "Player") { itemFrameController.RemoveItemIcon(); }
    }

	public bool MineEnabled() 
    {
		return mineEnabled;
	}

	public void ThrowMine(Vector3 direction)
    {
        if (!mineEnabled) return;

        Vector3 spawn = transform.position + transform.forward * 1.5f;
        Mine m = Instantiate(mine, spawn, Quaternion.identity) as Mine;
        m.originShip = gameObject;
        m.transform.forward = transform.forward;
        m.GetComponent<Rigidbody>().velocity = rb.velocity;
		m.direction = direction;
        mineEnabled = false;
    }

    public void EnableShield()
    {
        shieldEnabled = true;
        shieldTimer.Set(shieldTime);
        ps.gameObject.SetActive(true);
        shieldLight.gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(shieldAudioClip, transform.position);
    }

    public void DisableShield()
    {
        shieldEnabled = false;
        ps.gameObject.SetActive(false);
        shieldLight.gameObject.SetActive(false);
        if (tag == "Player") { itemFrameController.RemoveItemIcon(); }
    }

    public bool ShieldEnabled()
    {
        return shieldEnabled;
    }
}
