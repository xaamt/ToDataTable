using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
namespace ToDataTable
{
    public static class UserDefinedTableHelper
    {
        /// <summary>
        /// Convert <see cref="T:System.Collections.Generic.List`1" /> to <see cref="T:System.Data.DataTable" />
        /// <para>Example: <example><code>DataTable dt = PersonList.ToDataTable()</code></example></para>
        /// </summary>
        /// <typeparam name="T">any Class</typeparam>
        /// <param name="recordList">Any IEnumerable of T</param>
        /// <returns>DataTable</returns>
        /// <exception cref="ArgumentNullException">If recordList is null</exception>
        public static DataTable ToDataTable<T>(this IEnumerable<T> recordList) where T : new()
        {
            if (recordList == null) throw new ArgumentNullException(nameof(recordList), "The parameter can't be null");
            var _type = typeof(T);
            var columns = GetColumnsInfo(_type);
            var dt = new DataTable()
                .AddColumns(columns)
                .AddRows(columns, recordList);
            return dt;
        }

        /// <summary>
        /// Convert <see cref="T:System.Data.DataTable" /> to <see cref="T:Microsoft.Data.SqlClient.SqlParameter" />
        /// <para>Example: <example><code>SqlParameter param = dataTable.ToSqlParameter("@inputDT", "dbo.UDT")</code></example></para>
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sqlParameterName">Name of Stored-Procedure Parameter to pass with SqlParameter</param>
        /// <param name="SqlDataTableName">Name of User Defined Table Type in SQL Server</param>
        /// <returns>SqlParameter</returns>
        /// <exception cref="ArgumentNullException">If any input param is null</exception>
        public static SqlParameter ToSqlParameter(this DataTable dataTable, string sqlParameterName, string SqlDataTableName)
        {
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));
            if (string.IsNullOrWhiteSpace(sqlParameterName)) throw new ArgumentNullException(nameof(sqlParameterName));
            if (string.IsNullOrWhiteSpace(SqlDataTableName)) throw new ArgumentNullException(nameof(SqlDataTableName));

            var sqlParameter = new SqlParameter(sqlParameterName, dataTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = SqlDataTableName
            };
            return sqlParameter;
        }

        private static DataTable AddColumns(this DataTable dt, IEnumerable<ColumnInfo> columns)
        {
            foreach (var column in columns)
            {
                var type = column.Property.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                dt.Columns.Add(column.PropertyName, type);
            }
            return dt;
        }

        private static DataTable AddRows<T>(this DataTable dt, IReadOnlyCollection<ColumnInfo> columns, IEnumerable<T> list) where T : new()
        {
            foreach (var item in list)
            {
                var row = dt.NewRow();
                foreach (var column in columns)
                {
                    var value = column.Property.GetValue(item, null);
                    row[column.PropertyName] = (object)value ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static List<ColumnInfo> GetColumnsInfo(Type type)
        {
            var columns = new List<ColumnInfo>();
            var counter = 1;
            foreach (var propertyInfo in type.GetProperties())
            {
                var attribute = GetAttribute<UserDefinedTableTypeColumnAttribute>(propertyInfo);
                var column = new ColumnInfo
                {
                    PropertyName = !string.IsNullOrWhiteSpace(attribute?.Name) ? attribute.Name : propertyInfo.Name,
                    PropertyOrder = attribute?.Order ?? counter,
                    Property = propertyInfo,
                };
                columns.Add(column);
                counter++;
            }
            return columns.OrderBy(info => info.PropertyOrder).ToList();
        }

        private static T GetAttribute<T>(PropertyInfo propertyInfo) where T : Attribute
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(T), false);
            if (attributes.Any()) return (T)attributes.First();
            return null;
        }

        private class ColumnInfo
        {
            public string PropertyName { get; set; }
            public int PropertyOrder { get; set; }
            public PropertyInfo Property { get; set; }
        }
    }

}
