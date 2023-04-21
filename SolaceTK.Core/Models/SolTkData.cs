﻿using Microsoft.EntityFrameworkCore;
using System;

namespace SolaceTK.Core.Models
{
    //[Owned]
    public class SolTkData
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Data { get; set; }
        public SolTkDataOperator Operator { get; set; }
    }

    public enum SolTkDataOperator
    {
        Set,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulus,
        Exponential
    }
}
