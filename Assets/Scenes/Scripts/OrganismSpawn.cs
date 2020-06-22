using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrganismSpawn : MonoBehaviour
{
    public int STOP_SPAWNING_AT;
    public double _mutationRate;//for setting outside
    public double _neuralMutationRate;//for setting outside
    public int _organismNumber;//for view

    public float bodyEnergyMultiplier = 1;
    public static OrganismSpawn organismSpawner;

    public  GameObject[] cellTypes;
    System.Random r = new System.Random();

    public  GameObject organism;
    
    void Start()
    {
        OrganismSpawn.organismSpawner = this;

        ModifyBodyEnergy();

        InvokeRepeating("Spawn", 2f, 2f);
    }

    private void ModifyBodyEnergy()
    {
        for(int i = 0; i<cellTypes.Length;i++)
        {

            Cell c = cellTypes[i].GetComponent<Cell>();
            c.bodyEnergy = (int)(c.bodyEnergy * bodyEnergyMultiplier);
            c.energyStorage = (int)(c.energyStorage * bodyEnergyMultiplier);
        }
    }


    void Spawn()
    {
        
        GenesManager.mutationRate = _mutationRate;
        GenesManager.neuralMutationRate = _neuralMutationRate;
        GameObject[] organisms = GameObject.FindGameObjectsWithTag("organism");

        _organismNumber = organisms.Length;
        OrganismSpawn.organismNumber= organisms.Length;
        if (organisms.Length < STOP_SPAWNING_AT)
        {
            float x = (float)((r.NextDouble()*0.8+0.1) * (Convert.ToDouble(Hyperparameters.MAP_SIZE)));
            float y = (float)((r.NextDouble() * 0.8 + 0.1) * (Convert.ToDouble(Hyperparameters.MAP_SIZE))); //PROBLEMA DI SPAWN SUI BORDI??
            Chromosome chromosome;
        //    if (organisms.Length == 0)
       //     {
                chromosome = MakeChromosome();
     //       }
     //       else
     //       {

        //       chromosome = organisms[r.Next(organisms.Length)].GetComponent<Organism>().chromosome;
                
    //        }
            OrganismSpawn.SpawnOrganism(chromosome, new Vector3(x, y),OrganismSetter.BodyEnergy(chromosome) * 3 / 2);

        }

    }

    public static int organismNumber=0;
    public static Organism SpawnOrganism(Chromosome chromosome, Vector3 position, int initialEnergy)
    {
        if (organismNumber >= 1500) return null;
        if (position.x < 0 || position.x > Hyperparameters.MAP_SIZE || position.y < 0 || position.y > Hyperparameters.MAP_SIZE) return null;//out of bounds
        GameObject obj = Instantiate(organismSpawner.organism, position, new Quaternion());
        organismNumber++;
        Organism spawned = obj.GetComponent<Organism>();
        spawned.chromosome = chromosome;
        spawned.organismEnergy = new OrganismEnergy(0,initialEnergy);

        return spawned;
    }
    Chromosome MakeChromosome()
    {
        return new Chromosome(MakeGenes(), new NeuralChromosome(),new ChromosomeParameters.ChromosomeParametersBuilder().Build());

    }
    Gene[] MakeGenes()
    {//0-dig 1-flg 2-mac 3-che 4-fat 5-poi 6-spi 7-eye 8-basic
        Gene[] genes = new Gene[2];
        genes[0] = new Gene(0, new Vector3(1, 0), cellTypes[8], 1);
        genes[1] = new Gene(1, new Vector3(1, 0), cellTypes[1], 2);
     //   genes[2] = new Gene(2, new Vector3(1, 0), cellTypes[8], 3);
        return genes;
    }


}
