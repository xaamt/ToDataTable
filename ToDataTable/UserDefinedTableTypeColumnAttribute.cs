using System;

namespace ToDataTable
{
    /// <summary>
    /// This attribute can customize column name and order of converted DataTable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class UserDefinedTableTypeColumnAttribute : Attribute
    {
        /// <summary>
        /// This attribute can customize column name and order of converted DataTable
        /// </summary>
        /// <param name="order">Order of column in DataTable</param>
        public UserDefinedTableTypeColumnAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// This attribute can customize column name and order of converted DataTable
        /// </summary>
        /// <param name="name">Name of column in DataTable</param>
        public UserDefinedTableTypeColumnAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// This attribute can customize column name and order of converted DataTable
        /// </summary>
        /// <param name="order">Order of column in DataTable</param>
        /// <param name="name">Name of column in DataTable</param>
        public UserDefinedTableTypeColumnAttribute(int order, string name)
        {
            Order = order;
            Name = name;
        }

        public int Order { get; set; }

        public string Name { get; set; }
    }
}
