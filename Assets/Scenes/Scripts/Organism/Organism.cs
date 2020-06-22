using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


public class Organism : MonoBehaviour
{
    public string _tracker;
    public string _viewChromosome;
    public float CELL_WIDTH;
    public int bodyEnergy;

    public Chromosome chromosome;
    public OrganismEnergy organismEnergy;
    public CellsStructure cells = new CellsStructure();
    public NeuralNetwork neuralNetwork;
    public Neuron damageNeuron, energyNeuron, mitosisNeuron, sexualReproductionNeuron, bias;
    public bool fullyGrow = false;
    public LinkedList<KeyValuePair<CellAttributes, CellAttributes>> orderOfGrowth;
    private int startingAge;
    public PhysicalProperties pp = new PhysicalProperties();

    void Start()
    {
        startingAge = Map.minutes;
        _tracker = chromosome.MutationTracker;
        _viewChromosome = chromosome.ToString();
        cells = new CellsStructure();

        bodyEnergy = OrganismSetter.BodyEnergy(chromosome);

        orderOfGrowth = CellsCreator.GetOrderOfGrowth(this);

        OrganismSetter.SetNeuralNetwork(this);
        OrganismSetter.SetDrag(this);
        OrganismSetter.SetName(this);
        
        OrganismCellsGrowth.GrowCell(this,false);

        if (fullyGrow)
            OrganismCellsGrowth.FullyGrow(this);

        InvokeRepeating("Routine", 2f, 5f);

        InvokeRepeating("Think", 2f, SimulationParameters.brainTick);

        InvokeRepeating("Physics", 2f, SimulationParameters.physics);
    }

    public void Routine()
    {
        if (Alive())
        {
            Grow();

            Heal();
            Mitosis();
        }
        else Die();
    }

    private void Think()
    {
        energyNeuron.Value = (float)organismEnergy.Value / bodyEnergy;
        damageNeuron.Value = (float)cells.deadCells / cells.Count();

        neuralNetwork.FeedFoward();
    }

    private void Grow()
    {
        int number = (int)Math.Sqrt(cells.Count());
        for (int i = 0; i < number; i++)
        {//grow a number of cells based on the number that are already alive
            OrganismCellsGrowth.GrowCell(this, false);
        }

        OrganismHealthCheck.KillDeatachedCells(cells);
    }
    
    private void Heal()
    {
        int healProb = 10;//inverse heal probability for each dead cell next to living cell
        foreach (CellAttributes cell in cells)
        {
            if ((!cell.alive) &&
                GenesManager.r.Next(healProb * cell.type.GetComponent<Cell>().bodyEnergy) < organismEnergy.Value &&
                CellHasLivingNeighbour(cell) &&
                organismEnergy.Value > cell.type.GetComponent<Cell>().bodyEnergy &&
                CellsCreator.CanInstantiate(cell, transform))
            {
                organismEnergy.Value -= cell.type.GetComponent<Cell>().bodyEnergy;

                organismEnergy.SetTotalEnergyStorage(organismEnergy.GetTotalEnergyStorage() + cell.type.GetComponent<Cell>().energyStorage);
                cell.alive = true;

                CellsCreator.InstantiateCell(cell, transform);

                cells.deadCells--;
                return;
            }
        }
    }

    private bool CellHasLivingNeighbour(CellAttributes cell)
    {
        Vector3[] vectors = Cell.vectors;
        for(int i=0;i<vectors.Length;i++)
        {
            if (CellsCreator.AllowGrowthInDirection(cell, vectors[i]))
            {
                CellAttributes c = cells.GetCell(cell.relativePosition + vectors[i]);
                if (c != null && c.alive && CellsCreator.AllowGrowthInDirection(c, -vectors[i])) return true;
            }
        }
        return false;
    }

    public bool Alive()
    {
        return OrganismHealthCheck.AreValidCellsAlive(cells);

    }
    
    public static List<int> GetOutputOrganismNeuronsNumbers()
    {
        List<int> numbers = new List<int>();
        numbers.Add(-600);
        numbers.Add(-601);

        return numbers;
    }

    public static List<int> GetInputOrganismNeuronsNumbers()
    {
        List<int> numbers = new List<int>();
        numbers.Add(-500);
        numbers.Add(-501);
        numbers.Add(-502);

        return numbers;
    }

    public static List<Neuron> CreateNeuronsFromNumbers(List<int> list)
    {
        List<Neuron> neurons = new List<Neuron>();
        foreach (int i in list)
        {
            neurons.Add(new Neuron(i));
        }
        return neurons;
    }

