using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cherry : MonoBehaviour 
{
    public Sprite m_CherrySprite;
    public List<Sprite> m_BadCherrySprites;

    bool m_Bad = false;
    float m_DropTime = 1.0f;
    float m_FallTimer = 1.0f;
    float m_targetScale = 0;
    float m_RockSpeed = 10;
    float m_MaxRock = 30;
    float m_CurrentRockAmount = 0;

    GameObject m_CherryChild;

	// Use this for initialization
	void Start ()
    {
        m_CherryChild = transform.GetChild(0).gameObject;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        m_targetScale = m_CherryChild.transform.localScale.x;
        m_CherryChild.transform.localScale = Vector3.zero;

        m_FallTimer = m_DropTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(m_FallTimer > 0)
        {
            m_FallTimer -= Time.deltaTime;
            float temp = (m_DropTime - m_FallTimer) / m_DropTime;
            m_CherryChild.transform.localScale = new Vector3(temp * m_targetScale, temp * m_targetScale, temp * m_targetScale);
            m_CurrentRockAmount = Mathf.Lerp(m_CurrentRockAmount, 1, Time.deltaTime * 2);
        }
        else
        {
            m_CurrentRockAmount = Mathf.Lerp(m_CurrentRockAmount, 0.2f, Time.deltaTime * 5);
        }

        if( m_FallTimer < 0)
        {
            m_CherryChild.transform.localScale = new Vector3(m_targetScale, m_targetScale, m_targetScale);
            m_FallTimer = 0;
            m_CurrentRockAmount = 1;
            Drop();
        }

        // Rocking
        if (CherryDropperManager.Overload.HasValue && (bool)CherryDropperManager.Overload)
            m_CherryChild.transform.localRotation = Quaternion.AngleAxis(Mathf.Sin(Time.time * m_RockSpeed) * m_MaxRock * m_CurrentRockAmount, Vector3.forward);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Hero")
        {
            col.gameObject.GetComponent<Hero>().GrabCherry(m_Bad);
        }

        if(col.gameObject.tag != "Cherry")
            Destroy(gameObject);
    }

    public void MakeBad(int max)
    {
        int id = Random.Range(0, Mathf.Min(m_BadCherrySprites.Count, max));

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = m_BadCherrySprites[id];
        m_Bad = true;
        gameObject.name = "BadCherry";
    }

    public void SetDropTime(float time)
    {
        m_DropTime = time;
        m_FallTimer = m_DropTime;
    }

    void Drop()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}
