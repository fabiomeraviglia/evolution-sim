using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cell : MonoBehaviour
{

    public CellAttributes attributes = null;
    public int _energy;//only for debugging
    public int _health;
    public bool _alive;
    public int MAX_HEALTH; //maximum possible health
    public int bodyEnergy;//energy required to build this cell and released upon death
    public bool[] growInDirection; //up, right, down, left
    public readonly static Vector3[] vectors = new Vector3[] { new Vector3(0, 1), new Vector3(1, 0), new Vector3(0, -1), new Vector3(-1, 0) };
    public int energyStorage;
    public int energyConsumption;//how much energy does this use up

    public int regenerationFactor; //how fast health regenerate
                                   // Start is called before the first frame update
    void Start()
    {
        InvokeCellStuff();
    }

    public void InvokeCellStuff()
    {

        InvokeRepeating("ConsumeEnergy", 1f, 1f);
        InvokeRepeating("CheckHealth", 0.3f, 0.3f);

        attributes.health = MAX_HEALTH;

    }
    /// <summary>
    /// Sets attributes and neurons
    /// </summary>
    /// <param name="attributes"></param>
    public void SetAttributes(CellAttributes attributes)
    {
        this.attributes = attributes;
        attributes.instance = this;
        if (attributes.outputNeurons != null)
        {
            SetOutputNeurons(attributes.outputNeurons);
            SetInputNeurons(attributes.inputNeurons);
        }
    }

    public abstract void SetInputNeurons(List<Neuron> inputNeurons);

    public abstract void SetOutputNeurons(List<Neuron> outputNeurons);

    public abstract bool CanLiveAlone();

    public void Update()
    {
        _health = attributes.health;
        _energy = attributes.energy.Value;
        _alive = attributes.alive;
    }

    private void ConsumeEnergy()
    {
        if (!IsAlive()) return;
        if (attributes.energy.Value >= energyConsumption)
        {
            attributes.energy.Value -= energyConsumption;
        }
        else
        {
            TakeDamage(MAX_HEALTH/30);
        }

    }
    public bool IsAlive()
    {
        return attributes.alive;
    }
    private void CheckHealth()
    {
        if (!IsAlive()) return;
        if (attributes.health <= 0) Die(true);
        else
        {
            if (attributes.health < MAX_HEALTH && attributes.energy.Value> regenerationFactor)
            {
                attributes.health += regenerationFactor;
                attributes.energy.Value -= regenerationFactor;
                if (attributes.health > MAX_HEALTH) attributes.health = MAX_HEALTH;
            }
        }
    }

    public void Die(bool notifyParent)
    {
        Die(notifyParent, true);
    }
    public void Die(bool notifyParent, bool releaseEnergy)
    {
        attributes.alive = false;

        Destroy(transform.gameObject);

        if (releaseEnergy)
        {
            ReleaseEnergy(true);
        }

        transform.GetComponentInParent<Organism>().cells.deadCells++;
        if (notifyParent)
        {
            transform.GetComponentInParent<Organism>().CellDied();//notify organism 
        }
    }

    protected void EatFood(GameObject foodGameObject, int eatableAmount)
    {
        Food food = foodGameObject.GetComponent<Food>();

        if (eatableAmount >= food.energyValue)
        {
            attributes.energy.Value += food.energyValue;
            Destroy(foodGameObject);
        }
        else
        {
            attributes.energy.Value += eatableAmount;
            food.energyValue -= eatableAmount;
        }
    }

    public int ReleaseEnergy(bool releaseOnMap)
    {
        float energyInThisCell = (float)energyStorage / attributes.energy.GetTotalEnergyStorage();
        int energyFromOrganism = (int)(attributes.energy.Value * energyInThisCell);
        attributes.energy.Value -= energyFromOrganism;//steals energy from organism. it represents the energy that was stored in this cell

        int energyFromEgg = (int)(attributes.energy.EggEnergy * energyInThisCell);
        attributes.energy.EggEnergy -= energyFromEgg;

        float lostEnergy = 0.1f;        //ratio of how much energy is lost in the process
        int releasedEnergy = (int)((1 - lostEnergy) * (bodyEnergy + energyFromOrganism + energyFromEgg));

        if (releaseOnMap)
        {
            GameObject obj = Instantiate(FoodSpawn.PROTEIN, transform.position, new Quaternion());
            obj.GetComponent<Food>().energyValue = releasedEnergy;
            return 0;
        }
        return releasedEnergy;
    }

    virtual public void TakeDamage(int damage)
    {
        attributes.health -= damage;
        if (attributes.health < 0) Die(true);
    }

    virtual public void TakeBluntDamage(int damage)
    {
        TakeDamage(damage);
    }
    //neurons that should be in the input layer in the NN 
    abstract public List<int> GetInputNeuronsNumbers(int startingNumber);
    //neurons of the output layer in NN
    abstract public List<int> GetOutputNeuronsNumbers(int startingNumber);
}
