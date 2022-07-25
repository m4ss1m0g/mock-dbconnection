using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace MockDBConnection.Kernel
{
    internal class MockedDbParameter : DbParameter
    {
        public override DbType DbType { get; set; }

        public override ParameterDirection Direction { get; set; }
        public override bool IsNullable { get; set; }

        [AllowNull]
        public override string ParameterName { get; set; }
        public override int Size { get; set; }

        [AllowNull]
        public override string SourceColumn { get; set; }
        public override bool SourceColumnNullMapping { get; set; }
        public override object? Value { get; set; }

        public override void ResetDbType()
        {

        }
    }

}