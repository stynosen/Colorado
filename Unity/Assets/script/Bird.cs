using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
    public bool m_Done = true;
    public GameObject m_Cherry;
    public bool m_SpawnCherry;
    public AudioClip m_Tweet;

    private bool m_Animate = false;
    private float m_AnimateTimer = 2;
    private GameObject m_MyCherry = null;
    private float m_SoundDelay = 0.5f;

    private bool m_Enabled = false;
	// Use this for initialization
	void Start () 
    {
        CherryDropperManager.OnOverloadChanged += OverloadChanged;
        SetAnimate(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(m_Enabled)
        {
            if (m_Done)
            {
                m_Done = false;
                SetAnimate(false);
                m_AnimateTimer = Random.Range(0.3f,3.0f);
                Destroy(m_MyCherry);
                m_MyCherry = null;
                m_SpawnCherry = false;
            }

            if (CherryDropperManager.IsPlaying())
            {
                if (m_AnimateTimer > 0)
                {
                    m_AnimateTimer -= Time.deltaTime;
                    if (m_AnimateTimer < 0)
                    {
                        m_AnimateTimer = 0.0f;
                        SetAnimate(true);
                        GetComponent<Animator>().speed = Random.Range(0.5f, 1.0f);
                        m_Done = false;
                        m_SpawnCherry = false;
                    }
                }

                if (m_MyCherry == null && m_SpawnCherry)
                {
                    m_MyCherry = GameObject.Instantiate(m_Cherry);
                    m_MyCherry.transform.parent = gameObject.transform;
                    m_MyCherry.transform.localPosition = m_Cherry.transform.position;
                    m_MyCherry.transform.localScale = m_Cherry.transform.localScale;
                    m_MyCherry.transform.localRotation = m_Cherry.transform.rotation;
                }

                if (m_Animate)
                {
                    m_SoundDelay -= Time.deltaTime;
                    if (m_SoundDelay < 0)
                    {
                        m_SoundDelay = 0.5f;
                        AudioSource.PlayClipAtPoint(m_Tweet, Vector3.zero, 0.2f);
                    }
                }
            }
        }
	}

    void SetAnimate(bool enabled)
    {
        m_Animate = enabled;
        GetComponent<Animator>().enabled = enabled;
    }

    void OverloadChanged(bool overload)
    {
        m_Enabled = overload;
        GetComponent<SpriteRenderer>().enabled = overload;
    }
}
