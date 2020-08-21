using System.Collections.Generic;
using NUnit.Framework;
using ToDataTable;

namespace ToDataTableTest
{
    public class Tests
    {
        private List<CustomerUDT> UserDefinedTableParameter;

        [SetUp]
        public void Setup()
        {
            UserDefinedTableParameter = new List<CustomerUDT>()
            {
                new CustomerUDT() {Name = "Mark", Age = 32},
                new CustomerUDT() {Name = "Andy", Age = 59},
                new CustomerUDT() {Name = "Allan", Age = 23},
                new CustomerUDT() {Name = "John", Age = 41}
            };
        }

        [Test]
        public void Test1()
        {
            var dataTable = UserDefinedTableParameter.ToDataTable();

            var sqlParameter = dataTable.ToSqlParameter("@asdada", "dbo.ut");

            Assert.Pass();
        }
    }
}