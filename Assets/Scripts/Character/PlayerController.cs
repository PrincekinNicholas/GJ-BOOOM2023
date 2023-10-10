using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum State
    {
        Normal,
        Rolling,
    }

    [Tooltip("移动速度")]
    public float moveSpeed;
    [Tooltip("翻滚速度,即会一瞬间冲出的初始速度")]
    public float setRollSpeed;
    [Tooltip("翻滚速度会按照一定倍数随时间衰减，直至正常速度")]
    public float rollSpeedDropMultiplier;
    [Tooltip("短闪距离")]
    public float teleportPower;
    [Tooltip("传入短闪特效的Prefab")]
    public GameObject teleportEffect;
    public bool lockMovement;
    public LayerMask dashLayerMask;
    [Tooltip("False表示技能为一定时间内随机触发并禁止按键触发，true表示按键触发")]
    public bool testSkillMode = true;
    public float skillTime = 2f;
    public GameObject LightningPrefab;
    public GameObject ObstaclePrefab;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector3 faceDir;
    private Vector3 moveDir;
    private Vector3 rollDir;
    private float rollSpeed;
    private bool isDashButtonDown;
    private RaycastHit raycastHit;
    private State state;
    private GameObject _player;
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _animator = GetComponentInChildren<Animator>();
        state = State.Normal;
        _player = this.gameObject;

        if (!testSkillMode)
        {
            InvokeRepeating("RandomSkill", skillTime, skillTime);    //每隔N秒重复调用
        }

    }

    // Update is called once per frame
    void Update()
    {
        KeyController();
        Flip();
    }

    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (_rigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (_rigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);  //人物翻转180度
            }
        }
    }

    void KeyController()
    {
        switch( state )
        {
            case State.Normal:
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                moveDir = new Vector3(horizontal, 0, vertical);

                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    isDashButtonDown = true;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if ( moveDir != Vector3.zero )
                    {
                        rollDir = moveDir;
                    }
                    else
                    {
                        rollDir = faceDir;
                    }
                    
                    rollSpeed = setRollSpeed;
                    state = State.Rolling;
                    _animator.SetBool("isRoll", true);
                }

                //技能测试
                if (testSkillMode)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        CreateObstacle();
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        Lightning();
                    }
                }
                

                break;
            case State.Rolling:
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 2f;
                if ( rollSpeed < rollSpeedMinimum )
                {
                    state = State.Normal;
                    _animator.SetBool("isRoll", false);
                }
                break;
        }
    }

    void FixedUpdate()
    {
        switch( state)
        {
            case State.Normal:
                Move();
                Teleport();
                break;
            case State.Rolling:
                Roll();
                break;
        }
    }

    void Roll()
    {
        //_rigidbody.velocity = new Vector3(moveDir.x * moveSpeed, _rigidbody.velocity.y, moveDir.z * moveSpeed);
        _rigidbody.velocity = rollDir * rollSpeed;
    }

    void Teleport()
    {
        if( isDashButtonDown )
        {
            Vector3 dashPosition = transform.position + moveDir * teleportPower;
            if ( moveDir != Vector3.zero )
            {
                faceDir = moveDir;
            }
            
            if (Physics.Raycast(transform.position, moveDir, out raycastHit, teleportPower, dashLayerMask))
            {
                dashPosition = raycastHit.point;
            }

            // Spawn visual effect
            DashEffect.CreateDashEffect(transform.position, moveDir, Vector3.Distance(transform.position, dashPosition), teleportEffect);

            _rigidbody.MovePosition(dashPosition);
            isDashButtonDown= false;
        }
    }

    void Move()
    {
        if (lockMovement)
            return;

        if (moveDir == Vector3.zero)
        {
            _animator.SetBool("isRun", false);
            return;
        }

        _rigidbody.velocity = new Vector3(moveDir.x * moveSpeed, _rigidbody.velocity.y, moveDir.z * moveSpeed);
        if (moveDir != Vector3.zero)
        {
            faceDir = moveDir;
        }

        _animator.SetBool("isRun", true);
    }

    public Vector3 GetFaceDir()
    {
        return faceDir;
    }

    /// -----------------------------------------------------------------------------------------
    /// 技能列表
    /// 1 - Lightning
    /// 2 - CreateObstacle
    /// -----------------------------------------------------------------------------------------

    void RandomSkill()
    {
        int skillIndex = Random.Range(1, 3); //左闭右开,技能增加的话要增加index上限
        switch( skillIndex)
        {
            case 1:
                Lightning();
                break;
            case 2:
                CreateObstacle();
                break;
        }
    }

    void CreateObstacle()
    {
        Vector3 pos = this.transform.position + faceDir;
        Instantiate(ObstaclePrefab, pos, Quaternion.identity);
    }

    void Lightning()
    {
        //Instantiate(LightningPrefab);
        Instantiate(LightningPrefab, Vector3.zero, Quaternion.identity);
    }
}
