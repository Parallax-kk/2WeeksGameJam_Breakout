using AudioManager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    /// <summary>
    /// ロード中のパネル
    /// </summary>
    [SerializeField]
    private RectTransform m_LoadingPanel = null;

    private void Start()
    {
        BGMManager.Instance.ChangeBaseVolume(m_BGMSlider.value);
        SEManager.Instance.ChangeBaseVolume(m_SESlider.value);
        BGMManager.Instance.Play(BGMPath.CYBER_PHYSICAL, 1, 0, 1, true);
    }

    /// <summary>
    /// スタートボタン
    /// </summary>
    public void StartButton()
    {
        Sequence seq = DOTween.Sequence();
        SEManager.Instance.Play(SEPath.SHUTTER,0.7f,0.0f,1.0f);
        seq.Append(m_LoadingPanel.DOLocalMoveY(0.0f, 4.0f));
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            SceneManager.LoadScene("Main");
        });
    }

    /// <summary>
    /// 設定パネル表示/非表示ボタン
    /// </summary>
    public void DisplaySettingButton()
    {
        m_SettingPnale.SetActive(!m_SettingPnale.activeSelf);
    }

    /// <summary>
    /// スライダーの値変更
    /// </summary>
    public void UpdateSlider()
    {
        //BGM全体のボリュームを変更
        BGMManager.Instance.ChangeBaseVolume(m_BGMSlider.value);
        MainSystem.m_BGMRate = m_BGMSlider.value;

        //SE全体のボリュームを変更
        SEManager.Instance.ChangeBaseVolume(m_SESlider.value);
        MainSystem.m_SERate = m_SESlider.value;
    }
}
