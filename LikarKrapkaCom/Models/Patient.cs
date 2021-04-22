using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LikarKrapkaCom.Models
{
    public class Patient
    {
        public Patient()
        {
            Records = new HashSet<Record>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "Ім\'я")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }
        [RegularExpression(@"^[+]?([0-9]{2})?\(?([0-9]{3})\)?[-. ]?[)]?([0-9]{3})[-. ]?([0-9]{2})?[-. ]?([0-9]{2})$", ErrorMessage = "Невірно формат")]
        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }
}
