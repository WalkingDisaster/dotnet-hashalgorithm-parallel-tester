using System.Security.Cryptography;

namespace WalkingDisaster.HashAlgorithmExperiments.Approaches;

internal class TransientInstance : AbstractRunner
{
    protected override string Name => "Transient Instances";
    protected override string Title => "Transient Instance of SHA1 HashAlgorithm";

    protected override byte[] Run(byte[] input)
    {
        return SHA1.Create().ComputeHash(input);
    }
}