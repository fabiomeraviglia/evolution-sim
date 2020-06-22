using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OrganismHealthCheck
{
    public static void KillDeatachedCells(CellsStructure cells)
    {
        HashSet<Vector3> cellsToVisit = new HashSet<Vector3>();

        Vector3 currentPosition = new Vector3(0, 0);//center

        CellAttributes startingCell = cells.GetCell(currentPosition);
        
        if (startingCell==null || startingCell.alive == false)
        {//find center
            
            currentPosition = GetMinNumber(cells);

            if (currentPosition == new Vector3(0, 0))
                return; //no cells are alive
        }

        HashSet<Vector3> visitedCells = VisitCells(cells, cellsToVisit, currentPosition);

        //KILL ALL OF THE CELLS THAT WEREN'T VISITED
        KillNotVisited(cells, visitedCells);
    }

    private static Vector3 GetMinNumber(CellsStructure cells)
    {
        int minNumber = int.MaxValue;
        Vector3 cellPosition=new Vector3(0,0);
        foreach (CellAttributes cell in cells)
        {
            if (cell.alive && cell.number < minNumber)
            {
                minNumber = cell.number;
                cellPosition = cell.relativePosition;
            }
        }

        return cellPosition;
    }

    private static HashSet<Vector3> VisitCells(CellsStructure cells, HashSet<Vector3> cellsToVisit, Vector3 currentPosition)
    {
        HashSet<Vector3> visitedCells = new HashSet<Vector3>();
        cellsToVisit.Add(currentPosition);

        Vector3[] vectors = Cell.vectors;

        while (cellsToVisit.Any())
        {
            currentPosition = cellsToVisit.First();

            CellAttributes currentCell = cells.GetCell(currentPosition);

            for (int i = 0; i < vectors.Length; i++)
            {//find cells nearby to visit
             //checks that the direction is allowed by this cell
                if (CellLink(currentCell, vectors[i]))
                {
                    Vector3 pos = currentPosition + vectors[i];

                    if (!visitedCells.Contains(pos))
                    {
                        CellAttributes cell = cells.GetCell(pos);
                        if (cell != null && cell.alive && CellLink(cell,-vectors[i]))
                        {
                            cellsToVisit.Add(currentPosition + vectors[i]);
                        }
                    }
                }
            }

            visitedCells.Add(currentPosition);

            cellsToVisit.Remove(currentPosition);
        }
        return visitedCells;

    }

    private static bool CellLink(CellAttributes currentCell, Vector3 direction)
    {
        return CellsCreator.AllowGrowthInDirection(currentCell, direction);
    }

    private static void KillNotVisited(CellsStructure cells, HashSet<Vector3> visitedCells)
    {
        foreach (CellAttributes cell in cells)
        {
            if ((!visitedCells.Contains(cell.relativePosition)) && cell.alive)
            {
                cell.instance.Die(false);
            }
        }
    }

    public static bool AreValidCellsAlive(CellsStructure cells)
    {
        //Check that there is at least one cell that can live alone

        foreach (CellAttributes cell in cells)
        {
            if (cell.alive && cell.type.GetComponent<Cell>().CanLiveAlone())
            {
                return true;
            }
        }
        //if reach here all alive cells are not valid
        return false;
    }
}