using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // INPUT
        int N = int.Parse(File.ReadAllText("INPUT.TXT"));

        // dotal dot amount 
        long totalDiamonds = 0;
        for (int i = 0; i <= N; i++)
        {
            for (int j = i; j <= N; j++)
            {
                totalDiamonds += i + j;
            }
        }

        // OUTPUT
        File.WriteAllText("OUTPUT.TXT", totalDiamonds.ToString());

        Console.WriteLine($"Сумарна кількість точок: {totalDiamonds}");
    }
}

