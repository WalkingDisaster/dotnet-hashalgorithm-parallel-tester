namespace WalkingDisaster.HashAlgorithmExperiments;

internal readonly record struct RunResult(string Name, long TotalTimeInMilliseconds, int IterationsRun, int[] GarbageCollectionByGeneration)
{
    internal decimal AverageMillisecondsPerRun => (decimal)TotalTimeInMilliseconds / IterationsRun;
}