using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Читання даних з файлу INPUT.TXT
        var input = File.ReadAllLines("INPUT.TXT");
        var dimensions = input[0].Split();
        int N = int.Parse(dimensions[0]);
        int M = int.Parse(dimensions[1]);

        // Зчитуємо таблицю та створюємо копію для викреслення
        char[,] grid = new char[N, N];
        bool[,] used = new bool[N, N]; // Оголошуємо масив `used` тут

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                grid[i, j] = input[i + 1][j];
                used[i, j] = false; // Ініціалізуємо всі клітинки як невикреслені
            }
        }

        // Зчитуємо список слів
        var words = input.Skip(N + 1).Take(M).ToArray();

        // Пошук кожного слова в таблиці та викреслення
        foreach (var word in words)
        {
            bool found = false;
            // Використовуємо глобальний `used`, але скидаємо позначки для кожного нового слова
            bool[,] tempUsed = (bool[,])used.Clone();

            for (int i = 0; i < N && !found; i++)
            {
                for (int j = 0; j < N && !found; j++)
                {
                    if (SearchWord(grid, tempUsed, word, 0, i, j))
                    {
                        found = true;
                        // Оновлюємо `used`, позначаючи знайдені клітинки
                        for (int x = 0; x < N; x++)
                        {
                            for (int y = 0; y < N; y++)
                            {
                                if (tempUsed[x, y])
                                {
                                    used[x, y] = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Збір залишкових літер, які не були викреслені
        var remainingLetters = new List<char>();
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (!used[i, j])
                {
                    remainingLetters.Add(grid[i, j]);
                }
            }
        }

        // Сортуємо та виводимо залишкові літери в файл OUTPUT.TXT
        remainingLetters.Sort();
        File.WriteAllText("OUTPUT.TXT", string.Join("", remainingLetters));
    }

    // Функція пошуку слова з рекурсією
    static bool SearchWord(char[,] grid, bool[,] used, string word, int index, int x, int y)
    {
        if (index == word.Length) return true; // Успішний пошук слова

        // Перевірка меж та умов
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1) ||
            used[x, y] || grid[x, y] != word[index]) return false;

        // Відзначаємо поточну клітинку як викреслену
        used[x, y] = true;

        // Перевірка сусідніх клітинок (вверх, вниз, вліво, вправо)
        bool found = SearchWord(grid, used, word, index + 1, x - 1, y) ||
                     SearchWord(grid, used, word, index + 1, x + 1, y) ||
                     SearchWord(grid, used, word, index + 1, x, y - 1) ||
                     SearchWord(grid, used, word, index + 1, x, y + 1);

        // Якщо не знайшли слово, знімаємо позначку (backtracking)
        if (!found)
        {
            used[x, y] = false;
        }

        return found;
    }
}

