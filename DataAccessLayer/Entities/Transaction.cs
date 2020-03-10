using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(SenderUserId))]
        public virtual User SenderUser { get; set; }

        [ForeignKey(nameof(ReceiverUserId))]
        public virtual User ReceiverUser { get; set; }
    }
}