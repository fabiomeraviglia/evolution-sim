using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Hyperparameters
{
    //mutation probabilities
    public static double MUTATION_PROBABILITY=0.8;
    public static double NEURAL_MUTATION_PROBABILITY=0.1;
    public static double ELIMINATION_PROBABILITY=0.25;
    public static double INSERTION_PROBABILITY=0.45;
    public static double TRANSPOSITION_PROBABILITY=0.25;
    public static double MODIFICATION_PROBABILITY=0.25;
    public static double SPECIALIZATION_PROBABILITY = 1;
    
    public static double INSERTION_COPY_PROBABILITY=0.65;
    public static double INSERTION_NEWNUMBER_PROBABILITY=0.35;

    //genetics
    public static double SPECIES_DIFFERENCE_THRESHOLD = 4;
    public static bool ALLOW_MITOSIS = false;
    public static double SUBSTITUTION_PROBABILITY=0.01;

    //map
    public static int MAP_SIZE=500;
    public static int FOOD_RATE=7;
    public static int MAX_FOOD = 5000;
    public static int BARRIERS_NUMBER=1;
    public static int FOOD_ENERGY_VALUE = 2000;


    public static float FLAGELLO_FORCE = 10;
}