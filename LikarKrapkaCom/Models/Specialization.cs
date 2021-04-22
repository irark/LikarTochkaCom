using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LikarKrapkaCom.Models
{
    public class Specialization
    {
        public Specialization()
        {
            Doctors = new HashSet<Doctor>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
