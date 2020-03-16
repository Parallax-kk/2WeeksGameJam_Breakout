using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Zonbie", menuName = "Create Zonbie")]
public class Zonbie : ScriptableObject
{
    /// <summary>
    /// 耐久値
    /// </summary>
    [SerializeField]
    private int m_Endurance = 0;

    [SerializeField]
    private int m_Score = 0;

    /// <summary>
    /// 耐久値の取得
    /// </summary>
    /// <returns></returns>
    public int GetEndurance() { return m_Endurance; }

    /// <summary>
    /// スコアの取得
    /// </summary>
    /// <returns></returns>
    public int GetScore() { return m_Score; }
}