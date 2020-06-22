using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Neuron
{
    
    public float Value { get; set; }
    public List< float> weights;
    public List<Neuron> inputNeurons;
    public int number;


    public Neuron(int number)
    {
        this.number = number;
        weights = new List<float>();
        inputNeurons = new List<Neuron>();
    }
    

    public void Activate()
    {
        float sum = 0;
        for(int i = 0; i<inputNeurons.Count; i++)
        {
            sum += inputNeurons[i].Value * weights[i];
        }
        Value = Sigmoid(sum);
    }

    public static float Sigmoid(float value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }

    internal void AddInput(List<Neuron> startingNeurons, float weight)
    {
        foreach(Neuron neuron in startingNeurons)
        {
            inputNeurons.Add(neuron);
            weights.Add(weight);
        }
    }

    public override string ToString()
    {
        return inputNeurons.Count()+"->(" + number + ")->"+Value;
    }
}
