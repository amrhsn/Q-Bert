using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float jump;
    public float speed;
    
    float dist; 
    bool moving , jumping = false;

    private Quaternion playerRotation;
    
    private Vector3 targetPosition;
    private Vector3 lookAtTarget;
    Renderer mat;
    Vector3 target, startPosition ;
    Rigidbody rb;
    DataStructure tries;
    public Transform newInstance;
    public Animator animator;
    public Text scoreText;
    public GameObject particle;
   
    private static int score;

    void Start()
    {
      
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && moving == false)
            {
                target = hit.collider.gameObject.transform.position;
                for (int i = 0; i < tries.targetPositions.Length; i++)
                {
                    if (target == tries.targetPositions[i].position )
                    {
                        lookAtTarget = new Vector3(tries.targetPositions[i].position.x, transform.position.y, tries.targetPositions[i].position.z);

                        animator.SetTrigger("Jumping");
                        transform.LookAt(lookAtTarget);
                        speed = 2.3f;
                        target.y += 0.6f;
                        startPosition = transform.position;
                        if (startPosition.y - target.y < -0.5f)
                        {
                            rb.AddForce(new Vector3(-1f, 8f * rb.mass, 0), ForceMode.Impulse);
                            
                            //rb.AddForce(transform.up * jump, ForceMode.Impulse);
                            jumping = true;
                            //moving = true;
                            //animator.SetBool("Moving", false);
                            Invoke("MovingCondition", 0.2f);
                        }
                        else
                        {
                            rb.AddForce(transform.up * jump, ForceMode.Impulse);
                            speed += 1f;
                            moving = true;
                            //animator.SetBool("Moving", false);
                        }
                    }
                    else continue;

                }
                
               
            }  
        }
        if (moving)
        {
            
            float zAxis = Mathf.Lerp(transform.position.z, target.z, speed * Time.deltaTime);
            float xAxis = Mathf.Lerp(transform.position.x, target.x, speed * Time.deltaTime);
            Vector3 nexPosition = new Vector3(xAxis, transform.position.y, zAxis);
            transform.position = nexPosition;
            speed += 0.01f;
            dist = Mathf.Abs(Vector3.Distance(transform.position, target));
            //Debug.Log(dist);
            animator.SetBool("Moving", false);

        }

        if (dist < 0.5f && moving == true)
        {
            moving = false;
           // animator.SetBool("Moving", false);

        }
        
        if(LifeController.health==0)
        {
            gameObject.SetActive(false);
            
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        
        Instantiate(particle, transform.localPosition,Quaternion.Euler(90F,0F,0F));
        Destroy(GameObject.Find("DAX_Earth_Shield_00(Clone)"), 1f);
        mat = collision.gameObject.GetComponent<Renderer>();
        if (!(mat.material.color == Color.green))
        {
            //Destroy(particle);
            score += 25;
            SetScore();
        }
  
        mat.material.color = Color.green;
       
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
            tries = collision.gameObject.GetComponent<DataStructure>();

        }

    }
    void MovingCondition()
    {
        moving = true;
      
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