    public void SetTotalEnergyStorage()
    {
        organismEnergy.SetTotalEnergyStorage(0);
        foreach (CellAttributes cell in cells)
        {
            if (cell.alive)
            {
                organismEnergy.SetTotalEnergyStorage(organismEnergy.GetTotalEnergyStorage() + cell.type.GetComponent<Cell>().energyStorage);
            }
        }
    }
    
    public void Mitosis()
    {
        if (!Hyperparameters.ALLOW_MITOSIS)
            return;

        int energyToReproduce = (int)(bodyEnergy * chromosome.Parameters.EnergyToSonRatio * 4);


        if (organismEnergy.Value > energyToReproduce * (1 + chromosome.Parameters.ExcessEnergyToReproduce) && mitosisNeuron.Value >= 0.5)
        {
            Chromosome newChromosome = GenesManager.CreateNewGenesForReproduction(this.chromosome);

            OrganismSpawn.SpawnOrganism(newChromosome, ComputeChildPosition(), energyToReproduce);

            organismEnergy.Value -= energyToReproduce;
        }
    }

    private Vector3 ComputeChildPosition()
    {
        if (transform.childCount == 0) return transform.position;

        Bounds bounds = transform.GetChild(0).GetComponent<Collider2D>().bounds;
        for (int i = 1; i < transform.childCount; i++)
        {
            Collider2D collider = transform.GetChild(i).GetComponent<Collider2D>();

            bounds.Encapsulate(collider.bounds);

        }
        if (transform.position.x + transform.position.y < Hyperparameters.MAP_SIZE)
        {
            return bounds.center + bounds.extents * 2;
        }
        return bounds.center - bounds.extents * 2;
    }

   

    public void CellDied()
    {
        //All cells that get cut out from the center of the organism, die as well 

        OrganismHealthCheck.KillDeatachedCells(cells);

        SetTotalEnergyStorage();
        if (!OrganismHealthCheck.AreValidCellsAlive(cells))
            Die();

    }




    private void Die()
    {
        foreach (CellAttributes cell in cells)
        {
            if (cell.alive)
            {
                cell.instance.Die(false);
            }
        }
        Destroy(transform.gameObject);
    }


    public void Physics()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        Vector2 opposite = -body.velocity;
        float resistanceAmount = 2f;
        Vector2 friction = opposite * resistanceAmount * (float)Math.Pow(body.mass, 0.5);

        Vector2 current = Map.Current * SimulationParameters.currentAmount;

        body.AddForce(friction + current);

        pp.Update(GetComponent<Rigidbody2D>());

    }

    private void OnMouseDown()
    {
        DisplayNeuralNetwork.displayedNN = this.neuralNetwork;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "organism")
        {
            Organism other = collision.gameObject.GetComponent<Organism>();
            if (GenesManager.SameSpecie(this.chromosome, other.chromosome))
            {
                Altruism(other);
                SexualReproduction(other);
            }

        }

    }

    private void Altruism(Organism other)
    {//altruism towards same species organisms. Give energy to them

        if (other.organismEnergy.Value >= this.organismEnergy.Value)
            return;


        int energyToGive = (int)((organismEnergy.Value - other.organismEnergy.Value) * chromosome.Parameters.AltruismEnergy);

        organismEnergy.Value -= energyToGive;
        other.organismEnergy.Value += energyToGive;

    }

    private void SexualReproduction(Organism mother)
    {
        int energyToReproduce = (int)(bodyEnergy * chromosome.Parameters.EnergyToSonRatio * 2);
        int momEnergyToReproduce = (int)(mother.bodyEnergy * mother.chromosome.Parameters.EnergyToSonRatio * 2);

        if (organismEnergy.Value > (1 + chromosome.Parameters.ExcessEnergyToReproduce) * energyToReproduce &&
            mother.organismEnergy.Value > (1 + mother.chromosome.Parameters.ExcessEnergyToReproduce) * momEnergyToReproduce &&
            sexualReproductionNeuron.Value >= 0.5 &&
            mother.sexualReproductionNeuron.Value >= 0.5)
        {
            Chromosome newChromosome = GenesManager.CreateNewGenesForReproduction(this.chromosome, mother.chromosome);

            int totalEnergy = energyToReproduce + momEnergyToReproduce;
            OrganismSpawn.SpawnOrganism(newChromosome, ComputeChildPosition(), totalEnergy);

            organismEnergy.Value -= energyToReproduce;
            mother.organismEnergy.Value -= momEnergyToReproduce;
        }
    }

    public int Age { get => Map.minutes - startingAge; }
}
