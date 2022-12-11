using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : MonoBehaviour
{
    public float forwardSpeed;
    public float backwardSpeed;
    public int maxCarriedObjects;
    [Space]
    public LineRenderer lineRenderer;
    public PlayerInventory playerInv;
    public LayerMask rayMask;
    public LayerMask sellableMask;
    public float rayLenght;
    public float grabForce;
    public float fuelUsage;
    bool isWorking;
    public Transform laserStartPos;
    public Transform laserEndPos;
    public Transform laserEnd;
    public Tool toolManager;
    ToolItem extractor;
    private void OnEnable()
    {
        extractor = (ToolItem)playerInv.quickbarSlots[playerInv.selectedSlot].item;
    }
    public void NewFuel()
    {
        if (toolManager.fuelInSelectedTool != null)
        {
            Color startColor = toolManager.fuelInSelectedTool.fuelColor;
            Color endColor = toolManager.fuelInSelectedTool.fuelColor;
            startColor.a = .7f;
            endColor.a = 0;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }
    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, laserStartPos.position);
        lineRenderer.SetPosition(1, laserEnd.position);
    }
    private void Update()
    {
        if (playerInv.ctrls.Player.LMB.ReadValue<float>() > .1f && toolManager.fuelInSelectedTool != null && toolManager.fuelInSelectedTool.fuel > 0)
        {
            isWorking = true;
        }
        else
        {
            isWorking = false;
        }

        if (toolManager.fuelInSelectedTool != null && toolManager.fuelInSelectedTool.fuel <= 0)
        {
            Destroy(toolManager.fuelInSelectedTool);
            toolManager.fuelInSelectedTool = null;
            extractor.loadedFuel = null;
            playerInv.UpdateSlot(playerInv.selectedSlot, 1);
            playerInv.quickbarSlots[playerInv.selectedSlot].invSlot.slotSprite.CheckToolFuel();
            toolManager.HideFuel();
        }
    }
    private void FixedUpdate()
    {
        if (isWorking)
        {
            lineRenderer.startWidth = Mathf.Lerp(lineRenderer.startWidth, .7f, 4 * Time.fixedDeltaTime);
            toolManager.fuelInSelectedTool.fuel -= fuelUsage * Time.fixedDeltaTime;
            playerInv.UpdateSlot(playerInv.selectedSlot, 1);
            RaycastHit hit;
            if (Physics.Raycast(playerInv.playerCamera.transform.position, playerInv.playerCamera.transform.forward, out hit, rayLenght, rayMask))
            {
                laserEndPos.transform.position = hit.point;
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, 2, sellableMask);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.tag == "dragable" && System.Array.IndexOf(hitColliders, hitCollider) < maxCarriedObjects)
                    {
                        print(System.Array.IndexOf(hitColliders, hitCollider));
                        Rigidbody rb = hitCollider.gameObject.GetComponent<Rigidbody>();
                        rb.velocity = (hit.point - hitCollider.gameObject.transform.position) * grabForce;
                        rb.angularVelocity = Vector3.zero;
                    }
                }
                laserEnd.position = Vector3.Lerp(laserEnd.position, laserEndPos.position, forwardSpeed * Time.fixedDeltaTime);
            }
            else
            {
                Vector3 pos = playerInv.playerCamera.transform.position + (playerInv.playerCamera.transform.forward * rayLenght);
                laserEndPos.transform.position = pos;
                Collider[] hitColliders = Physics.OverlapSphere(pos, 2, sellableMask);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.tag == "dragable" && System.Array.IndexOf(hitColliders, hitCollider) < maxCarriedObjects)
                    {
                        Rigidbody rb = hitCollider.gameObject.GetComponent<Rigidbody>();
                        rb.velocity = (pos - hitCollider.gameObject.transform.position) * grabForce;
                        rb.angularVelocity = Vector3.zero;
                    }
                }
                if (hitColliders.Length > 0)
                {
                    Vector3 avPos = (laserEndPos.position + hitColliders[0].transform.position)/2;

                    laserEnd.position = Vector3.Lerp(laserEnd.position, avPos, forwardSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    laserEnd.position = Vector3.Lerp(laserEnd.position, laserEndPos.position, forwardSpeed * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            lineRenderer.startWidth = Mathf.Lerp(lineRenderer.startWidth, 0, 4 * Time.fixedDeltaTime);
            laserEnd.position = Vector3.Lerp(laserEnd.position, laserStartPos.position, backwardSpeed * Time.fixedDeltaTime);
        }
    }
}
