using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour 
{
    public float m_MaxSickTime = 3;
    public float m_Speed = 10;
    public float m_JumpForce = 50;
    public float m_CurrentSpeed;

    public Sprite m_NormalSprite;
    public Sprite m_SickSprite;
    private bool m_Grounded = true;

    private float m_SickTimer = 0;
	// Use this for initialization
	void Start () 
    {
        m_CurrentSpeed = 0;
        SetSprite(m_NormalSprite);
	}
	
	// Update is called once per frame
	void Update () 
    {
        float axis = Input.GetAxis("Horizontal");
        if (axis != 0)
        {
            m_CurrentSpeed = Mathf.Lerp(m_CurrentSpeed, (m_SickTimer <= 0 ? m_Speed : m_Speed/2) * axis, Time.deltaTime * 10);
        }
        else
        {
            m_CurrentSpeed = Mathf.Lerp(m_CurrentSpeed, 0, Time.deltaTime * 60);
            if (Mathf.Abs(m_CurrentSpeed) < 1)
                m_CurrentSpeed = 0;
        }

        HandleMovement(m_CurrentSpeed);

        if (Input.GetButton("Jump") && m_Grounded && GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            m_Grounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, m_SickTimer <= 0 ? m_JumpForce : m_JumpForce/2));
        }

        Vector3 newpos = transform.position;
        newpos.x += m_CurrentSpeed * Time.deltaTime;
        transform.position = newpos;

        if(m_SickTimer > 0)
            m_SickTimer -= Time.deltaTime;
        if(m_SickTimer < 0)
        {
            m_SickTimer = 0;
            SetSprite(m_NormalSprite);
        }
	}

    private void HandleMovement(float speed)
    {
        if (speed != 0)
        {
            transform.rotation = Quaternion.AngleAxis(speed > 0 ? 180 : 0, Vector3.up);
        }

        string animLeftName = "LegLeftWalk";
        string animRightName = "LegRightWalk";
        if (!m_Grounded)
        {
            animLeftName = "LegLeftJump";
            animRightName = "LegRightJump";
        }

        GameObject legLeft = GameObject.Find("LegLeft");
        GameObject legRight = GameObject.Find("LegRight");

        Animator anim = legLeft.GetComponent<Animator>();
        anim.Play(animLeftName);
        if (speed != 0)
        {
            anim.speed = 1;
        }
        else
        {
            anim.speed = 0;
            anim.Play(animLeftName, 0, 0);
        }

        anim = legRight.GetComponent<Animator>();
        anim.Play(animRightName);
        if (speed != 0)
        {
            anim.speed = 1;
        }
        else
        {
            anim.speed = 0;
            anim.Play(animRightName, 0, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag== "Ground")
            m_Grounded = true;
    }

    public void MakeSick()
    {
        if (CherryDropperManager.Overload.HasValue && (bool)CherryDropperManager.Overload)
        {
            m_SickTimer = m_MaxSickTime;
            SetSprite(m_SickSprite);
        }
    }

    void SetSprite(Sprite sprite)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }
}
