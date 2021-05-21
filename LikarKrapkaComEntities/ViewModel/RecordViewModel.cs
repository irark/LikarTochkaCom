using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LikarKrapkaComEntities.Models;

namespace LikarKrapkaComEntities.ViewModel
{
    public class RecordViewModel
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "Дата")]
        [DateValidate]
        public DateTime? Date { get; set; }
        public string Note { get; set; }

        public virtual DoctorViewModel Doctor { get; set; }
        public virtual PatientViewModel Patient { get; set; }
        public RecordViewModel(Record dataItem)
        {
            DataItem = dataItem;
            if (DataItem == null)
            {
                DataItem = new Record();
                IsNewRow = true;
            }
            DoctorId = DataItem.DoctorId;
            Id = DataItem.Id;
            PatientId = DataItem.PatientId;
            Date = DataItem.Date;
            Note = DataItem.Note;

        }

        public Record DataItem { get; set; }
        public bool IsNewRow { get; set; }

        public Action StateHasChanged { get; set; }
    }
    public class DateValidate : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "Час повин належати проміжку від 08:00 до 18:00, інтервал між записами - 1 година";
        }

        protected override ValidationResult IsValid(object objValue,
                                                       ValidationContext validationContext)
        {
            var dateValue = objValue as DateTime? ?? new DateTime();

            //alter this as needed. I am doing the date comparison if the value is not null

            if (dateValue.Minute != 0 || dateValue.Second != 0 || dateValue.Hour < 8 || dateValue.Hour > 18)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
