using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Gene: ISerializable
{
    public int StartingCell { get; }
    public Vector3 RelativePosition { get; }
    public GameObject Type { get; }
    public int Number { get; }

    public Gene(int startingCell, Vector3 relativePosition, GameObject type, int number)
    {
        this.StartingCell = startingCell;
        this.RelativePosition = new Vector3(relativePosition.x, relativePosition.y);
        this.Type = type;
        this.Number = number;
    }

    public Gene(Gene gene)
    {
        this.StartingCell = gene.StartingCell;
        this.RelativePosition =new Vector3(gene.RelativePosition.x, gene.RelativePosition.y);
        this.Type = gene.Type;
        this.Number = gene.Number;
    }

    public override bool Equals(object obj)
    {
        var gene = obj as Gene;
        return gene != null &&
               StartingCell == gene.StartingCell &&
               RelativePosition.x==gene.RelativePosition.x && RelativePosition.y == gene.RelativePosition.y &&
               EqualityComparer<GameObject>.Default.Equals(Type, gene.Type) &&
               Number == gene.Number;
    }

    public override int GetHashCode()
    {
        var hashCode = 1461567088;
        hashCode = hashCode * -1521134295 + StartingCell.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(RelativePosition);
        hashCode = hashCode * -1521134295 + EqualityComparer<GameObject>.Default.GetHashCode(Type);
        hashCode = hashCode * -1521134295 + Number.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return "[N:" + Number + " S:" + StartingCell + " (" + RelativePosition.x + "," + RelativePosition.y + ") " + Type.name+"]";
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("StartingCell", StartingCell, typeof(int));
        info.AddValue("Number", Number, typeof(int));
        info.AddValue("RelativePosition.x", RelativePosition.x, typeof(float));
        info.AddValue("RelativePosition.y", RelativePosition.y, typeof(float));
        info.AddValue("Type", Type.name, typeof(string));

    }
    public Gene(SerializationInfo info, StreamingContext context)
    {
        // Reset the property value using the GetValue method.

        StartingCell= (int)info.GetValue("StartingCell", typeof(int));
        Number= (int)info.GetValue("Number",  typeof(int));
        float x= (float)info.GetValue("RelativePosition.x",  typeof(float));
        float y = (float)info.GetValue("RelativePosition.y", typeof(float));
        RelativePosition = new Vector3(x, y);

        string name = (string)info.GetValue("Type", typeof(string));
        GameObject[] cellTypes = GameObject.FindGameObjectWithTag("organismspawner").GetComponent<OrganismSpawn>().cellTypes;
        foreach(GameObject cell in cellTypes)
        {
            if(name==cell.name)
            {
                Type = cell;
                break;
            }
        }

    }
}