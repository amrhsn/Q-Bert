using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float jump;
    public float speed;
    public GameObject child;
    float dist;
    bool moving, jumping,firstTime = false;

    private Quaternion playerRotation,enemyrot;
    private Vector3 targetPosition;
    private Vector3 lookAtTarget;
    Vector3 target, startPosition;
    Vector3 enemypos;
    Rigidbody rb;
    private DataStructure tries = new DataStructure();

    public Transform enemy;
   // public Transform instSources;
    // Use this for initialization
    void Start()
    {
        
        PlayerPrefs.SetFloat("Positionx", transform.position.x);
        PlayerPrefs.SetFloat("Positiony", transform.position.y);
        PlayerPrefs.SetFloat("Positionz", transform.position.z);
        


        enemypos = transform.localPosition;
        enemyrot = transform.rotation;
        // tries = new DataStructure();
        //firstTime = true;
        rb = GetComponent<Rigidbody>();
        StartCoroutine("MyCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
      
        if (moving == true)
        {
            
            float zAxis = Mathf.Lerp(transform.position.z, target.z, speed * Time.deltaTime);
            float xAxis = Mathf.Lerp(transform.position.x, target.x, speed * Time.deltaTime);
            
            Vector3 nexPosition = new Vector3(xAxis, transform.position.y, zAxis);
            
            transform.position = nexPosition;
            nexPosition = target;// newHEREEE
            
            speed += 0.01f;
            dist = Mathf.Abs(Vector3.Distance(transform.position, target));
            //Debug.Log(dist);

        }
        if (dist < 0.7f && moving)
        {
            moving = false;
            // target = tries.targetPositions[Random.Range(0, tries.targetPositions.Length)].position;  THIS WAS A BIG PROBLEM!!!!!
            StartCoroutine("MyCoroutine");


        }
           
        if(LifeController.health==0)
        {
            moving = false;
        }
    }
    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Movement();




    }

    void Movement()
    {
        
        if (!moving)
        {

            Debug.Log(target.x.ToString() + "             " + target.y.ToString() + "          " + target.z.ToString());

            target = tries.targetPositions[Random.Range(0, tries.targetPositions.Length-1)].position;
            for (int i = 0; i < tries.targetPositions.Length; i++)
            {
    

                if ((target == tries.targetPositions[i].position))
                {
                    lookAtTarget = new Vector3(tries.targetPositions[i].position.x, transform.position.y, tries.targetPositions[i].position.z);
                    transform.LookAt(lookAtTarget);
                    speed = 2.3f;
                    target.y += 0.5f;
                    startPosition = transform.position;
          
                    if (startPosition.y - target.y < -0.5f)
                    {
                        Debug.Log("da5l el IF");
                        //rb.AddForce(transform.up * jump, ForceMode.Impulse);
                        rb.AddForce(new Vector3(-1f, 9f * rb.mass,  0), ForceMode.Impulse);
                        jumping = true;
                        moving = true;
                  

                    }
                    else
                    {
                        rb.AddForce(transform.up * jump, ForceMode.Impulse);
                        speed += 1f;
                        moving = true;

                    }
                }
                else continue;
            }
        }

 

      
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.collider.gameObject.tag == "target1")
        {
            // StartCoroutine(Transferenemy());
           foreach(ContactPoint contact in collision.contacts)
            {

            target =  new Vector3(PlayerPrefs.GetFloat("Positionx"), PlayerPrefs.GetFloat("Positiony"), PlayerPrefs.GetFloat("Positionz"));
            transform.position = new Vector3(PlayerPrefs.GetFloat("Positionx"), PlayerPrefs.GetFloat("Positiony"), PlayerPrefs.GetFloat("Positionz"));
            rb.position = new Vector3(PlayerPrefs.GetFloat("Positionx"), PlayerPrefs.GetFloat("Positiony"), PlayerPrefs.GetFloat("Positionz"));
            transform.rotation = enemyrot;
            }
            


        }
        else
            tries = collision.gameObject.GetComponent<DataStructure>();

        Debug.Log("OnCollision");
    
    }
    IEnumerator Transferenemy()
    {
        //child.SetActive(false);
        //speed -=0.1f;
        // yield return new WaitForSeconds(1f);
        //enemypos = new Vector3(transform.position.x, transform.localPosition.y, 1);
        transform.localPosition = enemypos;
        transform.rotation = enemyrot;
        
        yield return new WaitForSeconds(1f);
        moving = false;
        //moving = false;
        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.2f);
        child.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        moving = true;

        Debug.Log("waaat");
    }



}
