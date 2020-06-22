using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class MutationManager
{
    public static System.Random r = new System.Random();
    public static double mutationRate = 0.28;
    public static Parameters Crossover(Parameters p1, Parameters p2)
    {
        Parameters newParameters = new Parameters(p1);
        //mutationprobabilities
        if (r.NextDouble() < 0.5) newParameters.MUTATION_PROBABILITY = p2.MUTATION_PROBABILITY;
        if (r.NextDouble() < 0.5) newParameters.NEURAL_MUTATION_PROBABILITY = p2.NEURAL_MUTATION_PROBABILITY;
        if (r.NextDouble() < 0.5) newParameters.ELIMINATION_PROBABILITY = p2.ELIMINATION_PROBABILITY;
        if (r.NextDouble() < 0.5) newParameters.INSERTION_PROBABILITY = p2.INSERTION_PROBABILITY;
        if (r.NextDouble() < 0.5) newParameters.TRANSPOSITION_PROBABILITY = p2.TRANSPOSITION_PROBABILITY;
        if (r.NextDouble() < 0.5) newParameters.MODIFICATION_PROBABILITY = p2.MODIFICATION_PROBABILITY;
        if (r.NextDouble() < 0.5) newParameters.SUBSTITUTION_PROBABILITY = p2.SUBSTITUTION_PROBABILITY;

        if (r.NextDouble() < 0.5) newParameters.INSERTION_COPY_PROBABILITY = p2.INSERTION_COPY_PROBABILITY;
        if (r.NextDouble() < 0.5) newParameters.INSERTION_NEWNUMBER_PROBABILITY = p2.INSERTION_NEWNUMBER_PROBABILITY;

        //genetics
        if (r.NextDouble() < 0.5) newParameters.SPECIES_DIFFERENCE_THRESHOLD = p2.SPECIES_DIFFERENCE_THRESHOLD;
        if (r.NextDouble() < 0.5) newParameters.ALLOW_MITOSIS = p2.ALLOW_MITOSIS;

        //map
        if (r.NextDouble() < 0.5) newParameters.MAP_SIZE = p2.MAP_SIZE;
        if (r.NextDouble() < 0.5) newParameters.FOOD_RATE = p2.FOOD_RATE;
        if (r.NextDouble() < 0.5) newParameters.MAX_FOOD = p2.MAX_FOOD;
        if (r.NextDouble() < 0.5) newParameters.BARRIERS_NUMBER = p2.BARRIERS_NUMBER;
        if (r.NextDouble() < 0.5) newParameters.FLAGELLO_FORCE = p2.FLAGELLO_FORCE;
        if (r.NextDouble() < 0.5) newParameters.FOOD_ENERGY_VALUE = p2.FOOD_ENERGY_VALUE;


        return newParameters;

    }

    public static Parameters Mutate(Parameters parameters)
    {
        Parameters mutatedParameters = new Parameters(parameters);
        //mutationprobabilities
     //   if (r.NextDouble() < mutationRate) mutatedParameters.MUTATION_PROBABILITY = MutateParameter(parameters.MUTATION_PROBABILITY, 0.01, 1, 0.3);
     //   if (r.NextDouble() < mutationRate) mutatedParameters.NEURAL_MUTATION_PROBABILITY = MutateParameter(parameters.NEURAL_MUTATION_PROBABILITY, 0.01, 1, 1);
        if (r.NextDouble() < mutationRate) mutatedParameters.ELIMINATION_PROBABILITY = MutateParameter(parameters.ELIMINATION_PROBABILITY, 0.01, 10, 3);
     //   if (r.NextDouble() < mutationRate) mutatedParameters.INSERTION_PROBABILITY = MutateParameter(parameters.INSERTION_PROBABILITY, 0.01, 10, 1);
        if (r.NextDouble() < mutationRate) mutatedParameters.TRANSPOSITION_PROBABILITY = MutateParameter(parameters.TRANSPOSITION_PROBABILITY, 0.01, 10, 3);
        if (r.NextDouble() < mutationRate) mutatedParameters.MODIFICATION_PROBABILITY = MutateParameter(parameters.MODIFICATION_PROBABILITY, 0.01, 10, 3);

        if (r.NextDouble() < mutationRate) mutatedParameters.SUBSTITUTION_PROBABILITY = MutateParameter(parameters.SUBSTITUTION_PROBABILITY, 0.001, 4, 3);

        if (r.NextDouble() < mutationRate) mutatedParameters.INSERTION_COPY_PROBABILITY = MutateParameter(parameters.INSERTION_COPY_PROBABILITY, 0.01, 1, 3);
        if (r.NextDouble() < mutationRate) mutatedParameters.INSERTION_NEWNUMBER_PROBABILITY = MutateParameter(parameters.INSERTION_NEWNUMBER_PROBABILITY, 0.01, 1, 3);

        //genetics
      //  if (r.NextDouble() < mutationRate) mutatedParameters.SPECIES_DIFFERENCE_THRESHOLD = MutateParameter(parameters.SPECIES_DIFFERENCE_THRESHOLD, 0.1, 8, 1);
        //  if (r.NextDouble() < mutationRate) mutatedParameters.ALLOW_MITOSIS = MutateBoolParameter();

        //map
    //    if (r.NextDouble() < mutationRate) mutatedParameters.MAP_SIZE = MutateParameter(parameters.MAP_SIZE, 50, 1300, 0.7);
     //   if (r.NextDouble() < mutationRate) mutatedParameters.FOOD_RATE = MutateParameter(parameters.FOOD_RATE, 1, 100, 0.7);
    //    if (r.NextDouble() < mutationRate) mutatedParameters.MAX_FOOD = MutateParameter(parameters.MAX_FOOD, 400, 30000, 1);

        //   if (r.NextDouble() < mutationRate) mutatedParameters.BARRIERS_NUMBER = MutateParameter(parameters.BARRIERS_NUMBER, 0, 2, 2);
    //    if (r.NextDouble() < mutationRate) mutatedParameters.FOOD_ENERGY_VALUE = MutateParameter(parameters.FOOD_ENERGY_VALUE, 400, 100000, 1);
   //     if (r.NextDouble() < mutationRate) mutatedParameters.FLAGELLO_FORCE = (float)MutateParameter(parameters.FLAGELLO_FORCE, 3, 200, 1);


        return mutatedParameters;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="mutationMagnitude"> variation between parameter/(1+magnitude) and parameter*(1+magnitude)</param>
    /// <returns></returns>
    private static double MutateParameter(double parameter, double min, double max, double mutationMagnitude)
    {

        if (parameter == 0)
        {
            return min + r.NextDouble() * (max - min);
        }

        double randomComponent = r.NextDouble() * 2 - 1;
        double mutated = parameter * Math.Pow(1 + mutationMagnitude, randomComponent);


        if (mutated > max) return max;
        if (mutated < min) return min;

        return RoundFixedRelativeError(mutated, mutationMagnitude / 5);


    }
    private static int MutateParameter(int parameter, int min, int max, double mutationMagnitude)
    {
        if ((int)(parameter * (1 + mutationMagnitude)) == parameter)
        {
            return (int)RoundFixedRelativeError((double)r.Next(min, max + 1), mutationMagnitude / 5);
        }
        return (int)MutateParameter((double)parameter, (double)min, (double)max, mutationMagnitude);

    }

    private static double RoundFixedRelativeError(double value, double relativeError)
    {

        if (value == 0)
            return value;

        double logBase = 1 + relativeError;
        int exponent = (int)Math.Round(Math.Log(value, logBase));

        return Math.Pow(logBase, exponent);
    }

    private static double RoundToSignificantDigits(double d)
    {
        int digits = 1;

        if (d == 0)
            return 0;

        double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);

        return scale * Math.Round(d / scale, digits);
    }

    private static bool MutateBoolParameter()
    {
        return r.NextDouble() > 0.5;
    }

}
