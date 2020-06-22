

using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class SaveOrganism : ISerializable
{

    public Chromosome chromosome;
    public float x, y;
    public int initialEnergy;
    public SaveOrganism(Organism organism)
    {
        this.chromosome = organism.chromosome;
        this.x = organism.transform.position.x;
        this.y = organism.transform.position.y;
        this.initialEnergy = organism.bodyEnergy + organism.organismEnergy.Value;
    }
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Chromosome", chromosome , typeof(Chromosome));
        info.AddValue("Position.x", x , typeof(float));
        info.AddValue("Position.y", y, typeof(float));
        info.AddValue("InitialEnergy", initialEnergy, typeof(int));

    }
    // The special constructor is used to deserialize values.
    public SaveOrganism(SerializationInfo info, StreamingContext context)
    {
        // Reset the property value using the GetValue method.

        chromosome = (Chromosome)info.GetValue("Chromosome", typeof(Chromosome));
         x = (float)info.GetValue("Position.x", typeof(float));
        y = (float)info.GetValue("Position.y", typeof(float));
        initialEnergy = (int)info.GetValue("InitialEnergy", typeof(int));
        
    }
}