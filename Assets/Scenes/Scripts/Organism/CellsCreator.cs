


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CellsCreator
{

    public static float CELL_WIDTH = 0.55f;
    public static int MAX_NUMBER_OF_CELLS=1000; 
    public static LinkedList<KeyValuePair<CellAttributes, CellAttributes>> GetOrderOfGrowth(Organism organism)
    {
        Chromosome chromosome = organism.chromosome;
        LinkedList<KeyValuePair<CellAttributes, CellAttributes>> orderOfGrowth = new LinkedList<KeyValuePair<CellAttributes, CellAttributes>>();//key= starting cell, value = new cell
        CellsStructure createdCells = new CellsStructure();

        foreach (Gene gene in chromosome.Genes)
        {
            if (!orderOfGrowth.Any())
            {

                CellAttributes firstCell = CreateCell(gene.Type, new Vector3(0, 0), new Quaternion(), gene.Number, organism.organismEnergy);


                orderOfGrowth.AddLast(new KeyValuePair<CellAttributes, CellAttributes>(null, firstCell));
                createdCells.AddCell(firstCell);

                continue;
            }

            if (createdCells.ContainsNumber(gene.StartingCell))
            {
                List<CellAttributes> targetCells = createdCells.GetCellsByNumber(gene.StartingCell);

                foreach (CellAttributes cell in targetCells)
                {
                    if (AllowGrowthInDirection(cell, gene.RelativePosition))
                    {
                        Vector3 pos = gene.RelativePosition;
                        Quaternion angle = ComputeAngle(gene.RelativePosition);

                        CellAttributes cellToGrow = CreateCell(gene.Type, pos, angle, gene.Number, organism.organismEnergy);
                        orderOfGrowth.AddLast(new KeyValuePair<CellAttributes, CellAttributes>(cell, cellToGrow));
                        createdCells.AddCell(cellToGrow);

                    }
                }
            }


            if (orderOfGrowth.Count >= MAX_NUMBER_OF_CELLS)
                return orderOfGrowth;
        }

        return orderOfGrowth;

    }

    public static bool AllowGrowthInDirection(CellAttributes cell, Vector3 relativePosition)
    {
        Vector3 rotatedPosition = Quaternion.Inverse(cell.angle) * relativePosition;
        Vector3[] vectors = Cell.vectors;

        for (int i = 0; i < vectors.Length; i++)
        {
            if (rotatedPosition == vectors[i])
                return cell.type.GetComponent<Cell>().growInDirection[i];
        }

        return false;
    }

    private static CellAttributes CreateCell(GameObject type, Vector3 relativePosition, Quaternion angle, int number, OrganismEnergy organismEnergy)
    {
        CellAttributes c = new CellAttributes();
        c.energy = organismEnergy;
        c.number = number;
        c.name = number.ToString();
        c.relativePosition = relativePosition;
        c.type = type;
        c.angle = angle;
        c.alive = true;

        return c;
    }
    private static Quaternion ComputeAngle(Vector3 relPos)
    {
        float degrees = Vector3.SignedAngle(new Vector3(0, 1), relPos, Vector3.forward);
        return Quaternion.Euler(Vector3.forward * degrees);
    }

    public static void InstantiateCell(CellAttributes cell, Transform parentTransform)
    {

        var realPostionAndRotation = RealPositionAndRotation(cell, parentTransform);
        GameObject c = UnityEngine.Object.Instantiate(cell.type, realPostionAndRotation.Key, realPostionAndRotation.Value);
        c.transform.parent = parentTransform;
        c.name = cell.name;

        c.GetComponent<Cell>().SetAttributes(cell);

    }
    /// <summary>
    /// Computes the real position and rotation of a cell given the relative position and the parent position and rotation
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="parentTransform"></param>
    /// <returns></returns>
    public static KeyValuePair<Vector3, Quaternion> RealPositionAndRotation(CellAttributes cell, Transform parentTransform)
    {
        Vector3 rotatedVector = parentTransform.rotation * cell.relativePosition;
        return new KeyValuePair<Vector3, Quaternion>(parentTransform.position + rotatedVector * CELL_WIDTH, cell.angle * parentTransform.rotation);

    }

    public static bool CanInstantiate(CellAttributes cell, Transform parentTransform)
    {
        var realPostionAndRotation = RealPositionAndRotation(cell, parentTransform);

        var res = Physics2D.OverlapCircle(realPostionAndRotation.Key, CELL_WIDTH / 2);
        if (res != null && !res.isTrigger && res.attachedRigidbody != parentTransform.GetComponent<Rigidbody2D>())
        {
            return false;
        }
        return true;
    }
}