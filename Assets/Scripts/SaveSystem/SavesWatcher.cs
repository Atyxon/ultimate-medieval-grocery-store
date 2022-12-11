using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public static class SavesWatcher 
{
    public static void SaveSlot(string[] saveName, int[] time, int[] day, int[] level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlotsData.umgs";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveSlotData data = new SaveSlotData(saveName, time, day, level);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveSlotData LoadSave()
    {
        string path = Application.persistentDataPath + "/SaveSlotsData.umgs";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveSlotData data = formatter.Deserialize(stream) as SaveSlotData;
            stream.Close();
            return data;
        }
        else
        {
            string[] newStringList = new string[100];
            for (int i = 0; i < newStringList.Length; i++)
            {
                newStringList[i] = null;
            }
            int[] newIntList = new int[100];
            int[] newDayIntList = new int[100];
            int[] newPlayerLevelIntList = new int[100];
            SaveSlot(newStringList, newIntList, newDayIntList, newPlayerLevelIntList);
            LoadSave();
            return null;
        }
    }
}
