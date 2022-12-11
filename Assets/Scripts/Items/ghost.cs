using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : MonoBehaviour
{
    public bool canBuild;
    public float sphereRadius;
    public LayerMask mask;
    [Space]
    public Material buildMaterial;
    public Color green;
    public Color red;
    private void Update()
    {
        if (Physics.CheckSphere(transform.position, sphereRadius, mask))
        {
            canBuild = false;
            buildMaterial.color = red;
        }
        else
        {
            canBuild = true;
            buildMaterial.color = green;
        }
    }
}
