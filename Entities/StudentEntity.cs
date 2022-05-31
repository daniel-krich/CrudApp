using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudHW.Entities
{
    public class StudentEntity
    {
#nullable disable
        public int Id { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        public int Age { get; set; }

        public virtual ICollection<StudentCourseEntity> Courses { get; set; }
    }
}
