using Microsoft.EntityFrameworkCore;
using System;

namespace SolaceTK.Core.Models
{
    public class SolTkCondition
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Data { get; set; }

        public SolTkOperator Operator { get; set; }


    }

    public enum SolTkOperator
    { 
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }
}
