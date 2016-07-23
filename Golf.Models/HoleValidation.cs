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

    public class MaxIntValueAttribute : ValidationAttribute
    {
        private readonly int _maxValue;

        public MaxIntValueAttribute(int maxValue)
        {
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return (int)value <= _maxValue;
        }
    }

    public class MinIntValueAttribute : ValidationAttribute
    {
        private readonly int _minValue;

        public MinIntValueAttribute(int minValue)
        {
            _minValue = minValue;
        }

        public override bool IsValid(object value)
        {
            return (int)value >= _minValue;
        }
    }
}