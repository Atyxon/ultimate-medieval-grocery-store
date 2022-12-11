using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesSlots
{
    public string[] names;
    public SavesSlots(string[] saveNameArray)
    {
        names = new string[100];
        names = saveNameArray;
    }
}
