using System.Security.Cryptography;

namespace WalkingDisaster.HashAlgorithmExperiments.Approaches;

internal class SingletonInstance : AbstractRunner
{
    private static readonly HashAlgorithm Hasher = SHA1.Create();

    protected override string Name => "Singleton Instance";
    protected override string Title => "Singleton Instance of SHA1 HashAlgorithm";

    protected override byte[] Run(byte[] input)
    {
        return Hasher.ComputeHash(input);
    }
}