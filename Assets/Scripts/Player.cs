using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer = null;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_moveMaxSpeed = 0f;
    [SerializeField] private float m_acceleration = 0f;
    [SerializeField] private float m_jumpPow = 10f;
    [SerializeField] private List<Transform> m_findPoint = null;
    public List<Transform> GetPlayerFindPoints
    {
        get { return m_findPoint; }
    }
    [SerializeField] private float m_getScoreTime = 0f;
    private float m_funnyMoveingElapsedTime = 0f;

    private Transform m_tf = null;
    private Rigidbody2D m_rb2d = null;
    private Vector2 m_moveVec = Vector2.zero;

    private bool m_canJump = false;

    private int m_random = 0;
    private int m_previousNum = 0;
    private bool m_isPushed = false;

    private bool m_isFunnyMove = false;
    [SerializeField] private GameObject m_popUpObj = null;
    private bool m_canHide = false;
    private bool m_isHide = false;
    public bool GetIsHide
    {
        get { return m_isHide; }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_tf = transform;
        m_rb2d = GetComponent<Rigidbody2D>();
        m_popUpObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimParameta();
        FunnyMove();
        MoveInput();
        Hide();
    }
    private void FixedUpdate()
    {
        Move();
        Jump();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_canJump = Input.GetKeyDown(KeyCode.W);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_canJump = Input.GetKeyDown(KeyCode.W);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_canJump = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hide")
        {
            m_canHide = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Hide")
        {
            m_canHide = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Hide")
        {
            m_canHide = false;
        }
    }
    private void SetAnimParameta()
    {
        m_Animator.SetBool("Idle", false);
        m_Animator.SetBool("Run", false);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            m_Animator.SetBool("Run", true);
        }
        else
        {
            m_Animator.SetBool("Idle", true);
        }

        m_isFunnyMove = false;
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
        {
            if (!m_isPushed)
            {
                //前回と同じアニメーションを再生しないようにする処理(最悪無限ループするけど気にしない)
                while (m_previousNum == m_random)
                {
                    m_random = Random.Range(0, 3);
                }
                m_previousNum = m_random;
            }
            m_isPushed = true;
            m_isFunnyMove = true;
            m_Animator.SetBool("Idle", false);
            m_Animator.SetInteger("Funny", m_random);
        }
        else
        {
            m_isPushed = false;
        }
    }
    private void MoveInput()
    {
        Vector2 Vec = Vector2.zero;
        //左
        if (Input.GetKey(KeyCode.A))
        {
            Vec = -Vector2.right;
            m_tf.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        //右
        if (Input.GetKey(KeyCode.D))
        {
            Vec = Vector2.right;
            m_tf.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        m_moveVec = Vec;
    }
    private void Move()
    {
        float TargetSpeed = m_moveVec.x * m_moveMaxSpeed;
        TargetSpeed = Mathf.Lerp(m_rb2d.velocity.x, TargetSpeed, 1f);

        float AcceleRate = (m_acceleration / m_moveMaxSpeed) * (1f / Time.fixedDeltaTime);

        float SpeedDif = TargetSpeed - m_rb2d.velocity.x;
        float Movement = SpeedDif * AcceleRate;
        m_rb2d.AddForce(Vector2.right * Movement, ForceMode2D.Force);
    }
    private void Jump()
    {
        if (!m_canJump) return;

        m_rb2d.AddForce(Vector2.up * m_jumpPow, ForceMode2D.Impulse);
        m_canJump = false;
    }
    private void FunnyMove()
    {
        if (m_isFunnyMove)
        {
            m_funnyMoveingElapsedTime += Time.deltaTime;
        }
        if (m_getScoreTime <= m_funnyMoveingElapsedTime)
        {
            m_funnyMoveingElapsedTime = 0f;
            GameManager.ms_instance.AddScore();
        }
        if (!m_isFunnyMove)
        {
            m_funnyMoveingElapsedTime = 0f;
        }
        GetScoreGauge.ms_instance.SetSliderValue(m_funnyMoveingElapsedTime / m_getScoreTime);
    }
    private void Hide()
    {
        m_isHide = false;
        float alpha = 1f;
        //隠れるトリガーにヒットしている
        if (m_canHide)
        {
            m_popUpObj.SetActive(true);
            m_popUpObj.transform.position = m_tf.position + new Vector3(0.5f, 1.27f, 0f);

            if (Input.GetKey(KeyCode.Space))
            {
                m_isHide = true;
                alpha = 0.5f;
                DebugPrint.Print(string.Format("隠れている"));
            }
        }
        else
        {
            m_popUpObj.SetActive(false);
        }

        m_spriteRenderer.color = new Vector4(1f, 1f, 1f, alpha);
    }
}
