using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OrcAI : MonoBehaviour
{
    [Header("Path Finding")]
    public Transform target;
    public float activateDistance = 50f; 
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpForce = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public LayerMask platformLayer;

    [Header("Custom Behavior")]
    public bool followEnable = true;
    public bool jumpEnable = true;
    public bool directionLookEnable = true;

    Path path;
    int currentWayPoint = 0;
    bool isOnGround = false;
    Seeker seeker;
    Rigidbody2D rb;
    BoxCollider2D coll;
    private void Awake() 
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start() 
    {
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate() 
    {
        if(TargetInDistance() && followEnable)
        {
            PathFollow();
        }
    }

    bool TargetInDistance()
    {
        return (target.position - transform.position).sqrMagnitude < activateDistance * activateDistance;
    }

    void PathFollow()
    {
        if(path == null)
        {
            return;
        }

        if(currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        isOnGround = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, jumpCheckOffset, platformLayer);
        Debug.DrawRay(coll.bounds.center,Vector2.down * (coll.bounds.extents.y + jumpCheckOffset), Color.red);  

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        if(jumpEnable && isOnGround)
        {
            if(direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        if(direction.x > 0.01)
        {
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
        }
        else if(direction.x < -0.01)
        {
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
        }

        float distanceSqr = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).SqrMagnitude();

        if(distanceSqr < nextWayPointDistance * nextWayPointDistance)
        {
            currentWayPoint++;
        }

        if(directionLookEnable)
        {
            if(target.position.x > rb.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            }
            else if(target.position.x < rb.position.x)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            }
        }

    }
     
    void UpdatePath()
    {
        if(followEnable && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void OnDisable() 
    {
        rb.velocity = Vector2.zero;
    }


}
