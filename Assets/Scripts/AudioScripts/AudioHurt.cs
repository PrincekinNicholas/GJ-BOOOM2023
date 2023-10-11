using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHurt : MonoBehaviour
{
    public AK.Wwise.Event PlayerHurt;
    public void PlayerHurtSound()
    {
        PlayerHurt.Post(gameObject);
    }
}
