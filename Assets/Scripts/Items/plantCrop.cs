using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantCrop : MonoBehaviour
{
    public plant plantObject;
    public void Cropped()
    {
        plantObject.Cropped();
        Destroy(this.gameObject);
    }
}
