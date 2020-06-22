using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Optimizator : MonoBehaviour
{


    public static System.Random r = new System.Random();

    public static float RUNNING_SPEED = 800;
    public static int population_size = 5;
    public static float singleSimulationTime =  35*60;
    public static Dictionary<Parameters, ParameterFitness> fitnesses = new Dictionary<Parameters, ParameterFitness>();
    public static Parameters[] population;
    Stopwatch generationTimer, simulationTimer;

    public Text text;
    public double _current_best_fitness = 0;
    public double _current_best_fitness_variance = 0;
    public int _current_simulation_timer = 0;
    public int _generation = 0;



    public void Start()
    {

        Load();
        CreateFirstPopulation();

        generationTimer = new Stopwatch();
        simulationTimer = new Stopwatch();


        StartCoroutine(Run());
        
    }

    private void CreateFirstPopulation()
    {
        population = new Parameters[population_size];
        int loaded = 0;
        if (fitnesses.Any())
        {
            var myList = fitnesses.ToList();

            myList.Sort((pair1, pair2) => -pair1.Value.average.CompareTo(pair2.Value.average));

            while (loaded < population_size && loaded < myList.Count)
            {
                population[loaded] = myList[loaded].Key;
                loaded++;
            }

        }
        for (int i = loaded; i < population_size; i++)
        {
            population[i] = new Parameters();

            //completely randomize some of the values
            for (int j = 0; j < i * 10; j++)
            {
                population[i] = MutationManager.Mutate(population[i]);
            }
        }
        
    }

    bool completed = false;


    public IEnumerator Run()
    {
        generationTimer.Start();
        while (true)
        {
            _generation++;

            completed = false;

            StartCoroutine(RunGeneration());
            while (completed == false)
            {
                yield return new WaitForSeconds(1);
            }
            generationTimer.Restart();
            
        }
    }

    public IEnumerator RunGeneration()
    {

        for (int i = 0; i < population.Length; i++)
        {


            ParameterFitness pf = new ParameterFitness();
            if (fitnesses.ContainsKey(population[i]))
            {
                pf = fitnesses[population[i]];

                if (i > 1 || (2 * pf.GetVariance() / Math.Sqrt(pf.n) < 0.1 * pf.average && r.Next(pf.n+1)!=0))
                    continue;
            }

            SimulationInitializer.SetSimulation(population[i], RUNNING_SPEED);
            simulationTimer.Restart();
            while (simulationTimer.Elapsed.TotalSeconds < singleSimulationTime)
            {
                yield return new WaitForSeconds(1);
                _current_simulation_timer = (int)simulationTimer.Elapsed.TotalSeconds;
            }
            double fitness = GetFitness();
            UpdateFitnesses(population[i], fitness);

            if (_current_best_fitness < fitness)
            {
                _current_best_fitness = fitness;
                _current_best_fitness_variance = fitnesses[population[i]].GetVariance();
            }
            text.text = "i=" + i + " gen: " + _generation + " timer:" + _current_simulation_timer + "  fitness = " + fitness + "avg:" + pf.average + " var=" + pf.GetVariance() + " n:" + pf.n + " Z:" + (2 * pf.GetVariance() / Math.Sqrt(pf.n)) + "<" + (0.1 * pf.average) + " best:" + _current_best_fitness + " bestVar:" + _current_best_fitness_variance;


            Save();
        }

        CreateNewGeneration();

        completed = true;
    }

    private void UpdateFitnesses(Parameters key, double fitness)
    {
        Load();//update the fitness table from possible other executions
        ParameterFitness pf = new ParameterFitness();
        if (fitnesses.ContainsKey(key))
        {
            pf = fitnesses[key];
        }
        pf.AddValue(fitness);

        fitnesses[key] = pf;
    }

    private void CreateNewGeneration()
    {
        var myList = population.ToList();

        myList.Sort((pair1, pair2) => -fitnesses[pair1].average.CompareTo(fitnesses[pair2].average));

        Parameters[] nextGeneration = new Parameters[population_size];
        nextGeneration[0] = myList[0];

        int pos = 1;

        while (pos < population_size)
        {
            Parameters p1 = SelectCandidateByRank(myList);
            Parameters p2 = SelectCandidateByRank(myList);

            nextGeneration[pos] = MutationManager.Mutate(MutationManager.Crossover(p1, p2));

            pos++;
        }

        population = nextGeneration;
    }


    private static Parameters SelectCandidateByRank(List<Parameters> myList)
    {
        int rankSum = population_size * (population_size + 1) / 2;
        int randVal = r.Next(rankSum);
        int currentRankPoints = population_size;
        Parameters p1 = null;
        while (p1 == null)
        {
            if (randVal < currentRankPoints)
                p1 = myList[population_size - currentRankPoints];
            randVal -= currentRankPoints;

            currentRankPoints--;
        }

        return p1;
    }

   

    private double GetFitness()
    {
        DisplayStatistics stats = GameObject.FindObjectOfType<DisplayStatistics>();

        string heaviestSpecie = GetHeaviestSpecie(stats);

        if (heaviestSpecie == "")
            return 0;

        float size = stats.speciesSize[heaviestSpecie][0] / stats.speciesSize[heaviestSpecie][1];
        float lifespan = (float)stats.speciesLifespan[heaviestSpecie][0] / (float)stats.speciesLifespan[heaviestSpecie][1];

        double fitness = Math.Pow(2, Math.Log(size) + 0.25 * Math.Log(lifespan));

        return fitness;
    }

    private string GetHeaviestSpecie(DisplayStatistics stats)
    {
        Dictionary<string, float[]> sizes = stats.speciesSize;
        string heaviest = "";

        float maxSize = 0;

        foreach (var specie in sizes)
        {
            float size = specie.Value[0] / specie.Value[1];

            if (size > maxSize  && stats.speciesAge[specie.Key] >= 10) //&& specie.Value[1] >= 2
            {
                maxSize = size;
                heaviest = specie.Key;
            }
        }
        
        return heaviest;
    }


    public static string previous = null;

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if (previous != null)
            File.Delete(previous);

        string path = Application.persistentDataPath + "/opt/opt_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".sav";
        FileStream stream = new FileStream(path, FileMode.Create);
        previous = path;

        formatter.Serialize(stream, fitnesses);

        stream.Close();

    }

    public static void Load()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/opt");

        if (files.Length == 0)
            return;

        string mostRecent = "";
        DateTime mostRecentDate = new DateTime();
        foreach (string file in files)
        {
            string tmp = file.Split('_')[1];
            tmp = tmp.Remove(tmp.Length - 4).Replace('-', '/').Replace('.', ':');
            try
            {
                DateTime d = Convert.ToDateTime(tmp);
                if (DateTime.Compare(d, mostRecentDate) > 0)
                {
                    mostRecent = file;
                    mostRecentDate = d;
                }
            }
            catch (Exception) { }
        }

        if (File.Exists(mostRecent))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(mostRecent, FileMode.Open);


            Dictionary<Parameters, ParameterFitness> deserialized = (Dictionary<Parameters, ParameterFitness>)formatter.Deserialize(stream);

            foreach (Parameters par in deserialized.Keys)
            {
                fitnesses[par] = deserialized[par];
            }

            stream.Close();

        }
    }
}
