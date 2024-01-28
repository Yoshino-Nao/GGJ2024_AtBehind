using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    [SerializeField] private GameObject m_gameClearCanvas;
    [SerializeField] Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        m_gameClearCanvas.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.text = GameManager.ms_instance.GetScore.ToString();
            m_gameClearCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
