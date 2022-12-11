using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class plant : MonoBehaviour
{
    public string plantName;
    [Space]
    public Animator moundAnim;
    public GameObject plantObject;
    public GameObject lastStateObject;
    public GameObject grownPlant;
    public GameObject prefabToPop;
    public GameObject popParticle;
    public GameObject[] growStates;
    public GameObject leftovers;
    public Transform transformToPop;
    [Space]
    public Material basePlantMaterial;
    public Material baseMoundMaterial;
    Material initPlantMaterial;
    Material initMoundMaterial;
    public Gradient moundWetnessGradient;
    public Gradient plantHealthGradient;
    public GameObject[] moundParts;
    public GameObject[] plantParts;
    [Space]
    public float plantHealth;
    public float plantMaxHealth;
    public float plantMoisure;
    public float plantMaxMoisure;
    public float fertilizer;
    public float fertilizerMax;
    [Space]
    public Gradient healthColor;
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public AnimationCurve deadPlantAnimationCurve;
    [Space]
    public float currentGrow = 0;
    public float maxGrow;
    public float curveTimer;
    public float baseGrowSpeedMultiplier;
    public float moisureGrowSpeedMultiplier;
    public float[] growThresholds;
    public float popForce;
    public int currentGrowState;
    bool fullyGrown;
    bool maxThreshold;
    bool cropped;
    bool canAnimationCurve;
    private void Start()
    {
        initMoundMaterial = Instantiate(baseMoundMaterial);
        initPlantMaterial = Instantiate(basePlantMaterial);
        for (int i = 0; i < moundParts.Length; i++)
        {
            Renderer mound = moundParts[i].GetComponent<Renderer>();
            mound.material = initMoundMaterial;
        }

        this.gameObject.name = plantName;
        baseGrowSpeedMultiplier = baseGrowSpeedMultiplier * Random.Range(1.0f, 2.0f);
        for (int i = 1; i < growStates.Length; i++)
        {
            growStates[i].transform.localScale = new Vector3(0, 0, 0);
        }
    }
    private void FixedUpdate()
    {
        if (plantMoisure > 0)
        {
            plantMoisure -= Time.fixedDeltaTime;
            if (plantMoisure < plantMaxMoisure * 0.3)
            {
                moisureGrowSpeedMultiplier = 0;
            }
            else
            {
                moisureGrowSpeedMultiplier = plantMoisure / plantMaxMoisure;
                if (plantMoisure > plantMaxMoisure * 0.9 && plantHealth < plantMaxHealth)
                {
                    plantHealth += Time.fixedDeltaTime;
                }
            }
            initMoundMaterial.color = moundWetnessGradient.Evaluate(plantMoisure / plantMaxMoisure);
        }
        else if (plantMoisure <= 0)
        {
            plantHealth -= Time.fixedDeltaTime;
        }

        if (canAnimationCurve)
        {
            curveTimer += Time.deltaTime;
            float showScale = showCurve.Evaluate(curveTimer);
            growStates[currentGrowState].transform.localScale = new Vector3(showScale, showScale, showScale);
            float hideScale = hideCurve.Evaluate(curveTimer);
            growStates[currentGrowState - 1].transform.localScale = new Vector3(hideScale, hideScale, hideScale);

            if (curveTimer >= 3)
            {
                canAnimationCurve = false;
                curveTimer = 0;
            }
        }

        if (!cropped)
        {
            if (!fullyGrown)
            {
                if (currentGrow < maxGrow)
                {
                    currentGrow += Time.fixedDeltaTime * (baseGrowSpeedMultiplier * moisureGrowSpeedMultiplier);
                    if (!maxThreshold && currentGrow >= growThresholds[currentGrowState + 1])
                    {
                        currentGrowState += 1;
                        canAnimationCurve = true;
                        if (currentGrowState == growThresholds.Length - 1)
                            maxThreshold = true;
                    }
                }
                else
                {
                    fullyGrown = true;
                }
            }
            else
            {
                grownPlant.SetActive(true);
            }
        }
        else
        {
            curveTimer += Time.fixedDeltaTime;
            float scale = deadPlantAnimationCurve.Evaluate(curveTimer);
            transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, transform.localScale.z * scale);
        }
    }
    public void Cropped()
    {
        cropped = true;
        plantObject.SetActive(false);
        GameObject poppedObject = Instantiate(prefabToPop, transformToPop.position, transformToPop.rotation);
        poppedObject.BroadcastMessage("DisableColliderOnStart");
        GameObject particle = Instantiate(popParticle, lastStateObject.transform.position, lastStateObject.transform.rotation);
        Destroy(particle, 4);
        Rigidbody rb = poppedObject.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * popForce);
        int y = 3;
        rb.angularVelocity = new Vector3(Random.Range(-y,y), Random.Range(-y, y), Random.Range(-y, y));
        moundAnim.SetTrigger("dead");
        Destroy(this.gameObject, 10);
        curveTimer = 0;
        if(leftovers != null)
            leftovers.SetActive(true);
    }
}
