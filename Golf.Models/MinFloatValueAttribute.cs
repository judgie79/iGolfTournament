using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Utility
{
    public class MinFloatValueAttribute : ValidationAttribute
    {
        private readonly float _minValue;

        public MinFloatValueAttribute(float minValue)
        {
            _minValue = minValue;
        }

        public override bool IsValid(object value)
        {
            return (float)value >= _minValue;
        }
    }
}