using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsSway : MonoBehaviour
{
    public Controlls ctrls;
    public MovementController moveCont;
    public Transform swayPoint;
    public PlayerInventory inv;
    Quaternion originRot;
    Vector3 armsOrigin;
    [Space]
    public float swaySensitivity;
    public float swayClamp;
    public float smoothValue;
    private float targetYRotation;
    private float targetXRotation;
    [Space]
    public float idleSpeed;
    public float walkSpeed;
    public Vector2 idleIntencity;
    public Vector2 walkIntencity;
    Vector2 targetIntencity;
    float speed;
    float forceZ;
    private void Awake()
    {
        ctrls = new Controlls();
        originRot = swayPoint.localRotation;
        armsOrigin = swayPoint.localPosition;
    }
    private void OnEnable()
    {
        ctrls.Enable();
    }
    private void OnDisable()
    {
        ctrls.Disable();
    }
    private void FixedUpdate()
    {
        forceZ += Time.fixedDeltaTime * speed;
        if (moveCont.isWalking)
        {
            speed = walkSpeed;
            targetIntencity = walkIntencity;
        }
        else
        {
            speed = idleSpeed;
            targetIntencity = idleIntencity;
        }

        Vector3 target = armsOrigin + new Vector3(Mathf.Cos(forceZ) * targetIntencity.x, Mathf.Sin(forceZ * 2) * targetIntencity.y, 0);
        swayPoint.transform.localPosition = Vector3.Lerp(swayPoint.transform.localPosition, target, 8 * Time.fixedDeltaTime);

        if (!inv.inventoryIsOpen && !inv.pauseMenu.isMenuOppened && !inv.isShopUIOpen)
        {
            Vector2 mouseMove = ctrls.Player.Mouse.ReadValue<Vector2>();

            targetXRotation = Mathf.Clamp(-mouseMove.x * swaySensitivity, -swayClamp, swayClamp);
            targetYRotation = Mathf.Clamp(mouseMove.y * swaySensitivity, -swayClamp, swayClamp);

            Quaternion rotX = Quaternion.AngleAxis(targetXRotation, Vector3.up);
            Quaternion rotY = Quaternion.AngleAxis(targetYRotation, Vector3.right);

            Quaternion targetRot = originRot * rotX * rotY;

            swayPoint.localRotation = Quaternion.Lerp(swayPoint.localRotation, targetRot, smoothValue * Time.deltaTime);
        }
        else
        {
            swayPoint.localRotation = Quaternion.Lerp(swayPoint.localRotation, Quaternion.Euler(0,0,0), smoothValue * Time.deltaTime);
        }
    }
}
