using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDog : MonoBehaviour
{
    public AK.Wwise.Event Dog;
    public void PlayDogSound()
    {
        Dog.Post(gameObject);
    }
}
