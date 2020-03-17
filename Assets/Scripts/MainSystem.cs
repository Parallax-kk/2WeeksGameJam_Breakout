using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using AudioManager;
using UnityEngine.UI;

public class MainSystem : MonoBehaviour
{
    /// <summary>
    /// ブロック群
    /// </summary>
    [SerializeField]
    Transform m_Zonbies = null;

    /// <summary>
    /// ボール
    /// </summary>
    [SerializeField]
    private Transform m_Ball = null;

    /// <summary>
    /// スコア
    /// </summary>
    private static int m_Score = 0;

    /// <summary>
    /// 初めのショットが終わったか否か
    /// </summary>
    [SerializeField]
    private bool m_finishFirstShot = false;

    /// <summary>
    /// 設定パネル
    /// </summary>
    [SerializeField]
    private GameObject m_SettingPnale = null;

    /// <summary>
    /// BGMスライダー
    /// </summary>
    [SerializeField]
    private Slider m_BGMSlider = null;

    /// <summary>
    /// SEスライダー
    /// </summary>
    [SerializeField]
    private Slider m_SESlider = null;

    /// <summary>
    /// スコアのテキスト
    /// </summary>
    [SerializeField]
    private static TextMeshProUGUI m_ScoreText = null;

    /// <summary>
    /// リザルト画面のスコアのテキスト
    /// </summary>
    [SerializeField]
    private static TextMeshProUGUI m_ResultScoreText = null;

    /// <summary>
    /// ストック表示パネル
    /// </summary>
    [SerializeField]
    private GameObject m_StockPanel = null;

    /// <summary>
    /// リザルト画面
    /// </summary>
    [SerializeField]
    private RectTransform m_ResultPanel = null;

    /// <summary>
    /// リザルトテキスト
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI m_ResultText = null;

    /// <summary>
    /// 一時停止パネル
    /// </summary>
    [SerializeField]
    private GameObject m_PausePanel = null;

    /// <summary>
    /// 初射出時に与える力
    /// </summary>
    [SerializeField]
    private float m_FirstShotPower = 10.0f;

    /// <summary>
    /// Zombieのプレハブ
    /// </summary>
    [SerializeField]
    private List<GameObject> m_listZonbiePdefabs = new List<GameObject>();

    /// <summary>
    /// ゾンビのスポーンポイント
    /// </summary>
    [SerializeField]
    private Transform m_SpownPoint = null;

    /// <summary>
    /// ゾンビのrootオブジェクト
    /// </summary>
    [SerializeField]
    private Transform m_ZonbieRoot = null;

    /// <summary>
    /// ゾンビスポーンSE
    /// </summary>
    private List<string> m_listZonbieSpownSE = new List<string>();

    /// <summary>
    /// ゲームオーバーか否かのフラグ
    /// </summary>
    public static bool m_isGameOver = false;

    private void Awake()
    {
        m_isGameOver = false;

        m_ScoreText = GameObject.Find("Canvas/Panel/ScoreText").GetComponent<TextMeshProUGUI>();
        m_ResultScoreText = GameObject.Find("Canvas/ResultPanel/ResultScoreText").GetComponent<TextMeshProUGUI>();
        m_listZonbieSpownSE = new List<string>() { SEPath.GROWL01, SEPath.GROWL02, SEPath.GROWL03,
                                                   SEPath.GROWL04, SEPath.GROWL05, SEPath.GROWL06};
        Time.timeScale = 1.0f;
        m_Score = 0;
        m_ScoreText.text = "Score:0";
        m_ResultScoreText.text = "Score:0";
        StartCoroutine("ZonbieSpown");
        m_BGMSlider.value = BGMManager.Instance.GetBaseVolume();
        m_SESlider.value = SEManager.Instance.GetBaseVolume();
    }

