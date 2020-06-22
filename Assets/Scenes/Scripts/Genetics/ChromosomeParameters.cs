using System;

[Serializable]
public class ChromosomeParameters
{
    public double EnergyToSonRatio { get; }
    public double AltruismEnergy { get; }
    public double ExcessEnergyToReproduce { get; }
    public double MinimumEnergyToGrow { get; }

    private ChromosomeParameters(double energyToSonRatio, double altruismEnergy, double excessEnergyToReproduce, double minimumEnergyToGrow )
    {
        EnergyToSonRatio = energyToSonRatio;

        AltruismEnergy = altruismEnergy;
        ExcessEnergyToReproduce = excessEnergyToReproduce;
        MinimumEnergyToGrow = minimumEnergyToGrow;
    }

    public ChromosomeParameters(double[] res)
    {
        EnergyToSonRatio = res[0];
        AltruismEnergy = res[1];
        ExcessEnergyToReproduce = res[2];
        MinimumEnergyToGrow = res[3];
    }
    
    public double[] ToArray()
    {
        return new double[] { EnergyToSonRatio , AltruismEnergy, ExcessEnergyToReproduce , MinimumEnergyToGrow };
    }

  


    public class ChromosomeParametersBuilder
    {
        double energyToSonRatio = 0.3;//0.3 x4 multiplier
        double altruismEnergy = 0.5;//0.1
        public double excessEnergyToReproduce=0.2;
        public double minimumEnergyToGrow=0.1;
        public ChromosomeParametersBuilder SetExcessEnergyToReproduce(double excessEnergyToReproduce)
        {
            this.excessEnergyToReproduce = excessEnergyToReproduce;
            return this;
        }
        public ChromosomeParametersBuilder SetMinimumEnergyToGrow(double minimumEnergyToGrow)
        {
            this.minimumEnergyToGrow = minimumEnergyToGrow;
            return this;
        }
        public ChromosomeParametersBuilder SetEnergyToSonRatio(double energyToSonRatio)
        {
            this.energyToSonRatio = energyToSonRatio;
            return this;
        }
        public ChromosomeParametersBuilder SetAltruismEnergy(double altruismEnergy)
        {
            this.altruismEnergy = altruismEnergy;
            return this;
        }
        public ChromosomeParameters Build()
        {
            return new ChromosomeParameters(energyToSonRatio, altruismEnergy, excessEnergyToReproduce, minimumEnergyToGrow);

        }
        
    }
}
