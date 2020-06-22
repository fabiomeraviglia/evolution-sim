using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacrofagoCell : Cell
{
    public int eatableAmount;
    void Start()
    {
        InvokeCellStuff();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "organism")
        {
            Cell cell = collision.collider.GetComponentInParent<Cell>();

            if (cell.IsAlive() && (cell is BasicCell || cell is DigestiveCell || cell is FatCell))
            {
                attributes.energy.Value += CellCollisionsHelper.AbsorbCellMitigateCollision(collision, cell);
            }

            if (cell.IsAlive() && (cell is PoisonCell))
            {
                cell.TakeDamage(100);

                CellCollisionsHelper.KillCellMitigateCollision(collision, this);
            }

        }


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "food")
        {
            Food food = collision.gameObject.GetComponent<Food>();
            if (food.type == "protein")
            {
                EatFood(collision.gameObject, eatableAmount);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "food")
        {
            Food food = collision.gameObject.GetComponent<Food>();
            if (food.type == "protein")
            {
                EatFood(collision.gameObject, eatableAmount*100);
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
