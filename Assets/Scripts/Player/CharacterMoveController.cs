using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMoveController : MonoBehaviour
{
    // Instance ini mirip seperti pada GameManager, fungsinya adalah membuat sistem singleton
    // untuk memudahkan pemanggilan script yang bersifat manager dari script lain
    private static CharacterMoveController _instance = null;
    public static CharacterMoveController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CharacterMoveController>();
            }
            return _instance;
        }
    }


    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;
    private Rigidbody2D rig;

    [Header("Jump")]
    public float jumpAccel;
    private bool isJumping;
    private CharacterSoundController sound;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;
    public bool isOnGround;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;
    [SerializeField] private Slider Bar;
    [SerializeField] private Text Energy;
    public float weightval=50;


    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;


    private Animator anim;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    private void FixedUpdate()
    {
        // raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(-0.5f,-1), (groundRaycastDistance + 0.05f), groundLayerMask);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, new Vector2(0.5f, -1), groundRaycastDistance, groundLayerMask);
        if (hit || hit2 || hit3)
        {
            if (!isOnGround && rig.velocity.y <= 0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        // calculate velocity vector
        Vector2 velocityVector = rig.velocity;

        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;
    }

    private void Update()
    {
        // read input
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
            {
                isJumping = true;
                sound.PlayJump();
            }
        }

        anim.SetBool("isOnGround", isOnGround);
        Bar.value = weightval;
        Energy.text = weightval.ToString();
        // calculate score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            if (weightval > 0)
            {
                weightval -= 0.35f;

            }
            if (weightval > 95)
            {
                if (transform.localScale.x < 3f)
                {
                    transform.localScale += new Vector3(0.01f, 0.01f, 0f);
                    groundRaycastDistance += 0.01f;
                    moveAccel -= 0.005f;
                    maxSpeed -= 0.005f;
                }
            }
            else if (weightval > 80 )
            {
                if (transform.localScale.x < 2.5f)
                {
                    transform.localScale += new Vector3(0.01f, 0.01f, 0f);
                    groundRaycastDistance += 0.01f;
                    moveAccel -= 0.005f;
                    maxSpeed -= 0.005f;
                }
            }
            else if (weightval > 70 )
            {
                if (transform.localScale.x < 2f)
                {
                    transform.localScale += new Vector3(0.01f, 0.01f, 0f);
                    groundRaycastDistance += 0.01f;
                    moveAccel -= 0.005f;
                    maxSpeed -= 0.005f;
                }
            }
            else if (weightval > 50)
            {
                if (transform.localScale.x < 1.2f)
                {
                    transform.localScale += new Vector3(0.01f, 0.01f, 0f);
                    groundRaycastDistance += 0.01f;
                    moveAccel -= 0.005f;
                    maxSpeed -= 0.005f;
                }
            }
            else if (weightval > 35)
            {
                if(transform.localScale.x > 0.8f)
                {
                    transform.localScale -= new Vector3(0.01f, 0.01f, 0f);
                    groundRaycastDistance -= 0.008f;
                    moveAccel += 0.01f;
                    maxSpeed += 0.01f;

                }

            }
            else if (weightval > 10)
            {
                if (transform.localScale.x > 0.4f)
                {
                    transform.localScale -= new Vector3(0.01f, 0.01f, 0f);
                    groundRaycastDistance -= 0.008f;
                    moveAccel += 0.01f;
                    maxSpeed += 0.01f;
                    Debug.Log(weightval);
                }
            }
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }


        // game over
        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        // set high score
        score.FinishScoring();

        // stop camera movement
        gameCamera.enabled = false;

        // show gameover
        gameOverScreen.SetActive(true);

        // disable this too
        this.enabled = false;
    }

    public void IncreaseGameSpeed(float speed)
    {
        maxSpeed += speed;

    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
        Debug.DrawLine(transform.position, transform.position + (new Vector3(-0.5f,-1f,0) * (groundRaycastDistance+0.05f)), Color.red);
        Debug.DrawLine(transform.position, transform.position + (new Vector3(0.5f, -1f, 0) * groundRaycastDistance), Color.red);
    }
}
