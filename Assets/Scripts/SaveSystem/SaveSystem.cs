using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(string saveName, PlayerInventory playerInventory, TimeManager timeMng, SaveLoadManager saveLoadMng)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveName + ".umgs";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameSave data = new GameSave(playerInventory, timeMng, saveLoadMng); 
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static GameSave LoadSave(string saveName)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".umgs";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameSave data = formatter.Deserialize(stream) as GameSave;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File not found!");
            return null;
        }
    }
    public static void DeleteSave(string saveName)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".umgs";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
