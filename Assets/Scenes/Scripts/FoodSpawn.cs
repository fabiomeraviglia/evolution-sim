using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{

    public int MAX_FOOD;
    public int foodCount = 0;
    public Vector2 startingPoint;
    public Vector2 size;

    public float foodRate;//amount of food for every 5 seconds
    System.Random r = new System.Random();

    public GameObject shellFood;
    public GameObject movableFood;
    public static GameObject FOOD;
    public static GameObject MOVABLE_FOOD;
    public GameObject protein;
    public static GameObject PROTEIN;
    // Start is called before the first frame update
    void Start()
    {

        FOOD = shellFood;
        MOVABLE_FOOD = movableFood;
        PROTEIN = protein;
        InvokeRepeating("UpdateFoodCount", 5f, 10f);
        InvokeRepeating("SpawnFood", 5f, 5f);
    }

    public void SpawnAllFood()
    {
        while (foodCount < MAX_FOOD)
        {
            SpawnFood();
        }
    }
    public void SpawnFood()
    {
        for (int i = 0; i < foodRate && ((foodRate - i) >=1 || r.NextDouble()<(foodRate-i)) && foodCount < MAX_FOOD; i++)
        {
            float x, y;
            if (startingPoint == null)
            {
                x = (float)(r.NextDouble() * (Convert.ToDouble(Hyperparameters.MAP_SIZE)));
                y = (float)(r.NextDouble() * (Convert.ToDouble(Hyperparameters.MAP_SIZE))); //PROBLEMA DI SPAWN SUI BORDI??
            }
            else
            {
                x = (float)(r.NextDouble() * size.x) + startingPoint.x;
                y = (float)(r.NextDouble() * size.y) + startingPoint.y; 
            }
            if (r.NextDouble() < 0.15)
            {
                Instantiate(shellFood, new Vector3(x, y), new Quaternion());
            }
            else
            {
                Instantiate(movableFood, new Vector3(x, y), new Quaternion());
            }
            foodCount++;
        }
    }




    public void UpdateFoodCount()
    {
        GameObject[] food = GameObject.FindGameObjectsWithTag("food");
        foodCount = food.Length;
    }
}
