using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellAttributes
{
    public Vector3 relativePosition; //position relative to the center of the organism
    public bool alive = true;
    public int health; //current health
    public int number; //number in the organism
    public Quaternion angle;
    public GameObject type;
    public List<Neuron> inputNeurons;
    public List<Neuron> outputNeurons;
    public Cell instance;
    public string name;
    public OrganismEnergy energy;
}
