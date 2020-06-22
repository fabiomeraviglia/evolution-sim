﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class SubstringsHelper
{


    public static string Lcs(string a, string b)
    {
        var lengths = new int[a.Length, b.Length];
        int greatestLength = 0;
        string output = "";
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < b.Length; j++)
            {
                if (a[i] == b[j])
                {
                    lengths[i, j] = i == 0 || j == 0 ? 1 : lengths[i - 1, j - 1] + 1;
                    if (lengths[i, j] > greatestLength)
                    {
                        greatestLength = lengths[i, j];
                        output = a.Substring(i - greatestLength + 1, greatestLength);
                    }
                }
                else
                {
                    lengths[i, j] = 0;
                }
            }
        }
        return output;
    }
}
