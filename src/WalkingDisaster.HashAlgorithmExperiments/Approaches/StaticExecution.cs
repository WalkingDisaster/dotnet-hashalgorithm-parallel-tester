using System.Security.Cryptography;

namespace WalkingDisaster.HashAlgorithmExperiments.Approaches;

internal class StaticExecution : AbstractRunner
{
    private const int HashByteSize = 20; // Update this to match the output size of the algorithm you're using
    protected override string Name => "Static Method";
    protected override string Title => "Static Execution of SHA1 HashAlgorithm on TryHashData";

    protected override byte[] Run(byte[] input)
    {
        var destination = new Span<byte>(new byte[HashByteSize]);
        var hashed = SHA1.TryHashData(input, destination, out var bytesWritten);
        if (!hashed || bytesWritten == 0)
        {
            throw new InvalidOperationException("Failed to hash");
        }
        var output = destination.ToArray();
        return output;
    }
}