using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_follows = null;
    [SerializeField] private float m_moveSpeed = 1f;
    [SerializeField] private float m_followsDist = 0f;
    private Transform m_tf = null;

    // Start is called before the first frame update
    void Start()
    {
        m_tf = transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_tf.position = Vector2.Lerp(m_tf.position, m_follows.position, m_moveSpeed * Time.deltaTime);
        m_tf.position = m_tf.position + -Vector3.forward * m_followsDist;
    }
}
