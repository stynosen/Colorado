using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CherryDropperManager : MonoBehaviour 
{
    public GameObject Cherry;
    public bool OverloadMode = false;
    public bool EnjoymentMode = false;

    public static bool? Overload = null;
    public static bool? Enjoyment = null;

    private float CurDropTime = -1;
    private List<Vector3> Spawners;
    private float AmountOfBadCherries = 1.0f;
    private float MinDropTime = 0.8f;
    private float MaxDropTime = 2.15f;
    private float MinDropSpeed = 0.5f;
    private float MaxDropSpeed = 1.5f;
    private float ChanceToGoBad = 4;

    private List<int> m_Previous;
    
    public delegate void ChangedOverloadHandler(bool overload);
    public delegate void ChangedEnjoymentHandler(bool enjoyment);

    public static ChangedOverloadHandler OnOverloadChanged;
    public static ChangedEnjoymentHandler OnEnjoymentChanged;

	// Use this for initialization
	void Start () 
    {
        Spawners = new List<Vector3>();
        CurDropTime = Random.Range(MinDropTime, MaxDropTime);
        AddSpawners();

        m_Previous = new List<int>();

        OnOverloadChanged += OverloadChanged;
	}
	
    void OverloadChanged(bool overload)
    {
        if ((bool)Overload)
        {
            MinDropTime = 0.3f;
            MaxDropTime = 1.0f;
            ChanceToGoBad = 3;
            AmountOfBadCherries = 4.0f;
            MinDropSpeed = 0.6f;
            MaxDropSpeed = 1.2f;
        }
        else
        {
            MinDropTime = 0.8f;
            MaxDropTime = 2.15f;
            ChanceToGoBad = 4;
            AmountOfBadCherries = 1.0f;
            MinDropSpeed = 0.9f;
            MaxDropSpeed = 1.5f;
        }
    }

	// Update is called once per frame
	void Update () 
    {
        HandleEvents();

        CurDropTime -= Time.deltaTime;
        if(CurDropTime <= 0)
        {
            CurDropTime = Random.Range(MinDropTime, MaxDropTime);
            DropCherry();
        }
	}

    private void HandleEvents()
    {
        if (!Overload.HasValue || (bool)Overload != OverloadMode)
        {
            Overload = OverloadMode;
            OnOverloadChanged((bool)Overload);
        }
        if (!Enjoyment.HasValue || (bool)Enjoyment != EnjoymentMode)
        {
            Enjoyment = EnjoymentMode;
            OnEnjoymentChanged((bool)Enjoyment);
        }
    }

    void DropCherry()
    {
        bool bad = Random.Range(0, (int)ChanceToGoBad-1) == 0;

        int currentDropper = Random.Range(0, Spawners.Count);
        while(m_Previous.Contains(currentDropper))
            currentDropper = Random.Range(0, Spawners.Count);

        if(m_Previous.Count >= 2)
            m_Previous.RemoveAt(0);
        m_Previous.Add(currentDropper);

        Vector3 pos = Spawners[currentDropper];
        GameObject cherry = GameObject.Instantiate(Cherry, pos, Cherry.transform.rotation) as GameObject;
        Cherry cherryComp = cherry.GetComponent<Cherry>();
        cherryComp.SetDropTime(Random.Range(MinDropSpeed, MaxDropSpeed));
        if(bad)
        {
            cherryComp.MakeBad((int)AmountOfBadCherries);
        }
    }

    void AddSpawners()
    {
        List<GameObject> spawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("DropNode"));
        for(int i = 0; i<spawners.Count; ++i)
        {
            Spawners.Add(spawners[i].transform.position);
            Destroy(spawners[i]);
        }
    }
}
