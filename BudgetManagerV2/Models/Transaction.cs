//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BudgetManagerV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Transaction
    {

        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill in a value")]
        public Nullable<double> Value { get; set; }
        [Required(ErrorMessage = "Please fill in a title")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Please fill in a date")]
        public DateTime Date { get; set; }
        public Nullable<int> FK_Category { get; set; }
        [DisplayName("Pick a category")]
        [Required(ErrorMessage = "Please fill in a category")]
        public virtual Category Category { get; set; }
    }
}
