using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sellableObject : MonoBehaviour
{
    public string objectName;
    public GameObject parent;
    public sellableSerializable item;
    public bool isDisabled;

    float timer;
    BoxCollider bCol;
    MeshCollider mCol;
    private void Start()
    {
        parent.name = objectName;
        bCol = this.gameObject.GetComponent<BoxCollider>();
        mCol = this.gameObject.GetComponent<MeshCollider>();
        if (isDisabled)
        {
            if (bCol)
                bCol.enabled = false;
            if (mCol)
                mCol.enabled = false;
        }
    }
    public void EnableColliders()
    {
        if(bCol)
            bCol.enabled = true;
        if (mCol)
            mCol.enabled = true;
        isDisabled = false;
    }
    public void DisableColliderOnStart()
    {
        if (bCol)
            bCol.enabled = false;
        if (mCol)
            mCol.enabled = false;
        isDisabled = true;
    }
    private void FixedUpdate()
    {
        if(isDisabled)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= .5)
            {
                if (bCol)
                    bCol.enabled = true;
                if (mCol)
                    mCol.enabled = true;
                isDisabled = false;
            }
        }
    }
}
