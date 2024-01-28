using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RiskGauge : MonoBehaviour
{
    [SerializeField] private GameObject m_imageObj = null;
    [SerializeField] private float m_gaugeMaxSize = 1f;
    [SerializeField] private GameObject m_textObj = null; 
    [SerializeField] private float m_textMaxSize = 1f;
    [SerializeField] private float m_textMinSize = 0.5f;
    private Transform m_imageTf = null;
    private Transform m_textTf = null;
    private Text m_text = null;
    // Start is called before the first frame update
    void Start()
    {
        m_imageTf = m_imageObj.transform;
        m_textTf = m_textObj.transform;
        m_text = m_textObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        Gauge(); 
        Text();
    }
    private void Gauge()
    {
        m_imageTf.localScale = Vector3.one * m_gaugeMaxSize * GameManager.ms_instance.GetRisk;
    }
    private void Text()
    {
        m_text.text = GameManager.ms_instance.GetAddScore().ToString();
        m_textTf.localScale = Vector3.one * Mathf.Clamp(m_textMaxSize * GameManager.ms_instance.GetRisk, m_textMinSize, m_textMaxSize);
    }
}
