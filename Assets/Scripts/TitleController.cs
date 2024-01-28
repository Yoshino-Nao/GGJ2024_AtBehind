using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private Text m_textPressAnyButton = null;
    [SerializeField] private float m_blinkTime = 1f;
    [SerializeField] private float m_pressedAnimationTime = 0.5f;
    [SerializeField] private float m_pressedFontSize = 80f;
    private bool m_isPressedAnimationStart = false;
    private float m_elapsedTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            m_isPressedAnimationStart = true;
        }

        //PressAnyButton���������Ƃ��̉��o
        if (m_isPressedAnimationStart)
        {
            m_textPressAnyButton.color = new Color(0f, 0f, 0f, Easing.QuintOut(m_elapsedTime, m_pressedAnimationTime, 1f, 0f));
            m_textPressAnyButton.fontSize = (int)Easing.QuintOut(m_elapsedTime, m_pressedAnimationTime, m_textPressAnyButton.fontSize, m_pressedFontSize);
            //�o�ߎ��Ԃ��v��
            m_elapsedTime += Time.deltaTime;
            //�^�C�g���̃A�j���[�V�������I�������烁�j���[�̑�����󂯕t����
            if (m_pressedAnimationTime <= m_elapsedTime)
            {
                GameStart();
            }
        }
        //PressAnyButton�̓_��
        else
        {
            //-0.5~0.5�̐U����0.5�𑫂���1~0�̐U���ɂ���
            m_textPressAnyButton.color = new Color(0f, 0f, 0f, Mathf.Sin(2 * Mathf.PI * (1f / m_blinkTime) * Time.time) / 2 + 0.5f);
        }
    }
    private void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
