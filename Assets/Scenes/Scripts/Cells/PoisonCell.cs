using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCell : Cell
{
    void Start()
    {
        InvokeCellStuff();

    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "organism")
        {
            Cell cell = collision.collider.GetComponentInParent<Cell>();
            if (cell.GetType() != typeof(PoisonCell))
            {
                cell.TakeDamage(5);//posion and damage other cell
            }
        }
        if (collision.gameObject.tag == "shell")
        {

            ShellFood shell = collision.collider.GetComponent<ShellFood>();
            
            shell.TakeDamage(5);
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
