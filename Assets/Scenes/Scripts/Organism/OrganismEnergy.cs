public class OrganismEnergy
{
    private int value;
    private int eggEnergy;
    public OrganismEnergy(int initialEnergy, int eggEnergy)
    {
        value = initialEnergy;
        this.EggEnergy = eggEnergy;
        SetTotalEnergyStorage(initialEnergy);
    }

    public int Value
    {
        get
        {
            return value;
        }

        set
        {
            if (value < 0)
            {
                this.value = 0;
            }
            else
            {
                if (value > GetTotalEnergyStorage())
                {
                    this.value = GetTotalEnergyStorage();
                }
                else
                {
                    this.value = value;
                }
            }
        }
    }

    public int TotalEnergyStorage
    {
        get => totalEnergyStorage;
        set
        {
            totalEnergyStorage = value;
            if (this.value < totalEnergyStorage)
                this.value = totalEnergyStorage;
        }

    }

    public int EggEnergy { get => eggEnergy; set => eggEnergy = value; }

    private int totalEnergyStorage;

    public int GetTotalEnergyStorage()
    {
        return totalEnergyStorage;
    }

    public void SetTotalEnergyStorage(int value)
    {
        totalEnergyStorage = value;
    }



}