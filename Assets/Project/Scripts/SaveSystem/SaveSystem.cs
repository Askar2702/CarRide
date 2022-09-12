using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
      public static void SaveData(Game gameData)
      {
          BinaryFormatter formatter = new BinaryFormatter();
          string path = Application.persistentDataPath + "/GameData.txt";
          FileStream stream = new FileStream(path, FileMode.Create);
          GameData Data = new GameData(gameData);
          formatter.Serialize(stream, Data);
          stream.Close();
      }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/GameData.txt";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("файла нету");
            return null;
        }
    }
}

