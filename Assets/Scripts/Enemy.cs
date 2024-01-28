using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_moveMaxSpeed = 0f;
    [SerializeField] private float m_moveSpeed = 0f;
    [SerializeField] private List<Transform> m_movePoints = null;
    [SerializeField] private float m_waitTime = 0f;
    [SerializeField] private Transform m_eyePoint = null;
    [SerializeField] private Animator m_animator;
    [SerializeField] private DrawConeMesh m_drawConeMesh = null;
    [SerializeField] private float m_viewLength = 0f;
    [SerializeField] private float m_viewRadius = 0f;

    [SerializeField] private bool m_isFind = false;
    private Transform m_tf = null;
    private Rigidbody2D m_rb2d = null;

    private Player m_player = null;

    private Vector3 m_moveVec = Vector2.zero;

    private int m_movePointsIndex = 0;
    [SerializeField] private float m_waitElapsedTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_tf = transform;
        m_rb2d = GetComponent<Rigidbody2D>();
        if (m_movePoints.Count <= 1)
        {
            Debug.LogError("����n�_�̗v�f���s�����Ă��܂�");
        }
        m_player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        m_isFind = IsFindPlayer();

        m_drawConeMesh.SetLength = m_viewLength;
        m_drawConeMesh.SetRadius = m_viewRadius * Mathf.Deg2Rad;


    }
    private void Patrol()
    {
        m_animator.SetBool("Idle", false);
        m_animator.SetBool("Run", false);

        //�ҋ@�o�ߎ��Ԃ��v��
        m_waitElapsedTime += Time.deltaTime;
        //�ҋ@�o�ߎ��Ԃ��ҋ@���Ԗ����ł���΁A�ړ����Ȃ�
        if (m_waitTime >= m_waitElapsedTime)
        {
            m_animator.SetBool("Idle", true);
            return;
        }
        m_animator.SetBool("Run", true);
        //����n�_�܂ł̃x�N�g�������߂�
        Vector3 ToMovePointVec = m_movePoints[m_movePointsIndex].position - m_tf.position;
        //����n�_�܂ł̃x�N�g���𐳋K���������̂��ړ������ɂ���
        m_moveVec = ToMovePointVec.normalized;
        //����n�_�܂ł̋��������߂�
        float ToMovePointDist = ToMovePointVec.magnitude;

        if (m_moveVec.x > 0)
        {
            m_tf.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        else if (m_moveVec.x < 0)
        {
            m_tf.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }


        //�ړ��n�_�܂ł̋������ړ������������Ă����ꍇ�́A�ړ������ňړ�
        if (ToMovePointDist >= (m_moveVec * m_moveSpeed * Time.deltaTime).magnitude)
        {
            m_tf.position += m_moveVec * m_moveSpeed * Time.deltaTime;
        }
        //������Ă����ꍇ�́A����n�_�܂ł̋����ňړ�
        else
        {
            m_tf.position += ToMovePointVec;
            //����n�_�܂ňړ������̂ŁA����n�_���X�g�̃C���f�b�N�X����
            if (m_movePointsIndex < m_movePoints.Count - 1)
            {
                m_movePointsIndex++;
            }
            else
            {
                m_movePointsIndex = 0;
            }
            //�ҋ@���Ԃ����Z�b�g
            m_waitElapsedTime = 0f;
        }
    }
    private bool IsFindPlayer()
    {
        if (m_player.GetIsHide) return false;

        bool isFind = false;
        foreach (Transform tf in m_player.GetPlayerFindPoints)
        {
            Vector3 ToPointVec = tf.position - m_eyePoint.position;

            Debug.DrawLine(m_eyePoint.position, m_eyePoint.position + ToPointVec);
            float ToPlayerFindPointDist = ToPointVec.magnitude;
            //�����O�̏ꍇ
            if (ToPlayerFindPointDist > m_viewLength)
            {
                continue;
            }

            Vector3 ToPointDir = ToPointVec.normalized;

            float ToPointAngle = Vector2.Angle(m_eyePoint.right, ToPointDir);

            if (ToPointAngle <= m_viewRadius)
            {
                isFind = true;
            }
        }

        return isFind;
    }
}
