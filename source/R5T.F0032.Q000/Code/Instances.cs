using System;

using R5T.F0033;
using R5T.Z0015;


namespace R5T.F0032.Q000
{
    public static class Instances
    {
        public static IFilePaths FilePaths { get; } = Z0015.FilePaths.Instance;
        public static IJsonOperator JsonOperator { get; } = F0032.JsonOperator.Instance;
        public static INotepadPlusPlusOperator NotepadPlusPlusOperator { get; } = F0033.NotepadPlusPlusOperator.Instance;
    }
}