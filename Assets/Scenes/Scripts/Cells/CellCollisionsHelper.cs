using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CellCollisionsHelper
{
    public static int AbsorbCellMitigateCollision(Collision2D collision, Cell cellToKill)
    {

        float massPerCell = collision.rigidbody.mass / collision.gameObject.GetComponentsInChildren<Cell>().Length;

        int energy = cellToKill.ReleaseEnergy(false);

        cellToKill.Die(true,false);

        MitigateCollision(collision, massPerCell);

        return energy;
    }

    public static void KillCellMitigateCollision(Collision2D collision, Cell cellToKill)
    {
        float massPerCell = collision.rigidbody.mass / collision.gameObject.GetComponentsInChildren<Cell>().Length;

        cellToKill.Die(true);

        MitigateCollision(collision, massPerCell);
    }


    public static void BluntDamageCellMitigateCollision(Collision2D collision, Cell cellToDamage, int bluntDamage)
    {
        float massPerCell = collision.rigidbody.mass / collision.gameObject.GetComponentsInChildren<Cell>().Length;

        cellToDamage.TakeBluntDamage(bluntDamage);

        if (!cellToDamage.IsAlive())
        {
            MitigateCollision(collision, massPerCell);
        }
    }

    private static void MitigateCollision(Collision2D collision, float massPerCell)
    {
        float reduction = massPerCell / collision.otherRigidbody.mass;

        Organism organism = collision.otherRigidbody.GetComponentInParent<Organism>();

        organism.pp.CombineWithRigidbody(collision.otherRigidbody, 1 - reduction);

        collision.gameObject.GetComponentInParent<Organism>().pp.CombineWithRigidbody(collision.rigidbody, 1 - reduction);
    }
}