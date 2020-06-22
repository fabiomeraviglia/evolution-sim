using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ParameterFitness : ISerializable
{

    public double average;
    public double squaredAverage;
    public int n;
    public double[] values;

    public ParameterFitness()
    {
        this.average = 0;
        this.squaredAverage = 0;
        this.n = 0;
    }

    public ParameterFitness(double average, double squaredAverage, int n)
    {
        this.average = average;
        this.squaredAverage = squaredAverage;
        this.n = n;
    }

    public double GetVariance()
    {
        if (n < 2) return double.PositiveInfinity;
        double variance = (double)n / (double)(n - 1) * (squaredAverage - Math.Pow(average, 2));
        return variance;
    }

    public void AddValue(double fitness)
    {
        average = (average * n + fitness) / (n + 1);
        squaredAverage = (squaredAverage * n + Math.Pow(fitness, 2)) / (n + 1);
        n++;
        UpdateValues(fitness);
    }

    private void UpdateValues(double fitness)
    {
        if (values == null)
        {
            values = new double[0];
        }

        double[] nvalues = new double[values.Length + 1];
        for (int i = 0; i < values.Length;i++)
        {
            nvalues[i] = values[i];
        }
        nvalues[values.Length] = fitness;
        values = nvalues;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("average", average, typeof(double));
        info.AddValue("squaredAverage", squaredAverage, typeof(double));
        info.AddValue("n", n, typeof(int));
        info.AddValue("values", values, typeof(double[]));
    }

    public ParameterFitness(SerializationInfo info, StreamingContext context)
    {
        // Reset the property value using the GetValue method.
        average = (double)info.GetValue("average", typeof(double));
        squaredAverage = (double)info.GetValue("squaredAverage", typeof(double));
        n = (int)info.GetValue("n", typeof(int));
        values = (double[])info.GetValue("values", typeof(double[]));
    }
}
