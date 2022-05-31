using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudHW.Models
{
    public class DbTableModel
    {
        public string? Name { get; set; }
        public List<DbFieldModel> Fields { get; set; } = new();
    }

    public class DbFieldModel
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}
