using FluentValidation;
using Microsoft.Extensions.Options;

namespace UserManagement.Api.Extensions
{
    public static class OptionsBuilderExtension
    {
        public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder)
            where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider =>
            {
                var validator = serviceProvider.GetRequiredService<IValidator<TOptions>>();
                return new FluentValidationOptions<TOptions>(validator);
            });

            return optionsBuilder;
        }
    }

    public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly IValidator<TOptions> _validator;

        public FluentValidationOptions(IValidator<TOptions> validator)
        {
            _validator = validator;
        }

        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            var result = _validator.Validate(options);
            if (result.IsValid)
            {
                return ValidateOptionsResult.Success;
            }

            var errors = result.Errors.Select(e => $"Property {e.PropertyName} failed validation. Error was: {e.ErrorMessage}");
            return ValidateOptionsResult.Fail(errors);
        }
    }
}
