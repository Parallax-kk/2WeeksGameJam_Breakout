using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    /// <summary>
    /// パラメータ
    /// </summary>
    [SerializeField]
    private Block m_Block = null;

    /// <summary>
    /// 現在の耐久値
    /// </summary>
    [SerializeField]
    private int m_CurrentEndurance = 0;

    private void Awake()
    {
        m_CurrentEndurance = m_Block.GetEndurance();   
    }

    /// <summary>
    /// 耐久値を減らす
    /// </summary>
    /// <param name="power"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            m_CurrentEndurance -= collision.gameObject.GetComponent<BallController>().GetPowor();

            //　耐久値が0以下なら破壊
            if (m_CurrentEndurance <= 0)
            {
                MainSystem.AddScore(m_Block.GetScore());
                Destroy(gameObject);
            }
        }
    }
}
