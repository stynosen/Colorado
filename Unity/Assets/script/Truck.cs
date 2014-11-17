using UnityEngine;
using System.Collections;

public class Truck : MonoBehaviour 
{
    public float m_Speed = 1.5f;
    public float m_Volume = 0.0f;
    private float m_OldX = 0;

    private bool m_MakeSound = true;
	// Use this for initialization
	void Start () 
    {
        m_OldX = transform.position.x;
        CherryDropperManager.OnPlayingChanged += OnPlayingChanged;
        CherryDropperManager.OnOverloadChanged += OnLoadChanged;
	}

    private void OnLoadChanged(bool overload)
    {
        m_MakeSound = overload;
        if (!m_MakeSound)
            GetComponent<AudioSource>().volume = 0;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (CherryDropperManager.IsPlaying())
        {
            float deltaX = m_OldX - transform.position.x;
            m_OldX = transform.position.x;

            int wheelCount = transform.childCount;
            for (int i = 0; i < wheelCount; ++i)
            {
                Transform wheel = transform.GetChild(i);
                wheel.Rotate(Vector3.forward, Time.deltaTime * 5000 * deltaX);
            }

            if (m_MakeSound)
                GetComponent<AudioSource>().volume = m_Volume;
        }
	}

    void OnPlayingChanged(bool playing)
    {
        if (playing)
        {
            GetComponent<Animator>().speed = m_Speed;
        }
        else
        {
            GetComponent<Animator>().speed = 0;
            GetComponent<AudioSource>().volume = 0;
        }
    }
}