using Microsoft.EntityFrameworkCore;
using CrudHW.Data;
using CrudHW.Entities;
using CrudHW.Extensions;
using CrudHW.Factories;
using System.Reflection;
using System.Text.Json;

namespace CrudHW
{
    public class ConsoleMenu
    {
        private DataContext _dbContext;
        private Type? _tableType;
        public ConsoleMenu(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Run()
        {
            Console.WriteLine("1. Simplified Mode");
            Console.WriteLine("2. Expert Mode\n");
            Console.Write("Select item by number: ");
            int option = Convert.ToInt32(Console.ReadLine());
            switch (option)
            {
                case 1:
                    SimpleInterfaceMode();
                    break;
                case 2:
                    ExpertInterfaceMode();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("\nBye.");
                    break;
            }
        }

        private void SimpleInterfaceMode()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Connected to: {_dbContext.Database.GetDbConnection().DataSource}");
                Console.WriteLine($"Current db: {_dbContext.Database.GetDbConnection().Database}\n");

                DbTablesFactory tables = new DbTablesFactory(_dbContext);

                Console.WriteLine("Tables:");
                tables.Create().ForEach(x => Console.WriteLine($"\t{x.Name} ({string.Join(", ", x.Fields.Select(p => $"{p.Name} [{p.Type}]"))})"));

                if (_tableType == null)
                {
                tableSelection:
                    Console.Write("\nEnter table name: ");
                    string? tableName = Console.ReadLine();
                    _tableType = _dbContext.GetTableType(tableName);
                    if(_tableType == null)
                        goto tableSelection;
                }

                Console.WriteLine($"\nCurrent entity: {_tableType.Name}\n");
                Console.WriteLine("1. Create entity");
                Console.WriteLine("2. Delete entity");
                Console.WriteLine("3. Update entity");
                Console.WriteLine("4. Get entity data");
                Console.WriteLine("5. Get all entities data");
                Console.WriteLine("6. Change current table");
                Console.WriteLine("7. Quit\n");

                Console.Write("Select item by number: ");
                int option = Convert.ToInt32(Console.ReadLine());

                Console.Clear();

                switch (option)
                {
                    case 1:
                        SimpleCreate();
                        break;
                    case 2:
                        SimpleDelete();
                        break;
                    case 3:
                        SimpleUpdate();
                        break;
                    case 4:
                        SimpleGetEntity();
                        break;
                    case 5:
                        SimpleGetEntities();
                        break;
                    case 6:
                        _tableType = null;
                        break;
                    default:
                        Console.WriteLine("\nBye.");
                        return;
                }
            }
        }

        private void ExpertInterfaceMode()
        {
            Console.Clear();
            Console.WriteLine($"Connected to: {_dbContext.Database.GetDbConnection().DataSource}");
            Console.WriteLine($"Current db: {_dbContext.Database.GetDbConnection().Database}\n");
            DbTablesFactory tables = new DbTablesFactory(_dbContext);

            Console.WriteLine("Tables:");
            tables.Create().ForEach(x => Console.WriteLine($"\t{x.Name} ({string.Join(", ", x.Fields.Select(p => $"{p.Name} [{p.Type}]"))})"));

            while (true)
            {
                Console.Write("\nSQL Query: ");
                var rows = _dbContext.ExecuteRaw(Console.ReadLine());
                if (rows != null)
                {
                    rows.ForEach(row => Console.WriteLine($"\t{row}"));
                    Console.WriteLine("\n\tSuccess.");
                }
                else Console.WriteLine("\n\tQuery error.");
            }
        }

        private void SimpleCreate()
        {
            if (_tableType == null) return;

            object? entity = Activator.CreateInstance(_tableType);
            if (entity != null)
            {
                foreach (PropertyInfo prop in entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(string)))
                {
                    if (prop.Name.ToLower() == "id") continue;

                    Console.Write($"{prop.Name}: ");
                    object? input = Console.ReadLine();
                    if (prop.GetValue(entity)?.GetType() == typeof(int))
                    {
                        prop.SetValue(entity, Convert.ToInt32(input));
                    }
                    else
                    {
                        prop.SetValue(entity, input);
                    }
                }
                _dbContext.Add(entity);
                _dbContext.SaveChanges();
                Console.WriteLine("Success.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
            }
        }

        private void SimpleDelete()
        {
            if (_tableType == null) return;

            Console.Write("Delete record by record id: ");
            int recordId = Convert.ToInt32(Console.ReadLine());
            object? entity = Activator.CreateInstance(_tableType);
            PropertyInfo? prop = entity?.GetType().GetProperty("Id");
            prop?.SetValue(entity, recordId);
            if (entity != null)
            {
                _dbContext.Remove(entity);
                _dbContext.SaveChanges();
                Console.WriteLine("Success.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
            }
        }

        private void SimpleUpdate()
        {
            if (_tableType == null) return;

            Console.Write("Update record by record id: ");
            int recordId = Convert.ToInt32(Console.ReadLine());
            object? entity = _dbContext.Find(_tableType, recordId);
            if (entity != null)
            {
                Console.WriteLine("\n * Unchanged fields leave blank.\n");
                foreach (PropertyInfo prop in entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(string)))
                {
                    if (prop.Name.ToLower() == "id") continue;

                    Console.Write($"{prop.Name} (Value: {prop.GetValue(entity)}): ");
                    object? input = Console.ReadLine();
                    if (input?.IsNumericType() == true || input?.ToString()?.Length > 0)
                    {
                        if (prop.GetValue(entity)?.GetType() == typeof(int))
                        {
                            prop.SetValue(entity, Convert.ToInt32(input));
                        }
                        else
                        {
                            prop.SetValue(entity, input);
                        }
                    }
                }
                _dbContext.Update(entity);
                _dbContext.SaveChanges();
                Console.WriteLine("Success.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
            }
        }

        private void SimpleGetEntity()
        {
            if (_tableType == null) return;

            Console.Write("Get record by record id: ");
            int recordId = Convert.ToInt32(Console.ReadLine());
            object? entity = _dbContext.Find(_tableType, recordId);
            if (entity != null)
            {
                foreach (PropertyInfo prop in entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(string)))
                {
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(entity)}");
                }
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
            }
        }

        private void SimpleGetEntities()
        {
            var entityType = _dbContext.Model.GetEntityTypes().FirstOrDefault(x => x.ClrType == _tableType);
            var tableNameAnnotation = entityType?.GetAnnotation("Relational:TableName");
            var tableName = tableNameAnnotation?.Value?.ToString();

            var rows = _dbContext.ExecuteRaw($"SELECT * FROM {tableName}");
            if (rows != null)
            {
                Console.Write("Results:\n");
                rows.ForEach(row => Console.WriteLine($"\t{row}"));
            }
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}