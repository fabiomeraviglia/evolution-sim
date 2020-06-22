using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NeuralMutator
{
    private static double structureModificationProbability = 0.08;
    private static System.Random r = GenesManager.r;
    internal static NeuralChromosome MutateNeuralChromosome(Chromosome chromosome)
    {
        double randomValue = r.NextDouble();


        if (randomValue < structureModificationProbability)
        {//insertion
            return StructureModification(chromosome.neuralChromosome, InputNeuronsNumbers(chromosome), OutputNeuronsNumbers(chromosome));
        }


        //modification
        return WeightModification(chromosome.neuralChromosome);


    }

    private static HashSet<int> OutputNeuronsNumbers(Chromosome chromosome)
    {
        HashSet<int> numbers = new HashSet<int>();
        foreach (Gene gene in chromosome.Genes)
        {
            numbers.UnionWith(gene.Type.GetComponent<Cell>().GetOutputNeuronsNumbers(gene.Number));

        }
        numbers.UnionWith(Organism.GetOutputOrganismNeuronsNumbers());
        return numbers;
    }

    private static HashSet<int> InputNeuronsNumbers(Chromosome chromosome)
    {
        HashSet<int> numbers = new HashSet<int>();
        foreach (Gene gene in chromosome.Genes)
        {
            numbers.UnionWith(gene.Type.GetComponent<Cell>().GetInputNeuronsNumbers(gene.Number));
        }

        numbers.UnionWith(Organism.GetInputOrganismNeuronsNumbers());
        return numbers;
    }

    private static NeuralChromosome WeightModification(NeuralChromosome chromosome)
    {

        if (chromosome.dendriteGenes.Length == 0)
            return chromosome;

        return ModifyWeights(chromosome);


    }

    private static NeuralChromosome ModifyWeights(NeuralChromosome chromosome)
    {
        DendriteGene[] dendriteGenes = new DendriteGene[chromosome.dendriteGenes.Length];

        for (int i = 0; i < dendriteGenes.Length; i++)
        {
            dendriteGenes[i] = new DendriteGene(chromosome.dendriteGenes[i].startNeuron, chromosome.dendriteGenes[i].endNeuron, ModifyWeight(chromosome.dendriteGenes[i].weight));
        }
        /*
        int pos = r.Next(dendriteGenes.Length);
        dendriteGenes[pos] = new DendriteGene(dendriteGenes[pos].startNeuron, dendriteGenes[pos].endNeuron, ModifyWeight(dendriteGenes[pos].weight));
        */
        chromosome = new NeuralChromosome(dendriteGenes);

        return chromosome;
    }


    private static float ModifyWeight(float weight)
    {
        if (r.NextDouble() < 0.01)
        {
            return (float)r.NextDouble() * 6 - 3;
        }

        return (float)(1 + (r.NextDouble() - 0.5) / 8) * weight;

    }

    private static NeuralChromosome StructureModification(NeuralChromosome chromosome, HashSet<int> inputNeuronsNumbers, HashSet<int> outputNeuronsNumbers)
    {
        if (chromosome.dendriteGenes.Length > 0 && r.NextDouble() < 0.35)
        {
            if (r.NextDouble() < 0.1)
            {
                return Elimination(chromosome);
            }
            return InsertNeuron(chromosome);
        }
        return InsertDendrite(chromosome, inputNeuronsNumbers, outputNeuronsNumbers);
    }

    private static NeuralChromosome InsertDendrite(NeuralChromosome chromosome, HashSet<int> inputNeuronsNumbers, HashSet<int> outputNeuronsNumbers)
    {
        HashSet<int> startingNeurons = new HashSet<int>();//neurons that will be added as input
        HashSet<int> endNeurons = new HashSet<int>();//neurons that require input
        List<int> neuronsInChromosome = chromosome.GetNeuronsNumbers();

        startingNeurons.UnionWith(inputNeuronsNumbers);
        startingNeurons.UnionWith(neuronsInChromosome);
        foreach (int n in outputNeuronsNumbers)
        {
            startingNeurons.Remove(n);
        }

        endNeurons.UnionWith(outputNeuronsNumbers);
        endNeurons.UnionWith(neuronsInChromosome);
        foreach (int n in inputNeuronsNumbers)
        {
            endNeurons.Remove(n);
        }
        
        if (!startingNeurons.Any() || !endNeurons.Any())
            return chromosome;

        int startingNeuron = startingNeurons.ToArray()[r.Next(startingNeurons.Count)];
        int endingNeuron = endNeurons.ToArray()[r.Next(endNeurons.Count)];

        HashSet< DendriteGene.ComparableGene> existingGenes = new HashSet<DendriteGene.ComparableGene>();
        foreach(DendriteGene dendrite in chromosome.dendriteGenes)
        {
            existingGenes.Add(dendrite.GetComparableGene());
        }
        if(existingGenes.Contains(new DendriteGene.ComparableGene(new DendriteGene(startingNeuron,endingNeuron, 0))))
        {
            return chromosome;
        }

        DendriteGene[] dendriteGenes = new DendriteGene[chromosome.dendriteGenes.Length + 1];
        for (int i = 0; i < chromosome.dendriteGenes.Length; i++)
        {
            dendriteGenes[i] = chromosome.dendriteGenes[i];
        }
        
        dendriteGenes[chromosome.dendriteGenes.Length] = new DendriteGene(startingNeuron, endingNeuron, (float)r.NextDouble() * 2 - 1);

        return new NeuralChromosome(dendriteGenes);
    }

    private static NeuralChromosome InsertNeuron(NeuralChromosome chromosome)
    {

        List<int> numbers = chromosome.GetNeuronsNumbers();

        int newNumber;
        if (numbers.Any())
        {
            newNumber = numbers.Max() + 1;
        }
        else
        {
            newNumber = 1;
        }

        //substitute a existing dendrite
        int pos = r.Next(chromosome.dendriteGenes.Length);
        DendriteGene dendrite = chromosome.dendriteGenes[pos];

        DendriteGene[] dendriteGenes = new DendriteGene[chromosome.dendriteGenes.Length + 1];
        for (int i = 0; i < chromosome.dendriteGenes.Length; i++)
        {
            dendriteGenes[i] = chromosome.dendriteGenes[i];
        }


        dendriteGenes[chromosome.dendriteGenes.Length] = new DendriteGene(newNumber, dendriteGenes[pos].endNeuron, dendriteGenes[pos].weight);

        dendriteGenes[pos] = new DendriteGene(dendrite.startNeuron, newNumber, 1f);



        return new NeuralChromosome(dendriteGenes);
    }

    private static NeuralChromosome Elimination(NeuralChromosome chromosome)
    {
        if (chromosome.dendriteGenes.Length == 0)
            return chromosome;


        int pos = r.Next(chromosome.dendriteGenes.Length);

        DendriteGene[] dGenes = new DendriteGene[chromosome.dendriteGenes.Length - 1];

        for (int i = 0; i < pos; i++)
        {
            dGenes[i] = chromosome.dendriteGenes[i];
        }

        for (int i = pos; i < dGenes.Length; i++)
        {
            dGenes[i] = chromosome.dendriteGenes[i + 1];
        }


        return new NeuralChromosome(dGenes);
    }
}

