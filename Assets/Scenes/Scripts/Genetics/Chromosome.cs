using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Chromosome : ISerializable
{
    public Gene[] Genes { get; }
    public string MutationTracker { get; }
    public NeuralChromosome neuralChromosome { get; }
    public ChromosomeParameters Parameters { get; }
    public Chromosome(Gene[] genes, NeuralChromosome neuralChromosome, ChromosomeParameters parameters)
    {
        this.Genes = new List<Gene>(genes).ToArray();
        this.MutationTracker = "";
        this.neuralChromosome = neuralChromosome;
        this.Parameters = parameters;
    }
    public Chromosome(Gene[] genes, NeuralChromosome neuralChromosome, string mutationTracker, ChromosomeParameters parameters)
    {
        this.Genes = new List<Gene>(genes).ToArray();
        this.MutationTracker = mutationTracker;
        this.neuralChromosome = neuralChromosome;
        this.Parameters = parameters;
    }
    public Chromosome(Chromosome chromosome)
    {
        this.Genes = new List<Gene>(chromosome.Genes).ToArray();
        this.MutationTracker = chromosome.MutationTracker;
        this.neuralChromosome = chromosome.neuralChromosome;
        this.Parameters = chromosome.Parameters;
    }

    public override string ToString()
    {
        String s = "";
        foreach (Gene g in Genes)
        {
            s += g.ToString() + "\r\n";
        }
        return s;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {

        info.AddValue("Genes", Genes, typeof(Gene[]));
        info.AddValue("MutationTracker", MutationTracker, typeof(string));
        info.AddValue("neuralChromosome", neuralChromosome, typeof(NeuralChromosome));
        info.AddValue("Parameters", Parameters, typeof(ChromosomeParameters));


    }



    public Chromosome(SerializationInfo info, StreamingContext context)
    {

        Genes = (Gene[])info.GetValue("Genes", typeof(Gene[]));

        MutationTracker = (string)info.GetValue("MutationTracker", typeof(string));
        neuralChromosome=(NeuralChromosome)info.GetValue("neuralChromosome", typeof(NeuralChromosome));

        Parameters= (ChromosomeParameters)info.GetValue("Parameters", typeof(ChromosomeParameters));


    }

}


