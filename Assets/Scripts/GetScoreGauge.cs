using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreGauge : MonoBehaviour
{
    public static GetScoreGauge ms_instance;

    [SerializeField] private Slider m_getGauge = null;

    private void Start()
    {
        ms_instance = this;
    }
    public void SetSliderValue(float value)
    {
        m_getGauge.value = value;
    }

}
