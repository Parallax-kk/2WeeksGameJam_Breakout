using UnityEngine;
using AudioManager;

public class BarController : MonoBehaviour
{
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float m_MoveSpeed = 1.0f;

    [SerializeField]
    private MainSystem m_MainSystem = null;

    private void Update()
    {
        if (Time.timeScale == 1.0f && !MainSystem.m_isGameOver)
        {
            // マウス位置をスクリーン座標からワールド座標に変換する
            var mausePosition = Input.mousePosition;
            mausePosition.z = 10;
            var targetPos = Camera.main.ScreenToWorldPoint(mausePosition);
            
            // X, Y座標の範囲を制限する
            targetPos.x = Mathf.Clamp(targetPos.x, -5.0f, 5.0f);
            targetPos.y = 0.8f;
            targetPos.z = 0.0f;

            // このスクリプトがアタッチされたゲームオブジェクトを、マウス位置に線形補間で追従させる
            transform.position = Vector3.Lerp(transform.position, targetPos, m_MoveSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zonbie")
        {
            m_MainSystem.DecreaseStock();
            Destroy(other.gameObject);

            if (!MainSystem.m_isGameOver)
            {
                SEManager.Instance.Play(SEPath.BITE);
            }
        }
    }
}
