using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    private bool m_Enjoyment = false;
    public int m_Score = 0;
	// Use this for initialization
	void Start () 
    {
        CherryDropperManager.OnEnjoymentChanged += EnjoymentChanged;
        GetComponent<TextMesh>().text = "";
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_Enjoyment)
            GetComponent<TextMesh>().text = m_Score.ToString();
	}

    public void AddScore(int s)
    {
        m_Score += s;
    }

    void EnjoymentChanged(bool enjoyment)
    {
        m_Enjoyment = enjoyment;
        if (!enjoyment)
            GetComponent<TextMesh>().text = "";
    }
}
