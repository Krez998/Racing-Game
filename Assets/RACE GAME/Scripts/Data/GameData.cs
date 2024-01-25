using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameData : MonoBehaviour
{
    public Data Data { get; private set; }
    public void Save(Data data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/save.dat", FileMode.Create);
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            Data = (Data)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
        }
    }
}

[System.Serializable]
public class Data
{
    public int Rating;
}
