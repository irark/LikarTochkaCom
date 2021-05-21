using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LikarKrapkaComEntities.Models;

namespace LikarKrapkaComEntities.ViewModel
{
    public class PatientViewModel
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
        public virtual ICollection<RecordViewModel> Records { get; set; }
        public PatientViewModel(Patient dataItem)
        {
            DataItem = dataItem;
            if (DataItem == null)
            {
                DataItem = new Patient();
                IsNewRow = true;
            }
            FirstName = DataItem.FirstName;
            Id = DataItem.Id;
            LastName = DataItem.LastName;
            PhoneNumber = DataItem.PhoneNumber;

        }

        public Patient DataItem { get; set; }
        public bool IsNewRow { get; set; }

        public Action StateHasChanged { get; set; }
    }
}
