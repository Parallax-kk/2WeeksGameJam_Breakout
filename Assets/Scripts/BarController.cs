using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float m_MoveSpeed = 1.0f;

    private void Update()
    {
        if (Time.timeScale == 1.0f)
        {
            // マウス位置をスクリーン座標からワールド座標に変換する
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // X, Y座標の範囲を制限する
            targetPos.x = Mathf.Clamp(targetPos.x, -13.0f, 13.0f);
            targetPos.y = 0.0f;
            targetPos.z = 0.0f;

            // このスクリプトがアタッチされたゲームオブジェクトを、マウス位置に線形補間で追従させる
            transform.position = Vector3.Lerp(transform.position, targetPos, m_MoveSpeed);
        }
    }
}
