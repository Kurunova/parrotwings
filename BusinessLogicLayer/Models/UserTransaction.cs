using System;

namespace BusinessLogicLayer.Models
{
    public class UserTransaction
    {
        public TransactionType Type { get; set; }

        public string CorrespondentName { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Balance { get; set; }

        public string AmountSting
        {
            get
            {
                var result = $"{Amount}";
                switch (Type)
                {
                    case TransactionType.Outgoing:
                        result = $"-{Amount}";
                        break;
                }
                return result;
            }
        }
    }
}