namespace WalkingDisaster.HashAlgorithmExperiments;

internal interface ITestRun
{
    internal string Name { get; }
    internal string Title { get; }
    internal event EventHandler<TestRunStatus> OnStatus;
    internal event EventHandler Done;
    void Execute(int rowNumber, int totalToRun, int degreeOfParallelism, int checkpoint);
}
