using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CherryDropperManager : MonoBehaviour
{
    public GameObject Cherry;
    public bool OverloadMode = false;
    public bool EnjoymentMode = false;
    public bool IsPlayingBool = false;

    public static bool? Overload = null;
    public static bool? Enjoyment = null;
    public static bool? Playing = null;
    public static float m_MaxScore = 0;

    public int m_AmountOfGoodCherries = 60;

    private float CurDropTime = -1;
    private List<Vector3> Spawners;
    private float AmountOfBadCherries = 1.0f;
    private float MinDropTime = 0.8f;
    private float MaxDropTime = 2.15f;
    private float MinDropSpeed = 0.5f;
    private float MaxDropSpeed = 1.5f;

    private float m_DropGoodTimer = 0;
    private List<float> m_DropGoodTimes = new List<float>();

    private List<int> m_Previous;

    public delegate void ChangedOverloadHandler(bool overload);
    public delegate void ChangedEnjoymentHandler(bool enjoyment);
    public delegate void ChangedPlaying(bool playing);

    public static ChangedOverloadHandler OnOverloadChanged;
    public static ChangedEnjoymentHandler OnEnjoymentChanged;
    public static ChangedEnjoymentHandler OnPlayingChanged;

    // Use this for initialization
    void Start()
    {
        m_MaxScore = m_AmountOfGoodCherries;

        Spawners = new List<Vector3>();
        CurDropTime = Random.Range(MinDropTime, MaxDropTime);
        AddSpawners();

        m_Previous = new List<int>();

        OnOverloadChanged += OverloadChanged;

        for (int i = 0; i < m_AmountOfGoodCherries; ++i)
        {
            float dropTime = Random.Range(0.0f, 115.0f);
            dropTime += Random.Range(-1.0f, 1.0f);
            m_DropGoodTimes.Add(dropTime);
        }

        m_DropGoodTimes.Sort();
    }

    void OverloadChanged(bool overload)
    {
        if ((bool)Overload)
        {
            MinDropTime = 0.3f;
            MaxDropTime = 0.8f;
            AmountOfBadCherries = 4.0f;
            MinDropSpeed = 0.6f;
            MaxDropSpeed = 1.1f;
        }
        else
        {
            MinDropTime = 0.8f;
            MaxDropTime = 1.25f;
            AmountOfBadCherries = 1.0f;
            MinDropSpeed = 0.9f;
            MaxDropSpeed = 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleEvents();

        if (IsPlayingBool)
        {
            m_DropGoodTimer += Time.deltaTime;
            CurDropTime -= Time.deltaTime;
            if (CurDropTime <= 0)
            {
                bool bad = true;

                if (m_DropGoodTimes.Count > 0 && m_DropGoodTimer >= m_DropGoodTimes[0])
                {
                    m_DropGoodTimes.RemoveAt(0);
                    bad = false;
                }

                CurDropTime = Random.Range(MinDropTime, MaxDropTime);
                DropCherry(bad);
            }
        }
    }

    private void HandleEvents()
    {
        if (!Overload.HasValue || (bool)Overload != OverloadMode)
        {
            Overload = OverloadMode;
            if (OnOverloadChanged != null)
                OnOverloadChanged((bool)Overload);
        }
        if (!Enjoyment.HasValue || (bool)Enjoyment != EnjoymentMode)
        {
            Enjoyment = EnjoymentMode;
            if (OnEnjoymentChanged != null)
                OnEnjoymentChanged((bool)Enjoyment);
        }
        if (!Playing.HasValue || (bool)Playing != IsPlayingBool)
        {
            Playing = IsPlayingBool;
            if (OnPlayingChanged != null)
                OnPlayingChanged((bool)Playing);
        }
    }

    void DropCherry(bool bad)
    {
        int currentDropper = Random.Range(0, Spawners.Count);
        while (m_Previous.Contains(currentDropper))
            currentDropper = Random.Range(0, Spawners.Count);

        if (m_Previous.Count >= 2)
            m_Previous.RemoveAt(0);
        m_Previous.Add(currentDropper);

        Vector3 pos = Spawners[currentDropper];
        GameObject cherry = GameObject.Instantiate(Cherry, pos, Cherry.transform.rotation) as GameObject;
        Cherry cherryComp = cherry.GetComponent<Cherry>();
        cherryComp.SetDropTime(Random.Range(MinDropSpeed, MaxDropSpeed));
        if (bad)
        {
            cherryComp.MakeBad((int)AmountOfBadCherries);
        }
    }

    void AddSpawners()
    {
        List<GameObject> spawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("DropNode"));
        for (int i = 0; i < spawners.Count; ++i)
        {
            Spawners.Add(spawners[i].transform.position);
            Destroy(spawners[i]);
        }
    }

    public void StartPlaying()
    {
        IsPlayingBool = true;
    }

    public void StopPlaying()
    {
        IsPlayingBool = false;
    }

    public static bool IsPlaying()
    {
        return Playing.HasValue && (bool)Playing;
    }

    public static bool IsEnjoymentMode()
    {
        return Enjoyment.HasValue && (bool)Enjoyment;
    }

    public static bool IsOverloadMode()
    {
        return Overload.HasValue && (bool)Overload;
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "START"))
            IsPlayingBool = true;
    }
}