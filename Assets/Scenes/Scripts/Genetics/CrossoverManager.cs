using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CrossoverManager
{

    
    public static int Difference<T>(T[] a, T[] b)
    {
        EfficientArray<T> a1 = new EfficientArray<T>(a);
        EfficientArray<T> b1 = new EfficientArray<T>(b);
        return Difference(a1, b1);
    }

    public static int Difference<T>(EfficientArray<T> a, EfficientArray<T> b)
    {

        if (a.Length == 0)
        {
            return b.Length;
        }
        if (b.Length == 0)
        {
            return a.Length;
        }

        int[,] lcs = PositionAndLengthLcs(a, b);

        if (lcs[0, 1] == 0)
        {//no substrings
            return Math.Max(a.Length, b.Length);
        }

        EfficientArray<T> a1 = SubArray(a, 0, lcs[0, 0]);
        EfficientArray<T> b1 = SubArray(b, 0, lcs[1, 0]);

        int upperA = lcs[0, 0] + lcs[0, 1], upperB = lcs[1, 0] + lcs[1, 1];
        EfficientArray<T> a2 = SubArray(a, upperA, a.Length - upperA);
        EfficientArray<T> b2 = SubArray(b, upperB, b.Length - upperB);

        return Difference(a1, b1) + Difference(a2, b2);

    }

    public static T[] Crossover<T>(T[] a, T[] b)
    {

        if (a.Length == 0)
        {
            return b;
        }
        if (b.Length == 0)
        {
            return a;
        }

        int[,] lcs = PositionAndLengthLcs(a, b);

        if (lcs[0, 1] == 0)
        {//no substrings
         //RANDOMLY RETURN ON OR THE OTHER
            if (GenesManager.r.Next(2) == 0)
                return a;
            return b;
            //  return UnionArray(a, b, new T[0]);
        }

        T[] a1 = SubArray(a, 0, lcs[0, 0]);
        T[] b1 = SubArray(b, 0, lcs[1, 0]);

        int upperA = lcs[0, 0] + lcs[0, 1], upperB = lcs[1, 0] + lcs[1, 1];
        T[] a2 = SubArray(a, upperA, a.Length - upperA);
        T[] b2 = SubArray(b, upperB, b.Length - upperB);

        if (GenesManager.r.Next(2) == 0)
            return UnionArray(Crossover(a1, b1), SubArray(a, lcs[0, 0], lcs[0, 1]), Crossover(a2, b2));
        return UnionArray(Crossover(a1, b1), SubArray(b, lcs[1, 0], lcs[1, 1]), Crossover(a2, b2));

    }

    private static T[] UnionArray<T>(T[] t1, T[] t2, T[] t3)
    {
        T[] result = new T[t1.Length + t2.Length + t3.Length];
        Array.Copy(t1, 0, result, 0, t1.Length);
        Array.Copy(t2, 0, result, t1.Length, t2.Length);
        Array.Copy(t3, 0, result, t1.Length + t2.Length, t3.Length);
        return result;
    }
    public static EfficientArray<T> SubArray<T>(this EfficientArray<T> data, int index, int length)
    {
        return new EfficientArray<T>(data, index, length);
    }

    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }
    public static int[,] PositionAndLengthLcs<T>(T[] a, T[] b)
    {
        return PositionAndLengthLcs(new EfficientArray<T>(a), new EfficientArray<T>(b));
    }
    public static int[,] PositionAndLengthLcs<T>(EfficientArray<T> a, EfficientArray<T> b)
    {
        var lengths = new int[a.Length, b.Length];
        int greatestLength = 0;
        int[,] positionAndLengthLcs = new int[2, 2]; //position and length, for a and b

        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < b.Length; j++)
            {
                if (a[i].Equals(b[j]))
                {
                    lengths[i, j] = i == 0 || j == 0 ? 1 : lengths[i - 1, j - 1] + 1;
                    if (lengths[i, j] > greatestLength)
                    {
                        greatestLength = lengths[i, j];

                        positionAndLengthLcs[0, 0] = i - greatestLength + 1;
                        positionAndLengthLcs[0, 1] = greatestLength;

                        positionAndLengthLcs[1, 0] = j - greatestLength + 1;
                        positionAndLengthLcs[1, 1] = greatestLength;
                    }
                }
                else
                {
                    lengths[i, j] = 0;
                }
            }
        }

        return positionAndLengthLcs;
    }

    public static ChromosomeParameters CrossParameters(ChromosomeParameters parameters1, ChromosomeParameters parameters2)
    {
        double[] p1 = parameters1.ToArray();
        double[] p2 = parameters2.ToArray();
        double[] res = new double[p1.Length];
        for (int i = 0; i < p1.Length; i++)
        {
            if (GenesManager.r.Next(2) == 0)
            {
                res[i] = p1[i];
            }
            else
            {
                res[i] = p2[i];
            }
        }

        return new ChromosomeParameters(res);
    }
    public class EfficientArray<T>
    {
        private T[] array;
        private int startIndex, length;
        public EfficientArray(T[] array)
        {
            this.array = array;
            startIndex = 0;
            length = array.Length;
        }
        public EfficientArray(EfficientArray<T> array)
        {
            this.array = array.array;
            startIndex = array.startIndex;
            length = array.length;
        }
        public EfficientArray(T[] array, int startIndex, int length)
        {
            this.array = array;
            this.startIndex = startIndex;
            this.length = length;
        }
        public EfficientArray(EfficientArray<T> array, int startIndex, int length)
        {
            this.array = array.array;
            this.startIndex = startIndex;
            this.length = length;
        }
        public int Length { get => length; }

        public T this[int i]
        {
            get { return array[startIndex + i]; }
            set => array[startIndex + i] = value;
        }
    }
}
