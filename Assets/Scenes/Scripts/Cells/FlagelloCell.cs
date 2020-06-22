using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagelloCell : Cell
{
    protected Rigidbody2D body;
    public int consumptionForRunning;

    Vector3 force=new Vector3();
    public Neuron amountNeuron;
    public Neuron angleNeuron;
    public float efficiency;
    // Start is called before the first frame update
    void Start()
    {
        InvokeCellStuff();
        InvokeRepeating("Routine", 1f, SimulationParameters.brainTick);

        efficiency = 0;
        InvokeRepeating("ComputeEfficiency", 0f, 2f);
        body = transform.parent.transform.GetComponent<Rigidbody2D>();

    }
    private void ComputeEfficiency()
    {
        CellsStructure cells = GetComponentInParent<Organism>().cells;

        efficiency = 1;

        for(int i = 0;i <=2; i++)
        {
            for(int j=-1; j<= 1; j++)
            {
                Vector3 vect = attributes.angle*-(new Vector3(j, -i));

                vect = new Vector3((float)Math.Round(vect.x, 3), (float)Math.Round(vect.y, 3));

                CellAttributes cell = cells.GetCell(attributes.relativePosition+vect);

                if(cell!=null && cell.alive && !(cell.type.GetComponent<Cell>() is FlagelloCell))
                {
                    Vector3 v = new Vector3(j*7f, -i);
                    efficiency -= 1/(v.magnitude);
                }

            }
        }
        if (efficiency < 0) efficiency = 0;
    }
    private void Routine()
    {
        Run();
        UseEnergyForRunning();

    }

    private void Run()
    {
        if (attributes.energy.Value > consumptionForRunning)
        {
            force =  Quaternion.Euler(0, 0, 60 * angleNeuron.Value - 30f) * -transform.up * amountNeuron.Value*Hyperparameters.FLAGELLO_FORCE;

            body.AddForceAtPosition(efficiency * force, transform.position);
        }
    }

    private void UseEnergyForRunning()
    {
        int energy = (int)(Math.Round(consumptionForRunning * amountNeuron.Value/2));
        if (attributes.energy.Value <= energy)
        {
            attributes.energy.Value = 0;
        }
        else
        {
            attributes.energy.Value -= energy;
        }
        
    }

    public override List<int> GetOutputNeuronsNumbers(int startingNumber)
    {
        List<int> n = new List<int>();
        n.Add(startingNumber*1000);
        n.Add(startingNumber * 1000+1);
        return n;
    }
    public override List<int> GetInputNeuronsNumbers(int startingNumber)
    {
        return new List<int>();
    }

    public override void SetInputNeurons(List<Neuron> inputNeurons)
    {

    }

    public override void SetOutputNeurons(List<Neuron> outputNeurons)
    {
        amountNeuron = outputNeurons[0];
        angleNeuron = outputNeurons[1];
    }

    public override bool CanLiveAlone()
    {
        return false;
    }
}