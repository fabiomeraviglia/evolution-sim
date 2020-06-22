using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Load : MonoBehaviour
{
    public void LoadOrganisms()
    {
        var data = SerializationManager.Load();
        if (data != null)
        {
            SerializationManager.InstantiateSaveObject(data);
        }
    }
}
