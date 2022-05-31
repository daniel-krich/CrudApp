using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrudHW.Extensions
{
    public static class DbContextExtensions
    {
        public static List<string>? ExecuteRaw(this DbContext dbContext, string? query)
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                dbContext.Database.GetDbConnection().Open();

                try
                {

                    using (var result = command.ExecuteReader())
                    {
                        var entities = new List<string>();

                        List<(string fieldName, int fieldOrdinal)> fields = Enumerable.Range(0, result.FieldCount).Select(index => (result.GetName(index), index)).ToList();

                        while (result.Read())
                        {
                            string[] fieldData = new string[fields.Count];

                            fields.ForEach(field => fieldData[field.fieldOrdinal] = $"{field.fieldName}: {result.GetValue(field.fieldOrdinal)}");

                            entities.Add($"({string.Join(", ", fieldData)})");
                        }

                        if(result.RecordsAffected >= 0)
                            entities.Add("Affected rows: " + result.RecordsAffected);

                        return entities;
                    }
                }
                catch(SqlException)
                {
                    return default;
                }
                finally
                {
                    dbContext.Database.GetDbConnection().Close();
                }
            }
        }

        public static Type? GetTableType(this DbContext dbContext, string? table_name)
        {
            return dbContext.Model.GetEntityTypes().FirstOrDefault(entityType =>
            {
                var tableNameAnnotation = entityType?.GetAnnotation("Relational:TableName");
                var tableName = tableNameAnnotation?.Value?.ToString();
                return tableName?.ToLower() == table_name?.ToLower();
            })?.ClrType;
        }
    }
}
