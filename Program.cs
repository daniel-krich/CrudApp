using Microsoft.EntityFrameworkCore;
using CrudHW.Data;
using CrudHW.Entities;
using CrudHW.Extensions;
using CrudHW.Factories;
using CrudHW;

using (var dbContext = new DataContext())
{
    var menu = new ConsoleMenu(dbContext);
    menu.Run();
}