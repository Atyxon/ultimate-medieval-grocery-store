using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    public Transform headTarget;
    public Animator characterAnim;
    public int objectsToBuyInt;
    public storeManager store;
    public CustomersManager manager;
    public NavMeshAgent agent;
    public Transform messagePoint;
    public List<sellableSerializable> demandingObjects = new List<sellableSerializable>();
    public Vector2 minMaxObjects;
    public float waitAfterSatisfied = 2;
    bool inQueue;
    bool setOrder;
    bool satisfied;
    bool isGone;
    bool exitedStore;
    bool tooLate;
    float timer;
    float dist;
    int exitPoint;
    public AudioSource audioSrc;
    public AudioClip[] cashClips;
    [Space]
    [TextArea]
    public string[] startText;
    [TextArea]
    public string[] endText;
    [TextArea]
    public string finalText;
    private void Start()
    {
        waitAfterSatisfied = Random.Range(waitAfterSatisfied - 1, waitAfterSatisfied + 1);
        store = FindObjectOfType<storeManager>();
        manager = FindObjectOfType<CustomersManager>();
        exitPoint = Random.Range(0, store.pointsToGoAway.Length);
        agent.SetDestination(store.pointToOpenDoors.position);
        int items = Random.Range((int)minMaxObjects.x, (int)minMaxObjects.y + 1);
        int textIndex = Random.Range(0, startText.Length);
        for (int i = 0; i < items; i++)
        {
            sellableSerializable newDemanding = new sellableSerializable();
            newDemanding.quantity = Random.Range(1, 4);
            newDemanding.sellableType = (sellableSerializable.SellableType)Random.Range(0, objectsToBuyInt);
            demandingObjects.Add(newDemanding);
        }

        finalText = startText[textIndex];
        for (int i = 0; i < demandingObjects.Count; i++)
        {
            finalText += demandingObjects[i].quantity + "x <sprite=" + (int)demandingObjects[i].sellableType + "> ";
        }
        finalText += endText[textIndex];
    }
    public void BoughtGoods()
    {
        store.messageUI.text = "Thanks!";
        satisfied = true;
        audioSrc.clip = cashClips[Random.Range(0, cashClips.Length)];
        audioSrc.Play();
    }
    private void Update()
    {
        headTarget.position = store.inventory.cameraController.pointToLookAt.position;
        if (agent.velocity.x > .5f || agent.velocity.z > .5f || agent.velocity.x < -.5f || agent.velocity.z < -.5f)
        {
            characterAnim.SetBool("isWalking", true);
        }
        else
        {
            characterAnim.SetBool("isWalking", false);
        }

        if (manager.timeMng.time > 1200)
        {

            tooLate = true;
            satisfied = true;
            isGone = false;
        }

        dist = Vector3.Distance(transform.position, store.pointToOpenDoors.position);
        if (!inQueue)
        {
            if (isGone)
            {
                float exitDist = Vector3.Distance(transform.position, store.pointsToGoAway[exitPoint].position);
                if (exitDist <= 3)
                {
                    manager.CustomerDespawn();
                    Destroy(this.gameObject);
                }
            }
        }

        if (!tooLate)
        {
            if (inQueue)
            {
                // IF FIRST IN QUEUE
                if (store.customersInQueue[0] == this)
                {
                    float distance = Vector3.Distance(this.gameObject.transform.position, store.pointsInQueue[0].position);
                    if (distance <= .5f)
                    {
                        store.SetMessagePoistion(messagePoint.position);
                        if (!setOrder)
                        {
                            store.messageUI.text = finalText;
                            store.customerInPlace = true;
                            store.customer = this;
                            store.NewCustomer(1);
                            setOrder = true;
                        }
                    }
                    else
                    {
                        setOrder = false;
                        store.customerInPlace = false;
                        store.SetMessagePoistion(messagePoint.position);
                    }
                }
            }

            if (dist <= 2)
            {
                if (store.frontDoors.isOpen)
                {
                    if (!inQueue && !isGone)
                    {
                        for (int i = 0; i < store.pointsInQueue.Length; i++)
                        {
                            if (!inQueue && store.customersInQueue[i] == null)
                            {
                                agent.SetDestination(store.pointsInQueue[i].position);
                                store.customersInQueue[i] = this;
                                inQueue = true;
                                dist = 5;
                            }
                        }
                    }
                }
                else
                {
                    store.frontDoors.interaction();
                    characterAnim.SetTrigger("open");
                }
            }
        }
        else
        {
            if (dist <= 2)
            {
                if (!store.frontDoors.isOpen)
                {
                    store.frontDoors.interaction();
                    characterAnim.SetTrigger("open");
                }
            }
        }

        if (satisfied)
        {
            if (!isGone)
            {
                timer += Time.deltaTime;
                if (timer >= waitAfterSatisfied && !exitedStore)
                {
                    agent.SetDestination(store.exitPointPhase1.position);
                    store.customer = null;
                    store.customersInQueue[0] = null;
                    if(!tooLate)
                        store.Next();
                    isGone = true;
                    inQueue = false;
                    store.customer = null;
                    store.message.SetActive(false);
                    exitedStore = true;
                }
            }
            float distance = Vector3.Distance(this.transform.position, store.exitPointPhase1.position);
            if (distance <= 2)
            {
                agent.SetDestination(store.pointsToGoAway[exitPoint].position);
            }
        }
    }
}
