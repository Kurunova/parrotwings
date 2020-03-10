using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Email { get; set; }

        public string Password { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Transaction> SendingTransaction { get; set; }

        public virtual ICollection<Transaction> ReceiveTransaction { get; set; }
    }
}