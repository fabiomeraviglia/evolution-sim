using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheratinaCell : Cell
{
    void Start()
    {
        InvokeCellStuff();

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "organism")
        {
            Cell cell = collision.collider.GetComponentInParent<Cell>();

            int damage = GetBluntDamage(collision);

            CellCollisionsHelper.BluntDamageCellMitigateCollision(collision, cell, damage);

        }
        if (collision.gameObject.tag == "shell")
        {

            int damage = GetBluntDamage(collision);
            ShellFood shell = collision.collider.GetComponent<ShellFood>();
            shell.TakeBluntDamage(damage);

        }
    }

    private int GetBluntDamage(Collision2D collision)
    {
        return Math.Max((int)(Math.Pow(collision.relativeVelocity.magnitude * 3, 2) * transform.parent.GetComponent<Rigidbody2D>().mass) - 100, 0);
    }

    public override void TakeBluntDamage(int damage)
    {
        int reducedDamage = (int)damage/10;
        TakeDamage(reducedDamage);
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
        return false;
    }
}
