using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class NeuralChromosome
{
    public DendriteGene[] dendriteGenes;


    public NeuralChromosome()
    {
        this.dendriteGenes = new DendriteGene[0];
    }

    public NeuralChromosome(DendriteGene[] dendriteGenes)
    {
        this.dendriteGenes = dendriteGenes;
    }
    
    /// <summary>
    /// Returns all the numbers referenced by the dendrites in this chromosome. Can be inputs, hidden and output neurons
    /// </summary>
    /// <returns></returns>
    public List<int> GetNeuronsNumbers()
    {
        HashSet<int> numbers = new HashSet<int>();

        foreach(DendriteGene dendrite in dendriteGenes)
        {
            numbers.Add(dendrite.startNeuron);

            numbers.Add(dendrite.endNeuron);
        }
        return numbers.ToList();
    }

    public DendriteGene.ComparableGene[] GetComparableGenes()
    {
        DendriteGene.ComparableGene[] comparable = new DendriteGene.ComparableGene[dendriteGenes.Length];
        for (int i =0; i<comparable.Length; i++)
        {
            comparable[i] = dendriteGenes[i].GetComparableGene();
        }
        return comparable;
    }

    public override bool Equals(object obj)
    {
        return obj is NeuralChromosome chromosome &&
               dendriteGenes.SequenceEqual(chromosome.dendriteGenes);
    }

    public override int GetHashCode()
    {
        int hash = 145208284 + dendriteGenes.Length;
        for(int i = 0; i<dendriteGenes.Length&& i<4;i++)
        {
            hash += 3 * dendriteGenes[i].GetHashCode();
        }
        return hash;

    }

}
[Serializable]
public class DendriteGene
{

    public readonly int startNeuron;
    public readonly int endNeuron;
    public readonly float weight;

    public DendriteGene(int startNeuron, int endNeuron, float weight)
    {
        this.startNeuron = startNeuron;
        this.endNeuron = endNeuron;
        this.weight = weight;
    }

    public override bool Equals(object obj)
    {
        var gene = obj as DendriteGene;
        return gene != null &&
               startNeuron == gene.startNeuron &&
               endNeuron == gene.endNeuron &&
               weight == gene.weight;
    }

    public override int GetHashCode()
    {
        var hashCode = -1723750910;
        hashCode = hashCode * -1521134295 + startNeuron.GetHashCode();
        hashCode = hashCode * -1521134295 + endNeuron.GetHashCode();
        hashCode = hashCode * -1521134295 + weight.GetHashCode();
        return hashCode;
    }

    public ComparableGene GetComparableGene()
    {
        return new ComparableGene(this);
    }

    public class ComparableGene
    {
        private int startNeuron, endNeuron;
        private DendriteGene dendrite;
        public ComparableGene(DendriteGene dendrite)
        {
            this.startNeuron = dendrite.startNeuron;
            this.endNeuron = dendrite.endNeuron;
            this.dendrite = dendrite;
        }

        public override bool Equals(object obj)
        {
            var gene = obj as ComparableGene;
            return gene != null &&
                   startNeuron == gene.startNeuron &&
                   endNeuron == gene.endNeuron;
        }

        public override int GetHashCode()
        {
            var hashCode = -739949279;
            hashCode = hashCode * -1521134295 + startNeuron.GetHashCode();
            hashCode = hashCode * -1521134295 + endNeuron.GetHashCode();
            return hashCode;
        }

        public DendriteGene GetGene()
        {
            return dendrite;
        }
    }
}
