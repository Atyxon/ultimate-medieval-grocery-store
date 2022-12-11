using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity;
    public float sensitivityMultiplier;
    public float xRotation;
    public Transform playerBody;
    public Controlls ctrls;
    public bool isEnabled;
    public Camera mainCam;
    public Transform pointToLookAt;
    private void Awake(){
        ctrls = new Controlls();
        initSettings();
    }
    private void OnEnable(){
        ctrls.Enable();
    }
    private void OnDisable(){
        ctrls.Disable();
    }
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (isEnabled)
        {
            Vector2 mouseMove = ctrls.Player.Mouse.ReadValue<Vector2>() * Time.deltaTime * sensitivity * sensitivityMultiplier;

            xRotation -= mouseMove.y;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            playerBody.Rotate(Vector3.up * mouseMove.x);
        }
    }
    public void initSettings()
    {
        mainCam.fieldOfView = PlayerPrefs.GetInt("fov");
        sensitivityMultiplier = PlayerPrefs.GetInt("sensitivity");
    }
}
