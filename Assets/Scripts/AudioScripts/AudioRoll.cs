using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRoll : MonoBehaviour
{
    public AK.Wwise.Event PlayerRoll;
    public void PlayerRollSound()
    {
        PlayerRoll.Post(gameObject);
    }
}
