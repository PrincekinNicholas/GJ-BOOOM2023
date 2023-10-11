using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Dog : MonoBehaviour
{
    public float moveSpeed = 2f;

    private GameObject _player;
    private bool needFlip = false;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Flip();
    }

    void Move()
    {
        if (!needFlip)
        {
            transform.Translate((_player.transform.position - this.transform.position) * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate((_player.transform.position - this.transform.position) * -1 * moveSpeed * Time.deltaTime);
        }
        _animator.SetBool("run", true);
    }

    void Flip()
    {
        Vector3 pos = (_player.transform.position - this.transform.position);

        bool playerHasXAxisSpeed = Mathf.Abs(pos.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (pos.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                needFlip = false;
            }
            if (pos.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);  //人物翻转180度
                needFlip = true;
            }
        }
    }

}
