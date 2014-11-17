using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static float m_Progress = 0.0f;
    public float m_TimeLeft = 120;

    public static bool m_Finished = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CherryDropperManager.IsPlaying())
        {
            if (m_TimeLeft > 0)
                m_TimeLeft -= Time.deltaTime;
            if (m_TimeLeft < 0)
                m_TimeLeft = 0.0f;

            if (m_TimeLeft == 0)
            {
                CherryDropperManager.IsPlayingBool = false;
                m_Finished = true;
            }
        }
        m_Progress = m_TimeLeft / 120.0f;
    }

    public float GetTimeLeft()
    {
        return m_TimeLeft;
    }
}