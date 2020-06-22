using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellsStructure : IEnumerable<CellAttributes>
{
    public int deadCells = 0;

    protected Dictionary<int, List<CellAttributes>> cellsByNumber = new Dictionary<int, List<CellAttributes>>();
    protected Dictionary<Vector3, CellAttributes> cellsByPosition = new Dictionary<Vector3, CellAttributes>();
    public void AddCell(CellAttributes cell)
    {
        if (!cellsByNumber.ContainsKey(cell.number))
        {
            cellsByNumber[cell.number] = new List<CellAttributes>();
        }
        cellsByNumber[cell.number].Add(cell);

        cellsByPosition[cell.relativePosition] = cell;
    }

    internal bool ContainsNumber(int number)
    {
        return cellsByNumber.ContainsKey(number) && cellsByNumber[number].Any();
    }

    internal List<CellAttributes> GetCellsByNumber(int number)
    {
        if (!ContainsNumber(number))
            return null;
        return new List<CellAttributes>(cellsByNumber[number]);
    }

    internal CellAttributes GetCell(Vector3 currentPos)
    {
        if (!cellsByPosition.ContainsKey(currentPos))
            return null;
        return cellsByPosition[currentPos];
    }

    internal CellAttributes RemoveCell(Vector3 currentPos)
    {
        if (!cellsByPosition.ContainsKey(currentPos))
            return null;

        CellAttributes cell = cellsByPosition[currentPos];

        cellsByPosition.Remove(cell.relativePosition);
        cellsByNumber[cell.number].Remove(cell);
        return cell;
    }

    public IEnumerator<CellAttributes> GetEnumerator()
    {
        return cellsByPosition.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
