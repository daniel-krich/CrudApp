using Microsoft.EntityFrameworkCore.Metadata;
using CrudHW.Data;
using CrudHW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudHW.Factories
{
    public class DbTablesFactory
    {
        private DataContext _dbContext;
        public DbTablesFactory(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<DbTableModel> Create()
        {
            return _dbContext.Model.GetEntityTypes().ToList().Select(entityType =>
            {
                var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
                var tableName = tableNameAnnotation?.Value?.ToString();
                return new DbTableModel
                {
                    Name = tableName,
                    Fields = entityType.GetProperties().Select(p => new DbFieldModel { Name = p.Name, Type = p.ClrType.Name }).ToList()
                };
            }).ToList();
        }
    }
}