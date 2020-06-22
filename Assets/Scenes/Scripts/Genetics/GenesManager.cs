using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GenesManager
{
    public static System.Random r = new System.Random();
    public static double mutationRate = Hyperparameters.MUTATION_PROBABILITY; //probability to have at least a mutation in a chromosome
    public static double neuralMutationRate = Hyperparameters.NEURAL_MUTATION_PROBABILITY;


    /// <summary>
    /// Creates new genes considering mutations 
    /// </summary>
    /// <param name="gene"></param>
    ///
    public static Chromosome CreateNewGenesForReproduction(Chromosome chromosome)
    {
        int maxNumberOfMutations = 20;
        int mutations = 0;

        Chromosome mutatedChromosome = new Chromosome(chromosome);
        //mutate genes
        while (r.NextDouble() < mutationRate && mutations < maxNumberOfMutations)
        {
            mutatedChromosome = MutateChromosome(mutatedChromosome);
            mutations++;
        }

        //now mutate neural network
        mutations = 0;
        while (r.NextDouble() < neuralMutationRate && mutations < maxNumberOfMutations)
        {
            mutatedChromosome = new Chromosome(mutatedChromosome.Genes, NeuralMutator.MutateNeuralChromosome(mutatedChromosome), mutatedChromosome.MutationTracker, mutatedChromosome.Parameters);
            mutations++;
        }

        //remove inconsistencies
        mutatedChromosome = new Chromosome(RemoveInconsistencies(mutatedChromosome.Genes), mutatedChromosome.neuralChromosome, mutatedChromosome.MutationTracker, mutatedChromosome.Parameters);


        return mutatedChromosome;
    }

    public static Chromosome CreateNewGenesForReproduction(Chromosome ch1, Chromosome ch2)
    {

        return CreateNewGenesForReproduction(CreateCrossover(ch1, ch2));
    }

    public static Chromosome CreateCrossover(Chromosome ch1, Chromosome ch2)
    {
        Gene[] genes = CrossoverManager.Crossover(ch1.Genes, ch2.Genes);


        var neuralComparableGenes = CrossoverManager.Crossover(ch1.neuralChromosome.GetComparableGenes(), ch2.neuralChromosome.GetComparableGenes());
        DendriteGene[] dendriteGenes = new DendriteGene[neuralComparableGenes.Length];
        for (int i = 0; i < dendriteGenes.Length; i++)
        {
            dendriteGenes[i] = neuralComparableGenes[i].GetGene();
        }


        NeuralChromosome neuralChromosome = new NeuralChromosome(dendriteGenes);

        ChromosomeParameters parameters = CrossoverManager.CrossParameters(ch1.Parameters, ch2.Parameters);

        Chromosome chromosome = new Chromosome(genes, neuralChromosome, ch1.MutationTracker + "CR", parameters);

        return chromosome;
    }

    public static string[] letters = { "qua", "que", "qui", "quo", "qu", "wa", "we", "wi", "wo", "wu", "ra", "re", "ri", "ro", "ru", "ta", "te", "ti", "to", "tu", "ya", "ye", "yo", "yu", "ea", "ee", "ei", "eo", "eu", "ua", "ue", "ui", "uo", "pa", "pe", "pi", "po", "pu", "aa", "ae", "ai", "ao", "au", "sa", "se", "si", "so", "su", "da", "de", "di", "do", "du", "fa", "fe", "fi", "fo", "fu", "ga", "ge", "gi", "go", "gu", "ha", "he", "hi", "ho", "hu", "ja", "je", "ji", "jo", "ju", "ka", "ke", "ki", "ko", "ku", "la", "le", "li", "lo", "lu", "za", "ze", "zi", "zo", "zu", "xa", "xe", "xi", "xo", "xu", "ca", "ce", "ci", "co", "cu", "va", "ve", "vi", "vo", "vu", "ba", "be", "bi", "bo", "bu", "na", "ne", "ni", "no", "nu", "ma", "me", "mi", "mo", "mu", "fra", "fre", "fri", "fro", "fru", "cra", "cre", "cri", "cro", "cru", "gra", "gre", "gri", "gro", "gru", "tra", "tre", "tri", "tro", "tru", "spa", "spe", "spi", "spo", "spu", "sta", "ste", "sti", "sto", "stu", "n", "r", "shi", "sha", "she", "sho", "shu", "pra", "pre", "pri", "pro", "pru" };

    private static string MutateName(string name)
    {
        int pos = r.Next(name.Length);
        string val = letters[r.Next(letters.Length)];
        string mutatedName = name.Remove(pos, 1).Insert(pos, val);
        return mutatedName;
    }

    private static Chromosome MutateChromosome(Chromosome chromosome)
    {
        Double randomValue = r.NextDouble();

        Chromosome mutatedChromosome = new Chromosome(chromosome.Genes, chromosome.neuralChromosome, chromosome.MutationTracker, MutateParameters(chromosome.Parameters));

        double total = Hyperparameters.ELIMINATION_PROBABILITY + Hyperparameters.INSERTION_PROBABILITY + Hyperparameters.MODIFICATION_PROBABILITY + Hyperparameters.TRANSPOSITION_PROBABILITY + Hyperparameters.SPECIALIZATION_PROBABILITY;
        double eliminationThreshold = Hyperparameters.ELIMINATION_PROBABILITY / total;
        double insertionThreshold = eliminationThreshold + Hyperparameters.INSERTION_PROBABILITY / total;
        double transpositionThreshold = insertionThreshold + Hyperparameters.TRANSPOSITION_PROBABILITY / total;
        double modificationThreshold = transpositionThreshold + Hyperparameters.MODIFICATION_PROBABILITY / total;

        if (randomValue < eliminationThreshold)
        {//Elimination
            return Elimination(mutatedChromosome);
        }

        if (randomValue < insertionThreshold)
        {//insertion
            return Insertion(mutatedChromosome);
        }

        if (randomValue < transpositionThreshold)
        {//transposition
            return Transposition(mutatedChromosome);
        }
        //modification
        if (randomValue < modificationThreshold)
        {
            return Modification(mutatedChromosome);
        }

        return Specialization(mutatedChromosome);


    }

    private static ChromosomeParameters MutateParameters(ChromosomeParameters parameters)
    {

        double[] param = parameters.ToArray();
        for (int i = 0; i < param.Length; i++)
        {
            param[i] = MutateParameter(param[i], 0, 1);
        }

        return new ChromosomeParameters(param);
    }

    private static double MutateParameter(double parameter, double min, double max)
    {
        return MutateParameter(parameter, min, max, 1 + mutationRate);
    }
    private static double MutateParameter(double parameter, double min, double max, double mutationMagnitude)
    {
        double mutated = parameter * Math.Pow(mutationMagnitude, (r.NextDouble() * 2 - 1));
        if (mutated > max) return max;
        if (mutated < min) return min;
        return mutated;

    }
    private static Chromosome Specialization(Chromosome chromosome)
    {
        String tracker = "";
        List<Gene> newGenes = new List<Gene>();

        int pos = r.Next(chromosome.Genes.Length);

        for (int i = 0; i < pos; i++)
        {
            newGenes.Add(chromosome.Genes[i]);
        }

        int newCellNumber = GetNewCellNumber(GetCellsNumbers(chromosome.Genes));
        int oldCellNumber = chromosome.Genes[pos].Number;
        newGenes.Add(new Gene(chromosome.Genes[pos].StartingCell, chromosome.Genes[pos].RelativePosition, chromosome.Genes[pos].Type, newCellNumber));

        for (int i = pos + 1; i < chromosome.Genes.Length; i++)
        {
            newGenes.Add(chromosome.Genes[i]);
            if (chromosome.Genes[i].StartingCell == oldCellNumber)
            {
                Gene newGene = new Gene(newCellNumber, chromosome.Genes[i].RelativePosition, chromosome.Genes[i].Type, chromosome.Genes[i].Number);
                newGenes.Add(newGene);
            }
        }

        tracker = "S"+ (newGenes.Count - chromosome.Genes.Length);
        return new Chromosome(newGenes.ToArray(), chromosome.neuralChromosome, chromosome.MutationTracker + tracker, chromosome.Parameters);
    }

    private static Chromosome Modification(Chromosome chromosome)
    {
        String tracker = "";
        Gene[] newGenes = new Gene[chromosome.Genes.Length];

        for (int i = 0; i < newGenes.Length; i++)
            newGenes[i] = new Gene(chromosome.Genes[i]);

        int pos = r.Next(newGenes.Length);
        Gene gene = newGenes[pos];
        int startingCell = gene.StartingCell;
        Vector3 relPos = gene.RelativePosition;
        GameObject type = gene.Type;
        int number = gene.Number;

        double rVal = r.NextDouble();
        List<int> cellNumbers = GetCellsNumbers(chromosome.Genes);

        if (rVal < 0.25)
        {
            startingCell = GetRandomStartingCell(cellNumbers);
            tracker = "M1";
        }
        else
        {
            if (rVal < 0.5)
            {
                relPos = GetRandomRelativePosition();
                tracker = "M2";
            }
            else
            {
                if (rVal < 0.75)
                {
                    type = GetRandomCellType();
                    tracker = "M3";
                }
                else
                {
                    number = GetRandomCellNumber(cellNumbers);
                    tracker = "M4";
                }
            }
        }

        newGenes[pos] = new Gene(startingCell, relPos, type, number);

        return new Chromosome(newGenes, chromosome.neuralChromosome, chromosome.MutationTracker + tracker, chromosome.Parameters);
    }

    private static Chromosome Transposition(Chromosome chromosome)
    {
        Gene[] newGenes = new Gene[chromosome.Genes.Length];

        for (int i = 0; i < chromosome.Genes.Length; i++)
        {
            newGenes[i] = new Gene(chromosome.Genes[i]);
        }

        int pos1 = r.Next(chromosome.Genes.Length);
        int pos2 = r.Next(chromosome.Genes.Length);

        Gene tmp = newGenes[pos1];
        newGenes[pos1] = new Gene(newGenes[pos1].StartingCell, newGenes[pos1].RelativePosition, newGenes[pos2].Type, newGenes[pos1].Number);
        newGenes[pos2] = new Gene(newGenes[pos2].StartingCell, newGenes[pos2].RelativePosition, tmp.Type, newGenes[pos2].Number); ;

        return new Chromosome(newGenes, chromosome.neuralChromosome, chromosome.MutationTracker + "T" + (Math.Abs(pos1 - pos2)), chromosome.Parameters);
    }

    private static Chromosome Insertion(Chromosome chromosome)
    {
        Gene[] newGenes = new Gene[chromosome.Genes.Length + 1];
        int pos = r.Next(chromosome.Genes.Length + 1);

        for (int i = 0; i < pos; i++)
            newGenes[i] = new Gene(chromosome.Genes[i]);

        newGenes[pos] = CreateNewGene(chromosome.Genes);

        for (int i = pos; i < chromosome.Genes.Length; i++)
            newGenes[i + 1] = new Gene(chromosome.Genes[i]);

        return new Chromosome(newGenes, chromosome.neuralChromosome, chromosome.MutationTracker + "I" + pos, chromosome.Parameters);
    }

    private static Gene CreateNewGene(Gene[] chromosome)
    {
        if (r.NextDouble() < Hyperparameters.INSERTION_COPY_PROBABILITY)
            return chromosome[r.Next(chromosome.Length)];

        List<int> cellsNumbers = GetCellsNumbers(chromosome);
        int startingCell = GetRandomStartingCell(cellsNumbers);
        Vector3 relativePosition = GetRandomRelativePosition();

        GameObject type = GetRandomCellType();

        int number = GetRandomCellNumber(cellsNumbers);

        return new Gene(startingCell, relativePosition, type, number);
    }
    public static int GetNewCellNumber(List<int> cellsNumbers)
    {
        if (!cellsNumbers.Any()) return 1;

        return cellsNumbers.Max() + 1;
    }
    public static int GetRandomCellNumber(List<int> cellsNumbers)
    {
        if (!cellsNumbers.Any()) return 1;

        if (r.NextDouble() > Hyperparameters.INSERTION_NEWNUMBER_PROBABILITY)
            return cellsNumbers[r.Next(cellsNumbers.Count)];

        return GetNewCellNumber(cellsNumbers);
    }

    private static GameObject GetRandomCellType()
    {
        return OrganismSpawn.organismSpawner.cellTypes[r.Next(OrganismSpawn.organismSpawner.cellTypes.Length)];
    }

    public static int GetRandomStartingCell(List<int> cellsNumbers)
    {
        return cellsNumbers[r.Next(cellsNumbers.Count)];
    }

    private static Vector3 GetRandomRelativePosition()
    {
        Vector3 relativePosition;
        double rVal = r.NextDouble();
        if (rVal < 0.25) relativePosition = new Vector3(0, 1);
        else
        {
            if (rVal < 0.5) relativePosition = new Vector3(0, -1);
            else
            {
                if (rVal < 0.75) relativePosition = new Vector3(1, 0);
                else relativePosition = new Vector3(-1, 0);
            }
        }

        return relativePosition;
    }

    public static List<int> GetCellsNumbers(Gene[] chromosome)
    {
        HashSet<int> cellsNumbers = new HashSet<int>();
        foreach (Gene gene in chromosome)
        {
            cellsNumbers.Add(gene.Number);
        }
        return cellsNumbers.ToList();
    }

    private static Chromosome Elimination(Chromosome chromosome)
    {
        if (chromosome.Genes.Length <= 1) return chromosome; //no elimination if only one gene
        Gene[] newGenes = new Gene[chromosome.Genes.Length - 1];
        int pos = r.Next(chromosome.Genes.Length);

        for (int i = 0; i < pos; i++)
            newGenes[i] = new Gene(chromosome.Genes[i]);
        for (int i = pos; i < chromosome.Genes.Length - 1; i++)
            newGenes[i] = new Gene(chromosome.Genes[i + 1]);

        return new Chromosome(newGenes, chromosome.neuralChromosome, chromosome.MutationTracker + "E" + pos, chromosome.Parameters);
    }

    private static Gene[] RemoveInconsistencies(Gene[] genes)
    {
        if (genes.Length <= 1)
            return genes;

        List<Gene> newGenes = new List<Gene>();

        HashSet<int> numbers = new HashSet<int>();
        numbers.Add(genes[0].Number);
        newGenes.Add(genes[0]);
        for (int i = 1; i < genes.Length; i++)
        {
            if (numbers.Contains(genes[i].StartingCell))
            {
                numbers.Add(genes[i].Number);
                newGenes.Add(genes[i]);
            }
        }

        return newGenes.ToArray();
    }


    public static Dictionary<KeyValuePair<Chromosome, Chromosome>, bool> sameSpecieComputed = new Dictionary<KeyValuePair<Chromosome, Chromosome>, bool>();

    public static bool SameSpecie(Chromosome ch1, Chromosome ch2)
    {
        KeyValuePair<Chromosome,Chromosome> pair = new KeyValuePair<Chromosome, Chromosome>(ch1, ch2);
        if (sameSpecieComputed.ContainsKey(pair))
        {
            var res = sameSpecieComputed[pair];
            if (sameSpecieComputed.Count > 50000)
            {
                sameSpecieComputed.Clear();
            }
            return res;
        }

        int disjointGenes = CrossoverManager.Difference(ch1.Genes, ch2.Genes);

        float differenceValue = 2 * disjointGenes / (float)Math.Max(ch1.Genes.Length, ch2.Genes.Length);
        if (differenceValue > Hyperparameters.SPECIES_DIFFERENCE_THRESHOLD)
        {
            sameSpecieComputed[pair] = false;
            return false;
        }

        //neural part:
        /*
        int disjointNeuralGenes = CrossoverManager.Difference(ch1.neuralChromosome.GetComparableGenes(), ch2.neuralChromosome.GetComparableGenes());
        differenceValue = 2 * disjointNeuralGenes / (float)Math.Max(ch1.neuralChromosome.dendriteGenes.Length, ch2.neuralChromosome.dendriteGenes.Length);

        if (differenceValue > 1)
            return false;
        */

        sameSpecieComputed[pair] = true;
        return true;
    }


}

