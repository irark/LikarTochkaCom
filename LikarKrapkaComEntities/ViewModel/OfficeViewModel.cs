using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LikarKrapkaComEntities.Models;

namespace LikarKrapkaComEntities.ViewModel
{
    public class OfficeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинне бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        public virtual ICollection<DoctorViewModel> Doctors { get; set; }
        public OfficeViewModel(Office dataItem)
        {
            DataItem = dataItem;
            if (DataItem == null)
            {
                DataItem = new Office();
                IsNewRow = true;
            }
            Name = DataItem.Name;
            Id = DataItem.Id;
            

        }

        public Office DataItem { get; set; }
        public bool IsNewRow { get; set; }

        public Action StateHasChanged { get; set; }
    }
}
