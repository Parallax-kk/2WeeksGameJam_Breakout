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
    private List<GameObject> m_listZombiePrefabs = new List<GameObject>();

    /// <summary>
    /// 血痕のプレハブ
    /// </summary>
    [SerializeField]
    private List<GameObject> m_listBloodPrefabs = new List<GameObject>();

    /// <summary>
    /// 血痕ルートオブジェクト
    /// </summary>
    private Transform m_Bloods = null;

    /// <summary>
    /// 血痕Y位置
    /// </summary>
    private static readonly float BLOOD_POSITION_Y = -0.1948f;

    /// <summary>
    /// 死んだかどうかのフラグ
    /// </summary>
    private bool m_isDead = false;

    private void Awake()
    {
        m_MainSystem = GameObject.Find("MainSystem").GetComponent<MainSystem>();
        m_Bloods = GameObject.Find("Bloods").transform;
        m_CurrentEndurance = m_Zonbie.GetEndurance();
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Bar = GameObject.Find("Bar").transform;
        m_NavAgent.destination = m_Bar.position;
        Instantiate(m_listZombiePrefabs[Random.Range(0, m_listZombiePrefabs.Count)],transform);
    }

    private void Update()
    {
        if (!m_isDead)
        {
            m_NavAgent.destination = m_Bar.position;

            if (transform.position.z <= -1.5f)
            {
                m_MainSystem.DecreaseStock();
                Destroy(gameObject);
            }
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
                m_isDead = true;
                MainSystem.AddScore(m_Zonbie.GetScore());

                GetComponent<BoxCollider>().enabled  = false;
                GetComponent<NavMeshAgent>().enabled = false;

                // 血痕生成
                var pos = transform.position;
                pos.y = BLOOD_POSITION_Y;
                Instantiate(m_listBloodPrefabs[Random.Range(0, m_listBloodPrefabs.Count)], pos, Quaternion.identity, m_Bloods);
                if(m_Bloods.childCount > 100)
                {
                    Destroy(m_Bloods.GetChild(0).gameObject);
                }

                // パーティクル動作
                transform.GetChild(0).GetComponent<ParticleSystem>().Play();

                Destroy(transform.GetChild(1).gameObject);
                Destroy(gameObject,1.0f);

                if (!MainSystem.m_isGameOver)
                {
                    SEManager.Instance.Play(SEPath.ATTACK);
                }
            }
        }
    }
}
