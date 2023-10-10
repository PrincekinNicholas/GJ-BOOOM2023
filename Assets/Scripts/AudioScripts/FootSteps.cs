using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AK.Wwise.Event FootstepSound;
    public void PlayPlayerFootstepSound()
    {
        FootstepSound.Post(gameObject);
    }
}
