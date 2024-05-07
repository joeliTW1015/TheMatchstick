using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcDetect : MonoBehaviour
{
    [SerializeField] float detectDistance;
    [SerializeField] Transform Player;
    [SerializeField] LayerMask canSeeLayer;
    BoxCollider2D coll;
    Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D attactBox;
    OrcAI orcAI;
    bool findPlayer;
    // Start is called before the first frame update
    void Start()
    {
        orcAI = GetComponent<OrcAI>();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        Player = orcAI.target;
        orcAI.followEnable = false;
        findPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!findPlayer && !PlayerDie.isDead)
        {
            DrawRays();
        }
        else if(orcAI.followEnable)
        {
            animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
            if(Mathf.Abs(transform.position.x - Player.position.x) <= 3 && !PlayerDie.isDead)
            {
                animator.SetTrigger("attact");
            }
            else if(PlayerDie.isDead)
            {
                orcAI.enabled = false;
            }
        }
    }

    void DrawRays()
    {
        RaycastHit2D hit1 = Physics2D.Linecast(coll.bounds.center + Vector3.up, coll.bounds.center + Vector3.up + transform.right * detectDistance * transform.localScale.x, canSeeLayer);
        RaycastHit2D hit2 = Physics2D.Linecast(coll.bounds.center, coll.bounds.center + transform.right * detectDistance* transform.localScale.x, canSeeLayer);
        RaycastHit2D hit3 = Physics2D.Linecast(coll.bounds.center - Vector3.up, coll.bounds.center - Vector3.up + transform.right * detectDistance* transform.localScale.x, canSeeLayer);
        Collider2D hitCollider = DetectPlayer(hit1, hit2, hit3);

        if(hitCollider != null)
        {
            if(hitCollider.CompareTag("Player") && !findPlayer)
            {
                findPlayer = true;
                Invoke("StartFollow", 0.5f);
                animator.SetTrigger("shout");
                Debug.DrawLine(coll.bounds.center + Vector3.up, coll.bounds.center + Vector3.up + transform.right * detectDistance* transform.localScale.x, Color.red);
                Debug.DrawLine(coll.bounds.center, coll.bounds.center + transform.right * detectDistance* transform.localScale.x, Color.red);
                Debug.DrawLine(coll.bounds.center - Vector3.up, coll.bounds.center - Vector3.up + transform.right * detectDistance* transform.localScale.x, Color.red);
            }
            else
            {
                Debug.DrawLine(coll.bounds.center + Vector3.up, coll.bounds.center + Vector3.up + transform.right * detectDistance* transform.localScale.x, Color.green);
                Debug.DrawLine(coll.bounds.center, coll.bounds.center + transform.right * detectDistance* transform.localScale.x, Color.green);
                Debug.DrawLine(coll.bounds.center - Vector3.up, coll.bounds.center - Vector3.up + transform.right * detectDistance* transform.localScale.x, Color.green);
            }
        }
    }

    Collider2D DetectPlayer(RaycastHit2D _hit1, RaycastHit2D _hit2, RaycastHit2D _hit3)
    {
        if(_hit1.collider != null)
            return _hit1.collider;

        if(_hit2.collider != null)
            return _hit2.collider;

        if(_hit3.collider != null)
            return _hit3.collider;

        return null;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Player") && !findPlayer)
        {
            findPlayer = true; 
            Invoke("StartFollow", 0.5f);
            animator.SetTrigger("shout");
        }
    }

    public void StartFollow()
    {
        if(orcAI.followEnable)
        {
            return;
        }
        orcAI.followEnable = true;
    }

    private void OnDisable() {
        attactBox.enabled = false;
    }
}
