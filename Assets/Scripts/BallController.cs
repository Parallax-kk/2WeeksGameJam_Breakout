using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    /// <summary>
    /// ボールの破壊力
    /// </summary>
    private int m_Powor = 1;
    public int GetPowor() { return m_Powor; }
    public void IncreasePowor() { m_Powor++; }
    public void DecreasePowor() { m_Powor--; }

    /// <summary>
    /// 初期位置
    /// </summary>
    private static readonly Vector3 DEFAULT_POSITION = new Vector3(0.0f,3.0f,0.0f);

    /// <summary>
    /// ボールのリセット
    /// </summary>
    public void Reset()
    {
        transform.position = DEFAULT_POSITION;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
