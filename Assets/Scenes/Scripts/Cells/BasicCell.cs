using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCell : Cell
{
    public int eatableAmount;
    void Start()
    {
        InvokeCellStuff();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject other = col.gameObject;

        if (other.tag == "food")
        {
            Food food = other.GetComponent<Food>();

            if (food.type == "protein" || food.type == "simple")
            {
                EatFood(col.gameObject, eatableAmount*100);
            }
        }

    }
    void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "food")
        {
            Food food = other.GetComponent<Food>();
            
            if (food.type == "protein" || food.type == "simple")
            {
                EatFood(collision.gameObject, eatableAmount);
            }

        }
    }
    public override void SetInputNeurons(List<Neuron> inputNeurons)
    {
    }

    public override void SetOutputNeurons(List<Neuron> outputNeurons)
    {
    }

    public override List<int> GetInputNeuronsNumbers(int startingNumber)
    {
        return new List<int>();
    }

    public override List<int> GetOutputNeuronsNumbers(int startingNumber)
    {

        return new List<int>();
    }

    public override bool CanLiveAlone()
    {
        return true;
    }
}
