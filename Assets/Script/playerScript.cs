using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{

    Rigidbody2D rb;

    [Header("Variables")]
    public float jumpForce;
    public float movementSpeed;
    public bool isGrounded;

    [Header("Controller keys variables")]

    bool isLeft;
    bool isRight;
    bool isJump;
    public float moveX;
    float moveY;


    [Header("Gameobject")]
    public GameObject obstacles;
    public GameObject Ring;
    public GameObject panel;

    public bool isOver;

    Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isLeft = false;
        isRight = false;
        isJump = false;
    }


    public void pointerDownLeft()
    {
        isLeft = true;     // left button press
    }
    public void pointerUpLeft()
    {
        isLeft = false;     // left button unPress
    }
    public void pointerDownRight()
    {
        isRight = true;
    }
    public void pointerUpRight()
    {
        isRight = false;
    }
    public void pointerDownJump()
    {
        isJump = true;
    }
    public void pointerUpJump()
    {
        isJump = false;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        //moveX = Input.GetAxis("Horizontal")* Time.deltaTime;     // both window controll

        //transform.Translate(new Vector2(moveX * movementSpeed, 0));

        if (isLeft || Input.GetAxis("Horizontal") < 0)
        {
            //Debug.Log("Enter in right keyword");
            moveX = -movementSpeed;
        }
        else if (isRight || Input.GetAxis("Horizontal") > 0)
        {
            //Debug.Log("Enter in left keyword");
            moveX = movementSpeed;
        }
        else
        {
            moveX = 0;
        }

        if ((isJump && isGrounded) || Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce));
        }

        rb.velocity = new Vector2(moveX, rb.velocity.y);
    }

    

    //void jump()
    //{
    //    rb.AddForce(Vector2.up * jumpForce);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("collided with ground");
            isGrounded = true;
        }

        if(collision.gameObject.CompareTag("obstacles"))
        {
            Debug.Log("Enter in obstacles");
            StartCoroutine("Restart");
        }

        if (collision.gameObject.CompareTag("Nail"))
        {
            reSizeScript _resizeScript = collision.gameObject.GetComponent<reSizeScript>();

            if (_resizeScript.property == 1)
            {
                Destroy(_resizeScript.invisibleCollider);
                _resizeScript.gameObject.transform.localScale = new Vector3(0.3f, 0.44f, 1);     // obstacles ka size decrease krne ke liye when player collid
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else if(_resizeScript.property == 2)
            {
                Destroy(_resizeScript.invisibleCollider);
                _resizeScript.gameObject.transform.localScale = new Vector3(0.3f, 0.44f, 1);     // obstacles ka size decrease krne ke liye when player collid
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if(collision.gameObject.CompareTag("End"))
        {
            isOver = true;
            StartCoroutine("Restart");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("ground exit");
            isGrounded = false;
        }
        
    }
    IEnumerator Restart()
    {
        if(isOver)
        {
            Debug.Log("enter in exit mode");  // this when game is over then canvas panel show 
            yield return new WaitForSeconds(0.5f);
            panel.SetActive(true);
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0;
        }

        yield return new WaitForSeconds(0.2f);  // when player collid with obstacles then restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}