using UnityEngine;
using UnityEngine.SceneManagement;
using AudioManager;
using UnityEngine.UI;

public class TitleSystem : MonoBehaviour
{
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

    private void Awake()
    {
        BGMManager.Instance.Play(BGMPath.CYBER_PHYSICAL, 0.5f, 0, 1, true);
    }

    /// <summary>
    /// スタートボタン
    /// </summary>
    public void StartButton()
    {
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 設定パネル表示/非表示ボタン
    /// </summary>
    public void DisplaySettingButton()
    {
        m_SettingPnale.SetActive(!m_SettingPnale.activeSelf);
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
