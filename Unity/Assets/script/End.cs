using UnityEngine;
using System.Collections;

public class End : MonoBehaviour 
{
    private float m_VisibleTime = -1.0f;
	// Use this for initialization
	void Start () 
    {
        CherryDropperManager.OnPlayingChanged += OnPlayingChange;
	}

    public void OnPlayingChange(bool playing)
    {
        if (!playing && Timer.m_Finished)
        {
            m_VisibleTime = 2.0f;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if(m_VisibleTime > 0)
        {
            m_VisibleTime -= Time.deltaTime;
            if(m_VisibleTime <= 0)
            {
                SendToEnquete();
            }
        }
	}

    void SendToEnquete()
    {
        Application.ExternalCall("GotoNextPage");
    }
}