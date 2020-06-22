using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Parameters : ISerializable
{
    //mutation probabilities
    public double MUTATION_PROBABILITY = 0.5;
    public double NEURAL_MUTATION_PROBABILITY = 0.5;
    public double ELIMINATION_PROBABILITY = 0.8;
    public double INSERTION_PROBABILITY = 1;
    public double TRANSPOSITION_PROBABILITY = 1;
    public double MODIFICATION_PROBABILITY = 0.5;
    public double SUBSTITUTION_PROBABILITY = 0.01;

    public double INSERTION_COPY_PROBABILITY = 0.2;
    public double INSERTION_NEWNUMBER_PROBABILITY = 0.15;

    //genetics
    public double SPECIES_DIFFERENCE_THRESHOLD = 0.5;
    public bool ALLOW_MITOSIS = true;

    //map
    public int MAP_SIZE = 175;
    public double FOOD_RATE = 3.5;
    public int MAX_FOOD = 4500;
    public int BARRIERS_NUMBER = 2;
    public int FOOD_ENERGY_VALUE = 7000;

    //cells
    public float FLAGELLO_FORCE = 5;

    public Parameters()
    {
    }

    public Parameters(Parameters parameters)
    {
        //mutationprobabilities
        this.MUTATION_PROBABILITY = parameters.MUTATION_PROBABILITY;
        this.NEURAL_MUTATION_PROBABILITY = parameters.NEURAL_MUTATION_PROBABILITY;
        this.ELIMINATION_PROBABILITY = parameters.ELIMINATION_PROBABILITY;
        this.INSERTION_PROBABILITY = parameters.INSERTION_PROBABILITY;
        this.TRANSPOSITION_PROBABILITY = parameters.TRANSPOSITION_PROBABILITY;
        this.MODIFICATION_PROBABILITY = parameters.MODIFICATION_PROBABILITY;

        this.INSERTION_COPY_PROBABILITY = parameters.INSERTION_COPY_PROBABILITY;
        this.INSERTION_NEWNUMBER_PROBABILITY = parameters.INSERTION_NEWNUMBER_PROBABILITY;

        //genetics
        this.SPECIES_DIFFERENCE_THRESHOLD = parameters.SPECIES_DIFFERENCE_THRESHOLD;
        this.ALLOW_MITOSIS = parameters.ALLOW_MITOSIS;
        this.SUBSTITUTION_PROBABILITY = parameters.SUBSTITUTION_PROBABILITY;

        //map
        this.MAP_SIZE = parameters.MAP_SIZE;
        this.FOOD_RATE = parameters.FOOD_RATE;
        this.MAX_FOOD = parameters.MAX_FOOD;
        this.BARRIERS_NUMBER = parameters.BARRIERS_NUMBER;
        this.FOOD_ENERGY_VALUE = parameters.FOOD_ENERGY_VALUE;

        this.FLAGELLO_FORCE = parameters.FLAGELLO_FORCE;
    }

    public override bool Equals(object obj)
    {
        var parameters = obj as Parameters;
        return parameters != null &&
               MUTATION_PROBABILITY == parameters.MUTATION_PROBABILITY &&
               NEURAL_MUTATION_PROBABILITY == parameters.NEURAL_MUTATION_PROBABILITY &&
               ELIMINATION_PROBABILITY == parameters.ELIMINATION_PROBABILITY &&
               INSERTION_PROBABILITY == parameters.INSERTION_PROBABILITY &&
               TRANSPOSITION_PROBABILITY == parameters.TRANSPOSITION_PROBABILITY &&
               MODIFICATION_PROBABILITY == parameters.MODIFICATION_PROBABILITY &&
               INSERTION_COPY_PROBABILITY == parameters.INSERTION_COPY_PROBABILITY &&
               INSERTION_NEWNUMBER_PROBABILITY == parameters.INSERTION_NEWNUMBER_PROBABILITY &&
               SPECIES_DIFFERENCE_THRESHOLD == parameters.SPECIES_DIFFERENCE_THRESHOLD &&
               ALLOW_MITOSIS == parameters.ALLOW_MITOSIS &&
               SUBSTITUTION_PROBABILITY == parameters.SUBSTITUTION_PROBABILITY &&
               MAP_SIZE == parameters.MAP_SIZE &&
                              MAX_FOOD == parameters.MAX_FOOD &&
               FOOD_RATE == parameters.FOOD_RATE &&
               BARRIERS_NUMBER == parameters.BARRIERS_NUMBER &&
                              FLAGELLO_FORCE == parameters.FLAGELLO_FORCE &&
               FOOD_ENERGY_VALUE == parameters.FOOD_ENERGY_VALUE;
    }

    public override int GetHashCode()
    {
        var hashCode = 1896524372;
        hashCode = hashCode * -1521134295 + MUTATION_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + NEURAL_MUTATION_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + ELIMINATION_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + INSERTION_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + TRANSPOSITION_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + MODIFICATION_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + INSERTION_COPY_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + INSERTION_NEWNUMBER_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + SPECIES_DIFFERENCE_THRESHOLD.GetHashCode();
        hashCode = hashCode * -1521134295 + ALLOW_MITOSIS.GetHashCode();
        hashCode = hashCode * -1521134295 + SUBSTITUTION_PROBABILITY.GetHashCode();
        hashCode = hashCode * -1521134295 + MAP_SIZE.GetHashCode();
        hashCode = hashCode * -1521134295 + FOOD_RATE.GetHashCode();
        hashCode = hashCode * -1521134295 + MAX_FOOD.GetHashCode();
        hashCode = hashCode * -1521134295 + BARRIERS_NUMBER.GetHashCode();
        hashCode = hashCode * -1521134295 + FLAGELLO_FORCE.GetHashCode();
        hashCode = hashCode * -1521134295 + FOOD_ENERGY_VALUE.GetHashCode();
        return hashCode;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("MUTATION_PROBABILITY", MUTATION_PROBABILITY, typeof(double));
        info.AddValue("NEURAL_MUTATION_PROBABILITY", NEURAL_MUTATION_PROBABILITY, typeof(double));
        info.AddValue("ELIMINATION_PROBABILITY", ELIMINATION_PROBABILITY, typeof(double));
        info.AddValue("INSERTION_PROBABILITY", INSERTION_PROBABILITY, typeof(double));
        info.AddValue("TRANSPOSITION_PROBABILITY", TRANSPOSITION_PROBABILITY, typeof(double));
        info.AddValue("MODIFICATION_PROBABILITY", MODIFICATION_PROBABILITY, typeof(double));
        info.AddValue("SUBSTITUTION_PROBABILITY", SUBSTITUTION_PROBABILITY, typeof(double));

        info.AddValue("INSERTION_COPY_PROBABILITY", INSERTION_COPY_PROBABILITY, typeof(double));
        info.AddValue("INSERTION_NEWNUMBER_PROBABILITY", INSERTION_NEWNUMBER_PROBABILITY, typeof(double));
        info.AddValue("SPECIES_DIFFERENCE_THRESHOLD", SPECIES_DIFFERENCE_THRESHOLD, typeof(double));
        info.AddValue("ALLOW_MITOSIS", ALLOW_MITOSIS, typeof(bool));


          info.AddValue("MAP_SIZE", MAP_SIZE, typeof(int));
        info.AddValue("FOOD_RATE", FOOD_RATE, typeof(double));
        info.AddValue("MAX_FOOD", MAX_FOOD, typeof(int));
        info.AddValue("BARRIERS_NUMBER", BARRIERS_NUMBER, typeof(int));

        info.AddValue("FLAGELLO_FORCE", FLAGELLO_FORCE, typeof(float));
        info.AddValue("FOOD_ENERGY_VALUE", FOOD_ENERGY_VALUE, typeof(int));
    }

    public Parameters(SerializationInfo info, StreamingContext context)
    {
        // Reset the property value using the GetValue method.
        MUTATION_PROBABILITY = (double)info.GetValue("MUTATION_PROBABILITY", typeof(double));
        NEURAL_MUTATION_PROBABILITY = (double)info.GetValue("NEURAL_MUTATION_PROBABILITY", typeof(double));
        ELIMINATION_PROBABILITY = (double)info.GetValue("ELIMINATION_PROBABILITY", typeof(double));
        INSERTION_PROBABILITY = (double)info.GetValue("INSERTION_PROBABILITY", typeof(double));
        TRANSPOSITION_PROBABILITY = (double)info.GetValue("TRANSPOSITION_PROBABILITY", typeof(double));
        MODIFICATION_PROBABILITY = (double)info.GetValue("MODIFICATION_PROBABILITY", typeof(double));
        INSERTION_COPY_PROBABILITY = (double)info.GetValue("INSERTION_COPY_PROBABILITY", typeof(double));
        INSERTION_NEWNUMBER_PROBABILITY = (double)info.GetValue("INSERTION_NEWNUMBER_PROBABILITY", typeof(double));
        SPECIES_DIFFERENCE_THRESHOLD = (double)info.GetValue("SPECIES_DIFFERENCE_THRESHOLD", typeof(double));
        ALLOW_MITOSIS = (bool)info.GetValue("ALLOW_MITOSIS", typeof(bool));
        SUBSTITUTION_PROBABILITY = (double)info.GetValue("SUBSTITUTION_PROBABILITY", typeof(double));

        MAP_SIZE = (int)info.GetValue("MAP_SIZE", typeof(int));
        FOOD_RATE = (double)info.GetValue("FOOD_RATE", typeof(double));
        MAX_FOOD = (int)info.GetValue("MAX_FOOD", typeof(int));
        FLAGELLO_FORCE = (float)info.GetValue("FLAGELLO_FORCE", typeof(float));
        BARRIERS_NUMBER = (int)info.GetValue("BARRIERS_NUMBER", typeof(int));
        FOOD_ENERGY_VALUE = (int)info.GetValue("FOOD_ENERGY_VALUE", typeof(int));
    }
}
