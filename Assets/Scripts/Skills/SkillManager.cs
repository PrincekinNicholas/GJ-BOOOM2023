using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public float skillTime = 2f;
    public GameObject LightningPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Lightning", 0, skillTime);    //ÿ��2���ظ����ã������϶���������ѡ��
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Lightning()
    {
        Instantiate(LightningPrefab);
        //Instantiate(LightningPrefab, this.gameObject.transform);
    }
}
