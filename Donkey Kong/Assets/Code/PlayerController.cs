 using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody2D myRigidbody;
    public float speed;
    public float jumpSpeed;
    private float life;
    private bool isRoundClear;

    [Header("Hammer")]
    public Transform hammerPoint;
    public float timeHammer;
    public LayerMask enemyLayer;
    private float attackRange = 0.3f;
    private bool hammer;

    [Header("Animator")]
    public Animator myAnimator;
 
    [Header("Physics")]
    public Vector2 physicsSpherePosition;
    public float physicsSphereRadius;
    public LayerMask floorLayer;

    [Header("ClimbingMovement")]
    public Transform ladderCheck;
    public float distance;
    public float climbSpeed;
    public LayerMask ladderLayer;
    bool climbing;

    public AudioClip jummpingMan;
    public AudioClip hammerMusic;
    public AudioClip deadSfx;
    public AudioClip roundClear;
   

    void Start()
    {
        life = 1;
        hammer = false;
        isRoundClear = false;
    }

    void Update()
    {
        MoveCharacter();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();        
        }

        ClimbingLadder();

        if (myRigidbody.velocity.y <= 0.2f)
            myAnimator.SetBool("IsJumping", false);


        if(life == 0)
        {
            Invoke("RestartScene", 5);
            myAnimator.SetBool("Dead", true);
            speed = 0;
            FindObjectOfType<SpawnBarril>().enabled = false;
            GameObject[] barris = GameObject.FindGameObjectsWithTag("Destructive");
            foreach(GameObject item in barris)
            {
                Destroy(obj: item.gameObject);
            }   
        }

        if(isRoundClear)
        {
            Invoke("RestartScene", 3);
            speed = 0;
        }

        if(transform.position.y < -8)
            Invoke("RestartScene", 0);

        if (hammer)
        {
            HammerAttack();
            AudioManager.PlayBGS(hammerMusic, false, false);
           
        }
        else if(life == 0)
            AudioManager.PlayBGS(deadSfx,false, false);
        else if (isRoundClear)
            AudioManager.PlayBGS(roundClear, false, false);
        else 
            AudioManager.PlayBGS(jummpingMan, true);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destructive"))
            life = 0;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            StartCoroutine(DelayDisable(timeHammer));
            hammer = true;
            jumpSpeed = 0;
            Destroy(collision.gameObject);
            myAnimator.SetBool("HasHammer", true);

        }

        if (collision.gameObject.CompareTag("GameController"))
        {
            isRoundClear = true;
        }
    }

    void HammerAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hammerPoint.position, attackRange, enemyLayer);
        
        foreach (Collider2D barril in hitEnemies)
        {
            Debug.Log("Acertou o barril");
            barril.gameObject.SetActive(false);
            
        }
           
    }
    void MoveCharacter()
    {
        float x = Input.GetAxisRaw("Horizontal");
        myRigidbody.velocity = new Vector2(x * speed, myRigidbody.velocity.y);
        myAnimator.SetFloat("MoveSpeed", Mathf.Abs(x));        

        if (x > 0.05f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (x < -0.05f)
            transform.rotation = Quaternion.Euler(0, 180, 0);

    }

    void Jump()
    {
        if (IsOnGround())
        {
            myRigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            myAnimator.SetBool("IsJumping", true);
           
        }
    }

    bool IsOnGround()
    {
        Vector2 physicsSphereOffset = physicsSpherePosition;

        if (transform.rotation.eulerAngles.y > 0)
            physicsSphereOffset = new Vector2(physicsSpherePosition.x * -1, physicsSpherePosition.y);
       
        return Physics2D.OverlapCircle((Vector2)transform.position + physicsSpherePosition, physicsSphereRadius, floorLayer);
    }

    bool IsOnLadder()
    {
        return Physics2D.OverlapCircle(ladderCheck.position, 0.2f, ladderLayer);
    }


    void ClimbingLadder()
    {
        Debug.Log(climbing);
        float y;

        if(IsOnLadder())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                climbing = true;
                myAnimator.SetBool("Climb", true);
            }
            else
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                climbing = false;

            }
        }    

        if (climbing == true && IsOnLadder())
        {
            y = Input.GetAxis("Vertical");
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, y * climbSpeed);
            myRigidbody.gravityScale = 0;

        }
        else
        {
            myRigidbody.gravityScale = 1;
            climbing = false;
            myAnimator.SetBool("Climb", false);
            GetComponent<CapsuleCollider2D>().isTrigger = false;
        }         
    }
    
    IEnumerator DelayDisable(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        hammer = false;
        myAnimator.SetBool("HasHammer", false);
        jumpSpeed = 5;
        

    }

    public void RestartScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnDrawGizmos()
    {
        Vector2 physicsSphereOffset = physicsSpherePosition;
        
     
        if (transform.rotation.eulerAngles.y > 0)
            physicsSphereOffset = new Vector2(physicsSpherePosition.x * -1, physicsSpherePosition.y);

        Gizmos.DrawWireSphere((Vector2)transform.position + physicsSpherePosition, physicsSphereRadius);

        Gizmos.DrawWireSphere(hammerPoint.position, attackRange);

        Gizmos.DrawWireSphere(ladderCheck.position, 0.2f);

    }
}
