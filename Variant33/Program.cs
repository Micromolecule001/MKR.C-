using System;
using System.Collections.Generic;
using System.IO;

public class PuzzleSolver
{
    private char[,] grid;
    private int N;
    private bool[,] used;
    private int[] dx = { 1, -1, 0, 0 }; // Directional arrays for movement (down, up, right, left)
    private int[] dy = { 0, 0, 1, -1 };

    // Constructor to initialize the PuzzleSolver with a grid and size
    public PuzzleSolver(char[,] grid, int N)
    {
        this.grid = grid;
        this.N = N;
        this.used = new bool[N, N]; // Keeps track of used cells
    }

    public bool FindWord(string word)
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (grid[i, j] == word[0])
                {
                    if (SearchWord(word, i, j, 0)) return true;
                }
            }
        }
        return false;
    }

    public bool SearchWord(string word, int x, int y, int index)
    {
        if (index == word.Length) return true;
        if (x < 0 || x >= N || y < 0 || y >= N || used[x, y] || grid[x, y] != word[index]) return false;

        used[x, y] = true;

        for (int dir = 0; dir < 4; dir++)
        {
            int newX = x + dx[dir];
            int newY = y + dy[dir];
            if (SearchWord(word, newX, newY, index + 1)) return true;
        }

        used[x, y] = false;
        return false;
    }

    public void RemoveWord(string word)
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (grid[i, j] == word[0])
                {
                    if (SearchAndRemove(word, i, j, 0))
                        return; // If found, exit
                }
            }
        }
    }

    public bool SearchAndRemove(string word, int x, int y, int index)
    {
        if (index == word.Length) return true;

        if (x < 0 || x >= N || y < 0 || y >= N || grid[x, y] != word[index])
            return false;

        char temp = grid[x, y];
        grid[x, y] = '-'; // Mark as removed

        bool found = SearchAndRemove(word, x + 1, y, index + 1) || // down
                     SearchAndRemove(word, x - 1, y, index + 1) || // up
                     SearchAndRemove(word, x, y + 1, index + 1) || // right
                     SearchAndRemove(word, x, y - 1, index + 1);   // left

        if (!found)
        {
            grid[x, y] = temp; // Restore the character if not found
        }

        return found;
    }

    public List<char> GetRemainingLetters()
    {
        List<char> remaining = new List<char>();
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (grid[i, j] != '-') // Check for remaining letters
                {
                    remaining.Add(grid[i, j]);
                }
            }
        }
        return remaining;
    }

    public void PrintGrid(string message)
    {
        Console.WriteLine(message);
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write(grid[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string[] input = File.ReadAllLines("INPUT.TXT");

        Console.WriteLine("==== INPUT.TXT ====");
        foreach (var value in input)
        {
            Console.WriteLine(value);
        }
        Console.WriteLine("==== END INPUT ====");

        var gridSize = input[0].Split();
        int N = int.Parse(gridSize[0]);
        int M = int.Parse(gridSize[1]);

        // Initialize the grid properly
        char[,] grid = new char[N, N];
        for (int i = 0; i < N; i++)
        {

            Console.WriteLine($"Line number {i}");
            for (int j = 0; j < N; j++)
            {
                Console.WriteLine($"Column number {j}");
                grid[i, j] = input[i + 1][j]; // Fill the grid row by row
                Console.WriteLine($"{grid[i, j]}");
            }
        }

        var solver = new PuzzleSolver(grid, N);

        solver.PrintGrid("=== Before ===");

        // Remove words as per the input
        for (int i = 0; i < M; i++)
        {
            solver.RemoveWord(input[N + 1 + i]);
        }

        // Get remaining letters and write them to OUTPUT.TXT
        var result = solver.GetRemainingLetters();
        Console.WriteLine("Result List<char>: " + string.Join(", ", result));
        File.WriteAllLines("OUTPUT.TXT", new string[] { string.Join("", result) });

        // Optional: Print the final grid to the console
        solver.PrintGrid("=== Grid After ===");
    }
}
