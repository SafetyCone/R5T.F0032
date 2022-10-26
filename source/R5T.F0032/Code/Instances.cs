using System;

using R5T.F0000;


namespace R5T.F0032
{
    public static class Instances
    {
        public static IFileOperator FileOperator { get; } = F0000.FileOperator.Instance;
        public static IJsonOperator JsonOperator { get; } = F0032.JsonOperator.Instance;
        public static IMemoryStreamOperator MemoryStreamOperator { get; } = F0000.MemoryStreamOperator.Instance;
    }
}