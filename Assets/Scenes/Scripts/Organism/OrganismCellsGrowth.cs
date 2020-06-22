using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class OrganismCellsGrowth
{

    public static void FullyGrow(Organism organism)
    {
        int l = organism.orderOfGrowth.Count;
        for (int i = 0; i < l; i++)
            GrowCell(organism, false);
        OrganismHealthCheck.KillDeatachedCells(organism.cells);
    }
    public static void GrowCell(Organism organism, bool killDetached)
    {
        if (!organism.orderOfGrowth.Any())
            return;

        KeyValuePair<CellAttributes, CellAttributes> pair = organism.orderOfGrowth.First.Value;
        CellAttributes startingCell = pair.Key;
        CellAttributes cellToGrow = pair.Value;

        int cellEnergy = cellToGrow.type.GetComponent<Cell>().bodyEnergy;

        if (organism.organismEnergy.EggEnergy + organism.organismEnergy.Value - cellEnergy < organism.bodyEnergy * organism.chromosome.Parameters.MinimumEnergyToGrow)
            return;//no energy for growth


        if (startingCell == null)
        {//first cell
            if (CellsCreator.CanInstantiate(cellToGrow, organism.transform))
            {
                GrowCell(cellToGrow, organism);

            }
        }
        else
        {
            if (startingCell.alive)
            {

                Vector3 direction = cellToGrow.relativePosition;

                cellToGrow.relativePosition += startingCell.relativePosition;

                if (!CellsCreator.CanInstantiate(cellToGrow, organism.transform))
                {
                    cellToGrow.relativePosition = direction;
                    return;
                }

                ShiftCells(cellToGrow.relativePosition, direction, organism);

                GrowCell(cellToGrow, organism);
            }
        }

        if(killDetached)
            OrganismHealthCheck.KillDeatachedCells(organism.cells);
    }

    public static void GrowCell(CellAttributes cellToGrow, Organism organism)
    {

        organism.cells.AddCell(cellToGrow);

        CellsCreator.InstantiateCell(cellToGrow, organism.transform);

        organism.orderOfGrowth.RemoveFirst();

        SubtractEnergyForGrowth(cellToGrow.type.GetComponent<Cell>().bodyEnergy, organism.organismEnergy);

        organism.SetTotalEnergyStorage();

        int freeEnergyStorage = organism.organismEnergy.TotalEnergyStorage - organism.organismEnergy.Value;
        if (organism.organismEnergy.EggEnergy > freeEnergyStorage)
        {
            organism.organismEnergy.Value += freeEnergyStorage;
            organism.organismEnergy.EggEnergy -= freeEnergyStorage;
        }
        else
        {
            organism.organismEnergy.Value += organism.organismEnergy.EggEnergy;
            organism.organismEnergy.EggEnergy = 0;
        }


    }

    public static void SubtractEnergyForGrowth(int cellEnergy, OrganismEnergy organismEnergy)
    {
        if (organismEnergy.EggEnergy < cellEnergy)
        {
            organismEnergy.Value -= cellEnergy - organismEnergy.EggEnergy;
            organismEnergy.EggEnergy = 0;

        }
        else
        {
            organismEnergy.EggEnergy -= cellEnergy;
        }
    }


    public static void ShiftCells(Vector3 startingPoint, Vector3 direction, Organism organism)
    {
        Vector3 currentPos = startingPoint;
        CellAttributes currentCell = organism.cells.RemoveCell(currentPos);

        while (currentCell != null)
        {
            Vector3 shiftedPosition = currentPos + direction;

            currentCell.relativePosition = shiftedPosition;

            if (currentCell.instance != null)
            {
                var newPositionAndRotation = CellsCreator.RealPositionAndRotation(currentCell, organism.transform);
                currentCell.instance.transform.position = newPositionAndRotation.Key;
                currentCell.instance.transform.rotation = newPositionAndRotation.Value;

            }


            CellAttributes nextCell = organism.cells.RemoveCell(shiftedPosition);
            currentPos = shiftedPosition;


            organism.cells.AddCell(currentCell);

            currentCell = nextCell;
        }

    }


    public static CellAttributes CreateCell(OrganismEnergy organismEnergy, GameObject type, Vector3 relativePosition, Quaternion angle, int number, bool alive)
    {
        CellAttributes c = new CellAttributes();
        c.energy = organismEnergy;
        c.number = number;
        c.name = number.ToString();
        c.relativePosition = relativePosition;
        c.type = type;
        c.angle = angle;
        c.alive = alive;

        return c;
    }

}
