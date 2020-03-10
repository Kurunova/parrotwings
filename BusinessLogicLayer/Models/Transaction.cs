using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class Transaction
    {
        public int SenderUserId { get; set; }

        //[Required]
        //[Range(1, Int32.MaxValue)]
        //[Display(Name = "Receiver")]
        public int ReceiverUserId { get; set; }

        [Required]
        [Display(Name = "Receiver")]
        public string ReceiverUserName { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}