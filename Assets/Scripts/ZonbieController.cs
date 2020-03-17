using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AudioManager;

public class ZonbieController : MonoBehaviour
{
    /// <summary>
    /// メインシステム
    /// </summary>
    private MainSystem m_MainSystem = null;

    /// <summary>
    /// バー
    /// </summary>
    private Transform m_Bar = null;

    /// <summary>
    /// パラメータ
    /// </summary>
    [SerializeField]
    private Zonbie m_Zonbie = null;

    /// <summary>
    /// navMesh
    /// </summary>
    private NavMeshAgent m_NavAgent = null;

    /// <summary>
    /// 現在の耐久値
    /// </summary>
    [SerializeField]
    private int m_CurrentEndurance = 0;

    /// <summary>
    /// ゾンビのプレハブ
    /// </summary>
    [SerializeField]
    private List<GameObject> m_ZombiePrefabs = new List<GameObject>();

    private void Awake()
    {
        m_MainSystem = GameObject.Find("MainSystem").GetComponent<MainSystem>();
        m_CurrentEndurance = m_Zonbie.GetEndurance();
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Bar = GameObject.Find("Bar").transform;
        m_NavAgent.destination = m_Bar.position;
        Instantiate(m_ZombiePrefabs[Random.Range(0,m_ZombiePrefabs.Count)],transform);
    }

    private void Update()
    {
        m_NavAgent.destination = m_Bar.position;

        if (transform.position.z <= -1.5f)
        {
            m_MainSystem.DecreaseStock();
            Destroy(gameObject);
        }
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
                MainSystem.AddScore(m_Zonbie.GetScore());
                Destroy(gameObject);

                if (!MainSystem.m_isGameOver)
                {
                    SEManager.Instance.Play(SEPath.ATTACK);
                }
            }
        }
    }
}
