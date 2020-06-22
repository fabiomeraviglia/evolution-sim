
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SetSimulation : MonoBehaviour
{
    public void Start()
    {


        SimulationInitializer.SetSimulation(new Parameters(), SimulationParameters.game_speed);
        
        SerializationManager.InstantiateSaveObject(SerializationManager.Load());
    }
}