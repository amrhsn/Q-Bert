using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EslamMovement : MonoBehaviour
{

    public float jump;
    private float speed;
    public float speedtoDown;
    public float speedtoUp;

    float dist;
    bool moving, jumping = false;

    private Quaternion playerRotation;

    private Vector3 targetPosition;
    private Vector3 lookAtTarget;
    Renderer mat;
    Vector3 target, startPosition;
    Rigidbody rb;
    public GameObject StartCube;
    DataStructure tries;
    public Transform newInstance;
    public Animator animator;
    public Text scoreText;
    public GameObject particle;

    //public float forceUp;
    //public float forcrDown;
    public float forceJumptoUp;
    public float forceJumptoDown;

    private static int score;

    public float delayTime;
    private float startTime;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        tries = StartCube.GetComponent<DataStructure>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && !moving)
            {
                target = hit.collider.gameObject.transform.position;
                for (int i = 0; i < tries.targetPositions.Length; i++)
                {
                    if (target == tries.targetPositions[i].position)
                    {
                        lookAtTarget = new Vector3(tries.targetPositions[i].position.x, transform.position.y, tries.targetPositions[i].position.z);

                        animator.SetTrigger("Jumping");
                        transform.LookAt(lookAtTarget);
                        //speed = 2.3f;
                        target.y += 0.6f;
                        startPosition = transform.position;
                        //Move to upper target
                        if (startPosition.y - target.y < -0.5f)
                        {
                            //rb.AddForce(new Vector3(-1f, forceUp * rb.mass, 0), ForceMode.Impulse);

                            rb.AddForce(transform.up * forceJumptoUp * rb.mass, ForceMode.Impulse);
                            speed = speedtoUp;
                            //rb.AddForce(transform.forward * forceUp, ForceMode.Impulse);

                            //jumping = true;
                            //moving = true;
                            //animator.SetBool("Moving", false);
                            //Invoke("MovingCondition", 0.2f);
                        }
                        else //Move to Lower target
                        {
                            rb.AddForce(transform.up * forceJumptoDown * rb.mass, ForceMode.Impulse);
                            speed = speedtoDown;
                            //rb.AddForce(transform.forward * forcrDown, ForceMode.Impulse);
                            //speed += 1f;
                            //moving = true;
                            //animator.SetBool("Moving", false);
                        }

                        moving = true;
                        startTime = Time.time; ;

                    }
                    else continue;

                }


            }
        }

        if (moving)
        {
            float yAxis;
            if (Time.time - startTime > 0.5f)
            {
                //Move down
                yAxis = Mathf.Lerp(transform.position.y, target.y, speed * Time.deltaTime);
            }
            else
            {
                //move up
                yAxis = Mathf.Lerp(transform.position.y, target.y + 1, speed * Time.deltaTime);
            }

            float zAxis = Mathf.Lerp(transform.position.z, target.z, speed * Time.deltaTime);
            float xAxis = Mathf.Lerp(transform.position.x, target.x, speed * Time.deltaTime);
            Vector3 nexPosition = new Vector3(xAxis, yAxis, zAxis);
            transform.position = nexPosition;
            //speed += 0.01f;
            dist = Mathf.Abs(Vector3.Distance(transform.position, target));

            //Debug.Log(dist);
            animator.SetBool("Moving", false);

        }

        if (dist < 0.5f && moving == true)
        {
            moving = false;
            // animator.SetBool("Moving", false);

        }

        /*
        if (startTime > 0 &&moving)
        {
            startTime -= Time.deltaTime;
           
        }

        else
        {
            rb.AddForce(-transform.up * 10 * rb.mass, ForceMode.Impulse);
        }
        */



        if (LifeController.health == 0)
        {
            gameObject.SetActive(false);

        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Enter");

        Instantiate(particle, transform.localPosition, Quaternion.Euler(90F, 0F, 0F));
        Destroy(GameObject.Find("DAX_Earth_Shield_00(Clone)"), 1f);

        mat = collision.gameObject.GetComponent<Renderer>();
        if (!(mat.material.color == Color.green))
        {
            //Destroy(particle);
            score += 25;
            SetScore();
        }

        mat.material.color = Color.green;
        tries = collision.gameObject.GetComponent<DataStructure>();
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.tag == "Enemy")
        {
            LifeController.health -= 1;
            RespawnPlayer();
            gameObject.SetActive(false);


        }
        else
        {
            // tries = collision.gameObject.GetComponent<DataStructure>();

        }

    }

    void MovingCondition()
    {
        moving = true;

    }

    void UpdatePos()
    {

    }

    void SetScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void RespawnPlayer()
    {

        Instantiate(gameObject.transform, newInstance.position, Quaternion.identity);
    }



}
