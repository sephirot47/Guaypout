using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NavSceneInRace : INavScene
{
    override public void Activate ()
    {
        SceneManager.LoadScene("InRace");
    }

    override public void Deactivate ()
    {
    }
}
