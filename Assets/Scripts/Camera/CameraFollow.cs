using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        if ( smoothing == 0 )
        {
            smoothing = (float)0.1;
        }
    }

    void LateUpdate()
    {
        // player/target is die, destroy
        if ( target != null )
        {
            if ( transform.position != target.position)
            {
                Vector3 targetPos = target.position;
                transform.position = Vector3.Lerp( transform.position, targetPos, smoothing );
            }
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
