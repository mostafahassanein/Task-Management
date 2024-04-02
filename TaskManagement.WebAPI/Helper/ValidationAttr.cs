using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebAPI.Helper
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class NumberMaxLengthAttribute : ValidationAttribute
    {
        private int MaxLength { get; set; }
        public NumberMaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return value.ToString().Length <= MaxLength;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format("The field {0} Max Length is {1}. \r", name, MaxLength);
        }
    }
    sealed public class ValidateInetger : ValidationAttribute
    {
        private int number;

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                try
                {
                    number = int.Parse(value.ToString());
                    if (number >= 0)
                        return true;
                    else
                        return false;
                }
                catch { return false; }
            }
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format("The field {0} accepts positive integers only.", name);
        }
    }
    
}
