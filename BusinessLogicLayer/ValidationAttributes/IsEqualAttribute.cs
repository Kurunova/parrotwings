using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BusinessLogicLayer.ValidationAttributes
{
    //[AttributeUsage(AttributeTargets.Property)]
    public class IsEqualAttribute : ValidationAttribute
    {
        private readonly string[] _properties;

        public IsEqualAttribute(params string[] properties)
        {
            _properties = properties;
        }

        public string[] Properties => _properties;

        protected override ValidationResult IsValid(object item, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;
            
            if (item != null && item is string currentProperty)
            {
                foreach (var otherProperty in _properties)
                {
                    var property = validationContext.ObjectType.GetProperty(otherProperty);
                    if (property == null)
                    {
                        return new ValidationResult($"Unknown property: {otherProperty}");
                    }
                    var otherValue = property.GetValue(validationContext.ObjectInstance, null);
                    
                    if (otherValue is string otherValueStr
                        && otherValueStr != currentProperty)
                    {
                        var displayName = property.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name;
                        result = new ValidationResult($"The field '{validationContext.DisplayName}' and '{displayName}' should be the same");
                        break;
                    }
                }
            }

            return result;
        }
    }
}