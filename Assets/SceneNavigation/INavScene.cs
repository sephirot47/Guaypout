using UnityEngine;
using System.Collections;

public class INavScene : MonoBehaviour
{
    public Transform GetNavigationPointTransform()
    {
        return transform.FindChild("NavPoint");
    }

    virtual public void Activate() {}
    virtual public void Deactivate() {}
}
