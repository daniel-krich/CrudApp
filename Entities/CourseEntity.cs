using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudHW.Entities
{
    public class CourseEntity
    {
#nullable disable

        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
