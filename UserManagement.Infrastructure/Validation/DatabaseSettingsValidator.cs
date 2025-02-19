using FluentValidation;

namespace UserManagement.Infrastructure.Validation
{
    public class DatabaseSettings
    {
        public required string DefaultConnection { get; set; }
    }

    public class DatabaseSettingsValidator : AbstractValidator<DatabaseSettings>
    {
        public DatabaseSettingsValidator()
        {
            RuleFor(x => x.DefaultConnection).NotEmpty().WithMessage("Connection string is required.");
        }
    }
}
