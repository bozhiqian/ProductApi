using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCloud.Product.Api.Data
{
    public class AddProductResult
    {
        public AddProductResult(ValidationResult validationResult, bool isDuplicate)
        {
            ValidationResult = validationResult;
            IsDuplicate = isDuplicate;
        }

        public bool IsSuccess => IsValid && !IsDuplicate;
        public ValidationResult ValidationResult { get; }
        public bool IsValid => ValidationResult.IsValid;
        public bool IsDuplicate { get; }
    }
}
