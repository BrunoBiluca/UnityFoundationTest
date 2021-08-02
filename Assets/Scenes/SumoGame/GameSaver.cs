using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaver : Singleton<GameSaver>
{
    public void Save<T>(string fileName, T obj)
    {
        var binaryFormatter = new BinaryFormatter();
        using var file = File.Create($"{Application.persistentDataPath}/{fileName}");

        binaryFormatter.Serialize(file, obj);
    }

    public T Load<T>(string fileName)
    {
        var binaryFormatter = new BinaryFormatter();
        using var file = File.Open(
            $"{Application.persistentDataPath}/{fileName}", 
            FileMode.Open
        );

        return (T)binaryFormatter.Deserialize(file);
    }
}