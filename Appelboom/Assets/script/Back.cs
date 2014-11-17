using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour 
{
    public Color Target;
    private Color Current;
	// Use this for initialization
	void Start () 
    {
        Current = GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () 
    {
        float p = 0.75f;
        float x = Mathf.Max(0, (1.0f - Timer.m_Progress) - p) / (1.0f-p);
        GetComponent<SpriteRenderer>().color = Color.Lerp(Current, Target, x);
	}
}
