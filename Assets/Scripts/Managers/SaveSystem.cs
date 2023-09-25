using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;  

public static class SaveSystem
{
    #region Constants
    #endregion

    #region Components
    #endregion

    #region Fields
    #endregion

    #region Properties
    #endregion

    public static void Save<T>(T saveData)
    {
        BinaryFormatter bf = new BinaryFormatter ();
        FileStream file = new FileStream (Application.persistentDataPath + "/savegame.dat", FileMode.Create);
        bf.Serialize(file, saveData);
        file.Close();
        UnityEngine.Debug.Log("save success!");
    }

    public static T Load<T>()
    {
        if (File.Exists(Application.persistentDataPath + "/savegame.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = new FileStream (Application.persistentDataPath + "/savegame.dat", FileMode.Open);

            UnityEngine.Debug.Log("Load success!");

            T loaded = (T)bf.Deserialize(file);
            file.Close();
            return loaded;
        }
        else
        {
            UnityEngine.Debug.Log("File not found!");
        }

        return default(T);
    }

}
