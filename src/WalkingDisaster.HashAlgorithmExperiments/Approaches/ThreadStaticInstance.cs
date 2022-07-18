using System.Security.Cryptography;

namespace WalkingDisaster.HashAlgorithmExperiments.Approaches;

internal class ThreadStaticInstance : AbstractRunner
{
    [ThreadStatic] private static HashAlgorithm? _hasher;

    protected override string Name => "Thread Static";
    protected override string Title => "Thread Static Instance of SHA1 HashAlgorithm";

    protected override byte[] Run(byte[] input)
    {
        _hasher ??= SHA1.Create();
        return _hasher.ComputeHash(input);
    }

}