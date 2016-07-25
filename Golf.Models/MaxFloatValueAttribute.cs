using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Utility
{
    public class MaxFloatValueAttribute : ValidationAttribute
    {
        private readonly float _maxValue;

        public MaxFloatValueAttribute(float maxValue)
        {
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return (float)value <= _maxValue;
        }
    }
}