﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class BooleanMustBeTrueAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }
}
