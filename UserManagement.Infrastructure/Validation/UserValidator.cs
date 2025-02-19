using FluentValidation;
using UserManagement.Core.Entities;

namespace UserManagement.Infrastructure.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty().MaximumLength(128);
            RuleFor(u => u.LastName).MaximumLength(128);
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.DateOfBirth).NotEmpty().Must(BeAtLeast18YearsOld);
            RuleFor(u => u.PhoneNumber).NotEmpty().Matches(@"^\d{10}$");
        }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;
            return age >= 18;
        }
    }
}
