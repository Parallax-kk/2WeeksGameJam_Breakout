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
    private static readonly Vector3 DEFAULT_POSITION = new Vector3(0.0f, 1.0f, 1.6f);

    /// <summary>
    /// 最低速度
    /// </summary>
    private static readonly float MIN_SPEED = 10.0f;

    /// <summary>
    /// ボールのリセット
    /// </summary>
    public void Reset()
    {
        transform.position = DEFAULT_POSITION;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var vel = GetComponent<Rigidbody>().velocity;

        if (vel.z > 0)
        {
            vel.z = vel.z > MIN_SPEED ? vel.z : MIN_SPEED;
        }
        else if (vel.z < 0)
        {
            vel.z = vel.z < -MIN_SPEED ? vel.z : -MIN_SPEED;
        }

        if (collision.gameObject.name == "LeftBar")
        {
            vel.x = -Mathf.Abs(vel.x) - 1.0f;
        }
        else if (collision.gameObject.name == "RightBar")
        {
            vel.x = Mathf.Abs(vel.x) + 1.0f;
        }

        GetComponent<Rigidbody>().velocity = vel;
    }
}
