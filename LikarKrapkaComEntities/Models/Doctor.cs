using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LikarKrapkaComEntities.Models

{
    public class Doctor
    {
        public Doctor()
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
        public int HospitalId { get; set; }
        public int OfficeId { get; set; }
        public int SpecializationId { get; set; }
        [Display(Name = "Лікарня")]
        public virtual  Hospital Hospital { get; set; }
        [Display(Name = "Кабінет")]
        public virtual Office Office { get; set; }
        [Display(Name = "Спеціалiзація")]
        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }
}
