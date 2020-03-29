using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quest.API.Services
{
    public class ValidationService
    {
        public class ShouldNotInclude : ValidationAttribute
        {
            private string ProhibitedSequence { get; }

            public ShouldNotInclude(string prohibitedSequence)
            {
                ProhibitedSequence = prohibitedSequence;
            }

            public override bool IsValid(object value)
            {
                var strValue = value as string;
                if (string.IsNullOrEmpty(strValue)) return true;
                return !strValue.Contains(this.ProhibitedSequence);
            }
        }
    }
}