    private IEnumerator ZonbieSpown()
    {
        while (true)
        {
            Instantiate(m_listZonbiePdefabs[Random.Range(0, m_listZonbiePdefabs.Count)],
                        m_SpownPoint.GetChild(Random.Range(0, m_SpownPoint.childCount)).position,
                        Quaternion.identity,
                        m_ZonbieRoot);

            if (Random.Range(0, 5) == 0)
            {
                int index = Random.Range(0, m_listZonbieSpownSE.Count);
                SEManager.Instance.Play(m_listZonbieSpownSE[index]);
            }

            if (m_isGameOver)
            {
                break;
            }

            yield return new WaitForSeconds(3.0f);
        }
    }

    private void Update()
    {
        if (!m_isGameOver)
        {
            if (!m_finishFirstShot && Input.GetMouseButtonDown(0) && !m_PausePanel.activeSelf && m_Ball.GetComponent<Rigidbody>().velocity.magnitude == 0.0f)
            {
                m_finishFirstShot = true;
                m_Ball.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-m_FirstShotPower, m_FirstShotPower), 0.0f, m_FirstShotPower);
                m_Ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 10.0f, 0.0f);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_PausePanel.activeSelf)
                {
                    Time.timeScale = 1.0f;
                    m_PausePanel.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0.0f;
                    m_PausePanel.SetActive(true);
                }
            }
        }
        if (m_Ball.position.z < -4.0f)
        {
            // 残機があればボール位置リセット
            if (m_StockPanel.transform.childCount > 0)
            {
                m_Ball.GetComponent<BallController>().Reset();
                DecreaseStock();
                m_finishFirstShot = false;
            }
            else if (!m_isGameOver)
            {
                DisplayResult();
            }
        }

        if (m_StockPanel.transform.childCount == 0 && !m_isGameOver)
        {
            DisplayResult();
        }
    }

    /// <summary>
    /// 残機減少
    /// </summary>
    public void DecreaseStock()
    {
        if (m_StockPanel.transform.childCount > 0)
        {
            Destroy(m_StockPanel.transform.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// リザルトの表示
    /// </summary>
    private void DisplayResult()
    {
        m_isGameOver = true;

        if (m_Zonbies.childCount > 0)
        {
            m_ResultText.text = "Game Over";
        }
        else
        {
            m_ResultText.text = "Congratulations!";
        }

        m_Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_ResultPanel.DOLocalMoveY(0.0f, 1.0f);
    }

    /// <summary>
    /// スコアの追加
    /// </summary>
    /// <param name="score"></param>
    public static void AddScore(int score)
    {
        if (!m_isGameOver)
        {
            m_Score += score;
            m_ScoreText.text = "Score:" + m_Score.ToString();
            m_ResultScoreText.text = "Score:" + m_Score.ToString();
        }
    }

    /// <summary>
    /// 続けるボタン
    /// </summary>
    public void ResumeButton()
    {
        Time.timeScale = 1.0f;
        m_PausePanel.SetActive(false);
    }

    /// <summary>
    /// 設定パネル表示/非表示ボタン
    /// </summary>
    public void DisplaySettingButton()
    {
        m_SettingPnale.SetActive(!m_SettingPnale.activeSelf);
    }

    /// <summary>
    /// リトライボタン
    /// </summary>
    public void RetryButton()
    {
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 終了ボタン
    /// </summary>
    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }

    /// <summary>
    /// ツイートボタン
    /// </summary>
    public void TweetButton()
    {
        string esctext = UnityWebRequest.EscapeURL("2 Weeks Game Jam No.1「Break Out」\n Score:" + m_Score.ToString() + "\n https://parallax-kk.github.io/2WeeksGameJam_Breakout_Web/");
        string esctag = UnityWebRequest.EscapeURL("2WeeksGameJam");
        string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

        //Twitter投稿画面の起動
        Application.OpenURL(url);
    }

    /// <summary>
    /// スライダーの値変更
    /// </summary>
    public void UpdateSlider()
    {
        //BGM全体のボリュームを変更
        BGMManager.Instance.ChangeBaseVolume(m_BGMSlider.value);

        //SE全体のボリュームを変更
        SEManager.Instance.ChangeBaseVolume(m_SESlider.value);
    }
}
