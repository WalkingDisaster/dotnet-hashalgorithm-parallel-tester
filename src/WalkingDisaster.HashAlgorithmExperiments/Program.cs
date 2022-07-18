using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using WalkingDisaster.HashAlgorithmExperiments;
using WalkingDisaster.HashAlgorithmExperiments.Approaches;

const int totalIterations = 100000000;
const int degreeOfParallelism = 10;
const int checkpoint = 10000;

var runs = new ITestRun[]
{
    new StaticExecution(),
    new ThreadStaticInstance(),
    new TransientInstance(),
    new SingletonInstance()
};

Console.Clear();
Console.WriteLine("Concurrency Tester");
Console.WriteLine($"Iterations: {totalIterations:0,000}, Degree of Parallelism: {degreeOfParallelism}");

var currentRow = 3;
var maxGeneration = GC.MaxGeneration;
var lastValues = Enumerable
    .Range(0, maxGeneration + 1)
    .Select(i => 0)
    .ToArray();

List<RunResult> results = new();

foreach (var run in runs)
{
    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

    Console.SetCursorPosition(0, currentRow);
    Console.Write($"Running: {run.Title}");
    currentRow++;
    var totalErrors = 0;

    ConcurrentQueue<string> messages = new();
    run.OnStatus += (_, e) =>
    {
        messages.Enqueue($"Pct Complete: {e.PercentComplete * 100:00.0}%, Total Errors: {e.TotalErrors}");
    };
    var stopwatch = new Stopwatch();
    run.Done += (_, _) => stopwatch.Stop();
    stopwatch.Start();
    run.Execute(currentRow, totalIterations, degreeOfParallelism, checkpoint);
    while (stopwatch.IsRunning)
    {
        while (messages.TryDequeue(out var message))
        {
            Console.SetCursorPosition(2, currentRow);
            Console.Write(message);
        }
        Thread.Sleep(100);
    }

    Console.SetCursorPosition(2, currentRow);
    var msPerIteration = (decimal)stopwatch.ElapsedMilliseconds / totalIterations;
    Console.Write("Time: ");
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write($"{stopwatch.ElapsedMilliseconds:#,###}ms");
    Console.ResetColor();
    Console.Write(", Iteration Time: ");
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write($"{msPerIteration:0.000000}ms");
    Console.ResetColor();
    Console.Write(", Errors: ");
    Console.ForegroundColor = totalErrors > 0 ? ConsoleColor.Red : ConsoleColor.Magenta;
    Console.Write(totalErrors);
    Console.ResetColor();

    for (var generation = 0; generation < lastValues.Length; generation++)
    {
        var lastGcCount = lastValues[generation];
        var currentGcCount = GC.CollectionCount(generation) - lastGcCount;
        lastValues[generation] = currentGcCount;
    }

    results.Add(new RunResult(run.Name, stopwatch.ElapsedMilliseconds, totalIterations, lastValues.ToArray()));

    Console.SetCursorPosition(2, ++currentRow);
    Console.Write("Garbage Collection: " + string.Join(',', lastValues.Select((count, generation) => $"Gen{generation}={count}").ToArray()));
    currentRow += 2;
}

Console.SetCursorPosition(0, ++currentRow);
Console.Write("\"Name\", \"TotalTimeInMilliseconds\", \"IterationsRun\", \"AverageMillisecondsPerRun\",");
Console.WriteLine(string.Join(", ", Enumerable.Range(0, maxGeneration +  1).Select(i => $"\"Gen{i}\"").ToArray()));

foreach (var result in results)
{
    Console.Write($"\"{result.Name}\", {result.TotalTimeInMilliseconds}, {result.IterationsRun}, {result.AverageMillisecondsPerRun},");
    Console.WriteLine(string.Join(", ", result.GarbageCollectionByGeneration.Select(ms => ms).ToArray()));
}

Console.WriteLine("Done!");
