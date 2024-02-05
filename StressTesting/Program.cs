using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StressTesting;

public static class Sort
{
    public static class Bubble
    {
        public static void Run(int[] arr, int n)
        {
            for (var i = 0; i < n - 1; i++)
            {
                for (var j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                }
            }
        }
    }

    public static class Qsort
    {
        private static int Partition(int[] arr, int low, int high)
        {
            var pivot = arr[high];
            var i = low - 1;

            for (var j = low; j <= high - 1; j++)
            {
                if (arr[j] >= pivot) 
                    continue;
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }

            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
            return i + 1;
        }

        private static void QuickSort(int[] arr, int low, int high)
        {
            if (low >= high)
                return;

            var pi = Partition(arr, low, high);

            QuickSort(arr, low, pi - 1);
            QuickSort(arr, pi + 1, high);
        }

        public static void Run(int[] arr, int n)
        {
            QuickSort(arr, 0, n - 1);
        }
    }

    public static class Shell
    {
        public static void Run(int[] arr, int n)
        {
            for (var gap = n / 2; gap > 0; gap /= 2)
            {
                for (var i = gap; i < n; i++)
                {
                    var temp = arr[i];
                    int j;

                    for (j = i; j >= gap && arr[j - gap] > temp; j -= gap)
                    {
                        arr[j] = arr[j - gap];
                    }
                    arr[j] = temp;
                }
            }
        }
    }

    public static class Selection
    {
        public static void Run(int[] arr, int n)
        {
            for (var i = 0; i < n - 1; i++)
            {
                var minIndex = i;
                for (var j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }

                (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
            }
        }
    }
}


internal static class Program
{
    private static void Main()
    {
        const int sizeArray = 100000;

        Console.WriteLine($"This example show 4 types sorting (bubble, qsort, shell, selection) for {sizeArray} elements");
        Console.WriteLine("Run...");

        var dataBubble = new int[sizeArray];
        var dataQsort = new int[sizeArray];
        var dataShell = new int[sizeArray];
        var dataSelection = new int[sizeArray];

        var rand = new Random();

        for (var i = 0; i != sizeArray; i++)
        {
            var value = rand.Next();

            dataBubble[i] = value;
            dataQsort[i] = value;
            dataShell[i] = value;
            dataSelection[i] = value;
        }

        var bubbleTask = Task.Run(() =>
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Sort.Bubble.Run(dataBubble, sizeArray);
            stopWatch.Stop();

            Console.WriteLine($"The time bubble: {stopWatch.ElapsedMilliseconds} ms");
        });

        var dataQsortTask = Task.Run(() =>
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Sort.Qsort.Run(dataQsort, sizeArray);
            stopWatch.Stop();

            Console.WriteLine($"The time qsort: {stopWatch.ElapsedMilliseconds} ms");
        });

        var dataShellTask = Task.Run(() =>
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Sort.Shell.Run(dataShell, sizeArray);
            stopWatch.Stop();

            Console.WriteLine($"The time shell: {stopWatch.ElapsedMilliseconds} ms");
        });

        var dataSelectionTask = Task.Run(() =>
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Sort.Selection.Run(dataSelection, sizeArray);
            stopWatch.Stop();

            Console.WriteLine($"The time selection: {stopWatch.ElapsedMilliseconds} ms");
        });

        Task.WaitAll(bubbleTask, dataQsortTask, dataShellTask, dataSelectionTask);

        Console.WriteLine("Press any key to close");
        Console.ReadKey();
    }
}