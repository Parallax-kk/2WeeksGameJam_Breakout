using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainSystem : MonoBehaviour
{
    /// <summary>
    /// ブロック群
    /// </summary>
    [SerializeField]
    Transform m_Blocks = null;

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
    /// ゲームオーバーか否かのフラグ
    /// </summary>
    private bool m_isGameOver = false;

    private void Awake()
    {
        m_ScoreText = GameObject.Find("Canvas/Panel/ScoreText").GetComponent<TextMeshProUGUI>();
        m_ResultScoreText = GameObject.Find("Canvas/ResultPanel/ResultScoreText").GetComponent<TextMeshProUGUI>();
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (!m_finishFirstShot && Input.GetMouseButtonDown(0) && !m_PausePanel.activeSelf)
        {
            m_finishFirstShot = true;
            m_Ball.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-m_FirstShotPower, m_FirstShotPower), m_FirstShotPower, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(m_PausePanel.activeSelf)
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

        if(m_Ball.position.y < -7.0f)
        {
            // 残機があればボール位置リセット
            if(m_StockPanel.transform.childCount > 0)
            {
                m_Ball.GetComponent<BallController>().Reset();
                Destroy(m_StockPanel.transform.GetChild(0).gameObject);
                m_finishFirstShot = false;
            }
            else if(!m_isGameOver)
            {
                m_isGameOver = true;

                if(m_Blocks.childCount > 0)
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
        }
    }

    /// <summary>
    /// スコアの追加
    /// </summary>
    /// <param name="score"></param>
    public static void AddScore(int score)
    {
        m_Score += score;
        m_ScoreText.text = "Score:" + m_Score.ToString();
        m_ResultScoreText.text = "Score:" + m_Score.ToString();
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
    public void PushTweetButton()
    {
        string esctext = UnityWebRequest.EscapeURL("2 Weeks Game Jam 「Break Out」\n Score:" + m_Score.ToString());
        string esctag = UnityWebRequest.EscapeURL("2WeeksGameJam");
        string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

        //Twitter投稿画面の起動
        Application.OpenURL(url);
    }
}
