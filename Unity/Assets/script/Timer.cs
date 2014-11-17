using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{
    public static float m_Progress = 0.0f;
    public float m_TimeLeft = 120;

    //private string m_Text = "";
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_TimeLeft > 0)
            m_TimeLeft -= Time.deltaTime;
        if (m_TimeLeft < 0)
            m_TimeLeft = 0.0f;
        string minutes = Mathf.Floor((int)m_TimeLeft / 60).ToString("00");
        string seconds = ((int)m_TimeLeft % 60).ToString("00");
        //m_Text = minutes + ":" + seconds;

        m_Progress = m_TimeLeft / 120.0f;
	}

    public float GetTimeLeft()
    {
        return m_TimeLeft;
    }
}