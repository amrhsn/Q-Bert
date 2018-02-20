using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementNav : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    public GameObject player;
    private Vector3 targetPos;

    private void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        //enemyAgent.autoTraverseOffMeshLink = false;
        player = GameObject.FindGameObjectWithTag("Player");
        targetPos = transform.position;
        enemyAgent.autoTraverseOffMeshLink = false;
        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        while (enemyAgent != null)
        {
            if (player != null)
            {
                if (enemyAgent.isOnOffMeshLink)
                {
                    StartCoroutine(Curve(enemyAgent, 0.5f));
                    enemyAgent.CompleteOffMeshLink();
                }
                else
                    enemyAgent.SetDestination(targetPos);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }


    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Collider"))
        {
            DataStructure tries = c.transform.parent.GetChild(0).GetComponent<DataStructure>();
            targetPos = tries.targetPositions[Random.Range(0, tries.targetPositions.Length - 1)].position;
        }
    }

    IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            //float yOffset = curve.Evaluate(normalizedTime);
            float yOffset = 1f;
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}
