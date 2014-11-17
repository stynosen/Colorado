using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    private bool m_Enjoyment = false;
    public int m_Score = 0;
    private float m_MaxHeight = -3.0f;

    private GameObject m_CherryPile;
    private float m_MinHeight;
	// Use this for initialization
	void Start () 
    {
        CherryDropperManager.OnEnjoymentChanged += EnjoymentChanged;
        GetComponent<TextMesh>().text = "";

        m_CherryPile = transform.GetChild(0).gameObject;
        m_CherryPile.GetComponent<SpriteRenderer>().enabled = false;
        m_MinHeight = m_CherryPile.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (CherryDropperManager.IsPlaying)
        {
            if (m_Enjoyment)
                GetComponent<TextMesh>().text = m_Score.ToString();
        }
	}

    public void AddScore(int s)
    {
        m_Score += s;
        
        if(m_Score > 0)
        {
            m_CherryPile.GetComponent<SpriteRenderer>().enabled = true;
            Vector3 newPos = m_CherryPile.transform.localPosition;
            newPos.y = Mathf.Lerp(m_MinHeight, m_MaxHeight, ((float)Mathf.Min(m_Score - 1, CherryDropperManager.m_MaxScore) / CherryDropperManager.m_MaxScore));
            m_CherryPile.transform.localPosition = newPos;
        }
    }

    public void EnjoymentChanged(bool enjoyment)
    {
        m_Enjoyment = enjoyment;
        if (!enjoyment)
            GetComponent<TextMesh>().text = "";

        transform.parent.gameObject.SetActive(enjoyment);
    }
}