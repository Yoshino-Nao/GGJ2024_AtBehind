using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager ms_instance;

    [SerializeField] private float m_canGetScoreMaxDist = 5f;
    [SerializeField] private float m_canGetScoreMinDist = 2f;
    [SerializeField] private float m_nearestEnemyDist = 0f;
    [SerializeField] public float m_addMaxScore = 500;
    [SerializeField] private int m_scoreMaximumValue = 999999;
    [SerializeField] private Text m_scoreText = null;
    private int m_score = 0;
    public int GetScore
    {
        get { return m_score; }
    }
    private float m_risk = 0f;
    public float GetRisk
    {
        get { return m_risk; }
    }
    private Player m_player = null;
    private Transform m_playerTf = null;

    // Start is called before the first frame update
    void Start()
    {
        if (ms_instance == null)
        {
            ms_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        m_player = FindObjectOfType<Player>();
        m_playerTf = m_player.transform;

        m_nearestEnemyDist = float.MaxValue;

        m_scoreText.text = 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //ƒvƒŒƒCƒ„[‚©‚çÅ‚à‹ß‚¢“G‚Æ‚Ì‹——£‚ðŒv‘ª
        ShortestDistanceToEnemy();
        //“G‚Æ‚ÌÅ’Z‹——£‚©‚çƒŠƒXƒN‚ðŒvŽZ
        RiskCalculation();
    }
    private void ShortestDistanceToEnemy()
    {
        var hits = Physics2D.CircleCastAll(m_playerTf.position, m_canGetScoreMaxDist, Vector2.zero, 0f, LayerMask.GetMask("Enemy")).Select(_ => _.transform).ToList();
        if (0 < hits.Count())
        {
            float min = float.MaxValue;
            foreach (var hit in hits)
            {
                float ToEnemyDist = (hit.position - m_playerTf.position).magnitude;

                if (min > ToEnemyDist)
                {
                    min = ToEnemyDist;
                }
            }
            m_nearestEnemyDist = min;
            DebugPrint.Print(string.Format("Å’Z‹——£{0}", min));
        }
    }
    private void RiskCalculation()
    {
        float Diff = m_canGetScoreMaxDist - m_canGetScoreMinDist;
        float RiskRatio = 1 - Mathf.Clamp((m_nearestEnemyDist - m_canGetScoreMinDist) / Diff, 0, 1);
        m_risk = RiskRatio;
    }
    public int GetAddScore()
    {
        return (int)(m_addMaxScore * m_risk);
    }
    public void AddScore()
    {
        m_score += (int)(m_addMaxScore * m_risk);
        if (m_score > m_scoreMaximumValue)
        {
            m_score = m_scoreMaximumValue;
        }
        m_scoreText.text = m_score.ToString();
    }
}
