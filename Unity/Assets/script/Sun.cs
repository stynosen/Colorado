using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour
{
    private Vector3 m_Center;
    public float m_Distance = 10;

    // Use this for initialization
    void Start()
    {
        m_Center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float degrees = Mathf.Lerp(0, 2.0f, Timer.m_Progress);
        transform.position = new Vector3(Mathf.Cos(degrees) * m_Distance, Mathf.Sin(degrees) * m_Distance, 0) + m_Center;
    }
}
