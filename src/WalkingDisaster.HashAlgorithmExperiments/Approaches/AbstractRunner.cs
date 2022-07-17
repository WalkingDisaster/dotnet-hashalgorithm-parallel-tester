using System.Text;

namespace WalkingDisaster.HashAlgorithmExperiments.Approaches;

internal abstract class AbstractRunner : ITestRun
{
    private event EventHandler<TestRunStatus> OnStatus = (_, _) => { };
    private event EventHandler Done = (_, _) => { };


    protected abstract string Title { get; }
    protected abstract string Name { get; }
    protected abstract byte[] Run(byte[] input);

    string ITestRun.Title => Title;
    string ITestRun.Name => Name;

    event EventHandler<TestRunStatus> ITestRun.OnStatus
    {
        add => OnStatus += value;
        remove => OnStatus -= value;
    }
    event EventHandler? ITestRun.Done
    {
        add => Done += value;
        remove => Done -= value;
    }

    void ITestRun.Execute(int rowNumber, int totalToRun, int degreeOfParallelism, int checkpoint)
    {
        var range = Enumerable.Range(0, totalToRun);
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = degreeOfParallelism
        };
        Task.Run(() =>
        {
            var totalErrors = 0;
            Parallel.ForEach(range, options, (i) =>
            {
                var theString = Guid.NewGuid().ToString("D");
                var bytes = Encoding.UTF8.GetBytes(theString);
                try
                {
                    Run(bytes);
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (Exception)
                {
                    totalErrors++;
                }
                finally
                {
                    if (i > 0 && i % checkpoint == 0)
                    {
                        OnStatus(this, new TestRunStatus((decimal) i / totalToRun, totalErrors));
                    }
                }
            });
            Done(this, EventArgs.Empty);
        });
    }
}
