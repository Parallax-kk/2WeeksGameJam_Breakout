using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour
{
    /// <summary>
    /// ボール
    /// </summary>
    [SerializeField]
    private GameObject m_Ball = null;

    /// <summary>
    /// 初射出時に与える力
    /// </summary>
    [SerializeField]
    private static readonly float POWER = 10.0f;

    private void Awake()
    {
        m_Ball.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-POWER, POWER), POWER, 0.0f);
    }

    /// <summary>
    /// スタートボタン
    /// </summary>
    public void StartButton()
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
}
