using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[ExecuteInEditMode] //加了这个之后可以在Editor运行
public class ThunderLightning : MonoBehaviour
{
    public float detail = 1;//增加后，线条数量会减少，每个线条会更长。  

    public float displacement = 15;//位移量，也就是线条数值方向偏移的最大值  

    //public Transform EndPostion;//链接目标  

    //public Transform StartPosition;

    public float yOffset = 0;

    private LineRenderer _lineRender;

    private List<Vector3> _linePosList;
    private GameObject _player;
    private Vector3 EndPosition;
    private Vector3 StartPosition;

    private void Awake()

    {
        //Transform parent = transform.parent;
        //if (parent != null )
        //{
        //    EndPostion = parent.transform;
        //    StartPosition = EndPostion;
        //    StartPosition.SetLocalPositionAndRotation(new Vector3(StartPosition.localPosition.x, StartPosition.localPosition.y + 5, StartPosition.localPosition.z), StartPosition.localRotation);
        //}
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            EndPosition = _player.transform.position;
            EndPosition.y = EndPosition.y - 1;
            Debug.Log("EndPosition:" + EndPosition);
            StartPosition = EndPosition + new Vector3(0, 5, 0);
            Debug.Log("StartPosition:" + StartPosition);
            //EndPostion = _player.transform;
            //StartPosition = EndPostion;
            //StartPosition.SetLocalPositionAndRotation(new Vector3(StartPosition.localPosition.x, StartPosition.localPosition.y + 5, StartPosition.localPosition.z), StartPosition.localRotation);
        }

        _lineRender = GetComponent<LineRenderer>();

        _linePosList = new List<Vector3>();

    }

    private void Update()

    {

        if (Time.timeScale != 0)

        {

            _linePosList.Clear();

            Vector3 startPos = Vector3.zero;

            Vector3 endPos = Vector3.zero;

            //if (EndPostion != null)

            //{

            //    endPos = EndPostion.position + Vector3.up * yOffset;

            //}

            //if (StartPosition != null)

            //{

            //    startPos = StartPosition.position + Vector3.up * yOffset;

            //}
            if (EndPosition != null)

            {

                endPos = EndPosition + Vector3.up * yOffset;

            }

            if (StartPosition != null)

            {

                startPos = StartPosition + Vector3.up * yOffset;

            }

            //获得开始点与结束点之间的随机生成点

            CollectLinPos(startPos, endPos, displacement);

            _linePosList.Add(endPos);

            //把点集合赋给LineRenderer

            _lineRender.SetVertexCount(_linePosList.Count);

            for (int i = 0, n = _linePosList.Count; i < n; i++)

            {

                _lineRender.SetPosition(i, _linePosList[i]);

            }

        }

    }

    //收集顶点，中点分形法插值抖动  

    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)

    {

        //递归结束的条件

        if (displace < detail)

        {

            _linePosList.Add(startPos);

        }

        else

        {

            float midX = (startPos.x + destPos.x) / 2;

            float midY = (startPos.y + destPos.y) / 2;

            float midZ = (startPos.z + destPos.z) / 2;

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;

            midY += (float)(UnityEngine.Random.value - 0.5) * displace;

            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

            Vector3 midPos = new Vector3(midX, midY, midZ);

            //递归获得点

            CollectLinPos(startPos, midPos, displace / 2);

            CollectLinPos(midPos, destPos, displace / 2);

        }
    }
}