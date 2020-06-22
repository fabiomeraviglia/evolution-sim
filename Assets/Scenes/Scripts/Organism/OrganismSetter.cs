using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class OrganismSetter
{

    public static int BodyEnergy(Chromosome chromosome)
    {
        Dictionary<int, int> amountOfNumber = new Dictionary<int, int>();
        int total = 0;
        foreach (Gene gene in chromosome.Genes)
        {
            int cellsProduced = 1;
            if (amountOfNumber.Any())
            {
                if (amountOfNumber.ContainsKey(gene.StartingCell))
                {
                    cellsProduced = amountOfNumber[gene.StartingCell];
                }
                else continue;
            }

            total += cellsProduced * gene.Type.GetComponent<Cell>().bodyEnergy;

            if (!amountOfNumber.ContainsKey(gene.Number))
            {
                amountOfNumber[gene.Number] = cellsProduced;
            }
            else
            {
                amountOfNumber[gene.Number] += cellsProduced;
            }
        }

        return total;
    }

    public static void SetNeuralNetwork(Organism organism)
    {
        organism.neuralNetwork = new NeuralNetwork();
        List<Neuron> outputNeurons = new List<Neuron>();
        List<Neuron> inputNeurons = new List<Neuron>();
        organism.bias = new Neuron(Organism.GetInputOrganismNeuronsNumbers()[0]);
        organism.bias.Value = 1;

        organism.damageNeuron = new Neuron(Organism.GetInputOrganismNeuronsNumbers()[1]);
        organism.energyNeuron = new Neuron(Organism.GetInputOrganismNeuronsNumbers()[2]);

        organism.mitosisNeuron = new Neuron(Organism.GetOutputOrganismNeuronsNumbers()[0]);
        organism.sexualReproductionNeuron = new Neuron(Organism.GetOutputOrganismNeuronsNumbers()[1]);

        inputNeurons.Add(organism.bias);
        inputNeurons.Add(organism.damageNeuron);
        inputNeurons.Add(organism.energyNeuron);
        outputNeurons.Add(organism.mitosisNeuron);
        outputNeurons.Add(organism.sexualReproductionNeuron);

        foreach (var pair in organism.orderOfGrowth)
        {
            CellAttributes cell = pair.Value;
            cell.inputNeurons = Organism.CreateNeuronsFromNumbers(cell.type.GetComponent<Cell>().GetInputNeuronsNumbers(cell.number));
            cell.outputNeurons = Organism.CreateNeuronsFromNumbers(cell.type.GetComponent<Cell>().GetOutputNeuronsNumbers(cell.number));

            inputNeurons.AddRange(cell.inputNeurons);
            outputNeurons.AddRange(cell.outputNeurons);
        }

        organism.neuralNetwork.BuildFromChromosome(organism.chromosome.neuralChromosome, inputNeurons.ToArray(), outputNeurons.ToArray());
    }

    public static void SetDrag(Organism organism)
    {

        float angularDragMultiplier = 0.3f;

        if (organism.transform.childCount == 0)
        {
            organism.GetComponent<Rigidbody2D>().angularDrag = 0.3f;
            return;
        }

        Bounds bounds = organism.transform.GetChild(0).GetComponent<Collider2D>().bounds;
        for (int i = 1; i < organism.transform.childCount; i++)
        {
            Collider2D collider = organism.transform.GetChild(i).GetComponent<Collider2D>();

            bounds.Encapsulate(collider.bounds);

        }
        organism.GetComponent<Rigidbody2D>().angularDrag = bounds.extents.magnitude * angularDragMultiplier;
    }
    public static void SetName(Organism organism)
    {//sets a name that is representative of the cell structure of the organism
        int maxLength = 4;//max number of strings linked together
        int compressionLevel = 4; //number of cells represented by a single letter, can change based on max LengTH


        List<CellAttributes> myList = new List<CellAttributes>();
        foreach (var pair in organism.orderOfGrowth)
        {
            myList.Add(pair.Value);
        }

        myList.Sort(new CellComparer());

        String name = "";
        int pos = 0, i = 1;
        compressionLevel = Math.Max(compressionLevel, myList.Count / maxLength + 1);


        foreach (CellAttributes cell in myList)
        {
            pos += cell.type.name.GetHashCode();
            if (i % compressionLevel == 0)
            {
                pos = Math.Abs(pos) % GenesManager.letters.Length;
                name += GenesManager.letters[pos];
                pos = 0;
            }
            i++;
        }
        if (i % compressionLevel != 1) name += GenesManager.letters[Math.Abs(pos) % GenesManager.letters.Length];
        organism.name = name;
    }

    private class CellComparer : IComparer<CellAttributes>
    {
        public int Compare(CellAttributes c1, CellAttributes c2)
        {
            if (c1.relativePosition.x > c2.relativePosition.x) return 1;
            if (c1.relativePosition.x < c2.relativePosition.x) return -1;
            if (c1.relativePosition.y > c2.relativePosition.y) return 1;
            if (c1.relativePosition.y < c2.relativePosition.y) return -1;
            return 0;
        }
    }

}
