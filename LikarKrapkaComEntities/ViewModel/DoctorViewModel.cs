using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LikarKrapkaComEntities.Models;

namespace LikarKrapkaComEntities.ViewModel
{
    public class DoctorViewModel
    {
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
        public virtual HospitalViewModel Hospital { get; set; }
        [Display(Name = "Кабінет")]
        public virtual OfficeViewModel Office { get; set; }
        [Display(Name = "Спеціалiзація")]
        public virtual SpecializationViewModel Specialization { get; set; }
        public virtual ICollection<RecordViewModel> Records { get; set; }

        public DoctorViewModel(Doctor dataItem)
        {
            DataItem = dataItem;
            if (DataItem == null)
            {
                DataItem = new Doctor();
                IsNewRow = true;
            }
            FirstName = DataItem.FirstName;
            Id = DataItem.Id;
            LastName = DataItem.LastName;
            SpecializationId = DataItem.SpecializationId;
            HospitalId = DataItem.HospitalId;
            OfficeId = DataItem.OfficeId;
            PhoneNumber = DataItem.PhoneNumber;

        }

        public Doctor DataItem { get; set; }
        public bool IsNewRow { get; set; }

        public Action StateHasChanged { get; set; }
    }
}
