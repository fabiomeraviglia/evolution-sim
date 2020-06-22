using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class SimulationInitializer
{
    static public void SetSimulation(Parameters parameters, float RUNNING_SPEED)
    {

        foreach (var org in GameObject.FindObjectsOfType<Organism>())
        {
            GameObject.DestroyImmediate(org.gameObject);
        }
        foreach (var food in GameObject.FindObjectsOfType<Food>())
        {
            GameObject.DestroyImmediate(food.gameObject);
        }
        foreach (var barrier in GameObject.FindObjectsOfType<Barrier>())
        {
            GameObject.DestroyImmediate(barrier.gameObject);
        }
        GameObject.FindObjectOfType<DisplayStatistics>().reset();


        var spawner = GameObject.FindObjectOfType<OrganismSpawn>();
        spawner._mutationRate = parameters.MUTATION_PROBABILITY;
        spawner._neuralMutationRate = parameters.NEURAL_MUTATION_PROBABILITY;
        Hyperparameters.ELIMINATION_PROBABILITY = parameters.ELIMINATION_PROBABILITY;
        Hyperparameters.INSERTION_PROBABILITY = parameters.INSERTION_PROBABILITY;
        Hyperparameters.MODIFICATION_PROBABILITY = parameters.MODIFICATION_PROBABILITY;
        Hyperparameters.TRANSPOSITION_PROBABILITY = parameters.TRANSPOSITION_PROBABILITY;
        Hyperparameters.INSERTION_COPY_PROBABILITY = parameters.INSERTION_COPY_PROBABILITY;
        Hyperparameters.INSERTION_NEWNUMBER_PROBABILITY = parameters.INSERTION_NEWNUMBER_PROBABILITY;

        Hyperparameters.SPECIES_DIFFERENCE_THRESHOLD = parameters.SPECIES_DIFFERENCE_THRESHOLD;
        Hyperparameters.ALLOW_MITOSIS = parameters.ALLOW_MITOSIS;
        Hyperparameters.SUBSTITUTION_PROBABILITY = parameters.SUBSTITUTION_PROBABILITY;
        Hyperparameters.FLAGELLO_FORCE = parameters.FLAGELLO_FORCE;

        OrganismSpawn.organismSpawner = spawner;

        var foodSpawn = GameObject.FindObjectOfType<FoodSpawn>();
        foodSpawn.foodRate = (float) parameters.FOOD_RATE;
        foodSpawn.MAX_FOOD = parameters.MAX_FOOD;
        foodSpawn.movableFood.GetComponent<Food>().energyValue = parameters.FOOD_ENERGY_VALUE;

        foodSpawn.startingPoint = new Vector2(0, 0);
        foodSpawn.size = new Vector2(parameters.MAP_SIZE / 2, parameters.MAP_SIZE / 2);

        FoodSpawn foodSpawn2 = GameObject.Instantiate(foodSpawn);

        foodSpawn2.startingPoint = new Vector2(parameters.MAP_SIZE / 2, 0);
        foodSpawn2.size = new Vector2(parameters.MAP_SIZE / 2, parameters.MAP_SIZE / 2);
        foodSpawn2.foodRate = (float)parameters.FOOD_RATE/22f;

        FoodSpawn foodSpawn3 = GameObject.Instantiate(foodSpawn);

        foodSpawn3.startingPoint = new Vector2(0, parameters.MAP_SIZE / 2);
        foodSpawn3.size = new Vector2(parameters.MAP_SIZE / 2, parameters.MAP_SIZE / 2);
        foodSpawn3.foodRate = (float)parameters.FOOD_RATE / 2.8f;

        FoodSpawn foodSpawn4 = GameObject.Instantiate(foodSpawn);


        foodSpawn4.startingPoint = new Vector2(parameters.MAP_SIZE / 2, parameters.MAP_SIZE / 2);
        foodSpawn4.size = new Vector2(parameters.MAP_SIZE / 2, parameters.MAP_SIZE / 2);
        foodSpawn4.foodRate = (float)parameters.FOOD_RATE / 8f;

        Hyperparameters.BARRIERS_NUMBER = parameters.BARRIERS_NUMBER;


        var map = GameObject.FindObjectOfType<Map>();

        Hyperparameters.MAP_SIZE = parameters.MAP_SIZE;
        map.height = parameters.MAP_SIZE;
        map.width = map.height;

        Map newmap = GameObject.Instantiate(map);
        newmap.timescale = RUNNING_SPEED;

        GameObject.DestroyImmediate(map.gameObject);

        newmap.name = "Map";

        foodSpawn.UpdateFoodCount();

    }
}