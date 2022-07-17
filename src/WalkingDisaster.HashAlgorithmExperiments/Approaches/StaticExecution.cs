using System.Security.Cryptography;

namespace WalkingDisaster.HashAlgorithmExperiments.Approaches;

internal class StaticExecution : AbstractRunner
{
    protected override string Name => "Static Method";
    protected override string Title => "Static Execution of SHA1 HashAlgorithm on TryHashData";

    protected override byte[] Run(byte[] input)
    {
        var destination = new Span<byte>();
        SHA1.TryHashData(input, destination, out var bytesWritten);
        return destination.ToArray();
    }
}