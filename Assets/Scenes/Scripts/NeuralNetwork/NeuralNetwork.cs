using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeuralNetwork
{

    Dictionary<int, List<Neuron>> hiddenNeurons = new Dictionary<int, List<Neuron>>();//list all the neurons by number. 
    Dictionary<int, List<Neuron>> inputNeurons = new Dictionary<int, List<Neuron>>();
    Dictionary<int, List<Neuron>> outputNeurons = new Dictionary<int, List<Neuron>>();
    
    Neuron[] hidden;
    Neuron[] output;

    /// <summary>
    /// Activate all neurons
    /// </summary>
    public void FeedFoward()
    {
        foreach(Neuron neuron in hidden)
        {
            neuron.Activate();
        }
        foreach (Neuron neuron in output)
        {
            neuron.Activate();
        }
    }


    /// <summary>
    /// Creates the neural network by linking the hidden neurons (as described in the chromosome )to the input and output layer
    /// </summary>
    /// <param name="chromosome">chromosome from which to create the NN</param>
    /// <param name="inputLayer">Neurons that give input to the neural network</param>
    /// <param name="outputLayer">neurons that are in the output layer of the neural network</param>
    public void BuildFromChromosome(NeuralChromosome chromosome, Neuron[] inputLayer, Neuron[] outputLayer)
    {
        //Create all the neurons
        CreateHiddenNeurons(chromosome, inputLayer, outputLayer);


        //Add links between neurons
        CreateDendrites(chromosome);
        
        output  = ConvertToArray(outputNeurons);
        hidden = ConvertToArray(hiddenNeurons);
    }
    private Neuron[] ConvertToArray(Dictionary<int,List<Neuron>> map)
    {
        List<Neuron> array= new List<Neuron>();
        foreach (List<Neuron> neurons in map.Values)
        {
            foreach (Neuron neuron in neurons)
            {
                array.Add(neuron);
            }
        }
        return array.ToArray();
    }
    private void CreateDendrites(NeuralChromosome chromosome)
    {
        foreach (DendriteGene dendrite in chromosome.dendriteGenes)
        {
            List<Neuron> startingNeurons = new List<Neuron>();//neurons that will be added as input
            List<Neuron> endNeurons = new List<Neuron>();//neurons that require input

            if (inputNeurons.ContainsKey(dendrite.startNeuron))
            {
                startingNeurons.AddRange(inputNeurons[dendrite.startNeuron]);
            }
            if (hiddenNeurons.ContainsKey(dendrite.startNeuron))
            {
                startingNeurons.AddRange(hiddenNeurons[dendrite.startNeuron]);
            }

            if (outputNeurons.ContainsKey(dendrite.endNeuron))
            {
                endNeurons.AddRange(outputNeurons[dendrite.endNeuron]);
            }
            if (hiddenNeurons.ContainsKey(dendrite.endNeuron))
            {
                endNeurons.AddRange(hiddenNeurons[dendrite.endNeuron]); //hidden neurons can be both start and end
            }


            foreach (Neuron neuron in endNeurons)
            {
                neuron.AddInput(startingNeurons, dendrite.weight);//add all starting neurons as input
            }
        }
    }
    /// <summary>
    /// Creates all hidden neurons and links them to the input and output layer. Fills 
    /// the dictionaries variables
    /// </summary>
    /// <param name="chromosome"></param>
    /// <param name="inputLayer">Neurons that comes from the organism and can be used as input in the neural network</param>
    /// <param name="outputLayer">Neurons that will be used by the organism to make decisions, are the output of the neural network</param>
    private void CreateHiddenNeurons(NeuralChromosome chromosome, Neuron[] inputLayer, Neuron[] outputLayer)
    {
        List<int> hiddenNeuronsNumbers = chromosome.GetNeuronsNumbers();
        
        foreach (Neuron neuron in inputLayer)
        {
            AddNeuron(inputNeurons, neuron);
            hiddenNeuronsNumbers.Remove(neuron.number);
        }
        foreach (Neuron neuron in outputLayer)
        {
            AddNeuron(outputNeurons, neuron);
            hiddenNeuronsNumbers.Remove(neuron.number);
        }

        foreach (int hn in hiddenNeuronsNumbers)
        {
            Neuron neuron = new Neuron(hn);

            AddNeuron(hiddenNeurons, neuron);
        }
    }

    private static void AddNeuron(Dictionary<int, List<Neuron>> neurons, Neuron neuron)
    {
        if (!neurons.ContainsKey(neuron.number))
        {
            neurons[neuron.number] = new List<Neuron>();
        }

        neurons[neuron.number].Add(neuron);
    }

    public override string ToString()
    {
        string inputs = "Input:\n";
        foreach(int value in inputNeurons.Keys)
        {
            foreach(Neuron neuron in inputNeurons[value])
            {
                inputs += neuron.ToString()+"\n";
            }
        }
        string hidS = "Hidden:\n";
        foreach(Neuron neuron in hidden)
        {
            hidS += neuron.ToString() + "\n";

        }
        string outputs = "Output:\n";
        foreach (Neuron neuron in output)
        {
            outputs += neuron.ToString() + "\n";

        }

        return inputs + hidS + outputs;
    }
    public Neuron[] GetInputLayer()
    {
        return ConvertToArray(inputNeurons);
    }
    public Neuron[] GetOutputLayer()
    {
        return output.ToArray();
    }
    public Neuron[] GetHiddenLayer()
    {
        return hidden.ToArray();
    }
}
