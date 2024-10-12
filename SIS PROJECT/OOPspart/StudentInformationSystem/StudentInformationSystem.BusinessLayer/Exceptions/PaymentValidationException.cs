﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Exceptions
{
    public class PaymentValidationException : System.Exception
    {
        public PaymentValidationException(string message) : base(message) { }
    }
}