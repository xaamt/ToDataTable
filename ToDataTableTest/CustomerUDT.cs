using System;

using ToDataTable;

namespace ToDataTableTest
{
    public class CustomerUDT
    {
        [UserDefinedTableTypeColumn(1, "Hasan")]
        public String Name { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public int? Age { get; set; }
    }
}
