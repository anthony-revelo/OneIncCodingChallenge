using FluentValidation;

namespace UserManagement.Infrastructure.Validation
{
    public class MySpecificConfiguration { 
        public string MySpecificProperty1 { get; set; }
        public string MySpecificProperty2 { get; set; }
        public string MySpecificProperty3 { get; set; }
    }
    public class MySpecificConfigurationValidator : AbstractValidator<MySpecificConfiguration>
    {
        public MySpecificConfigurationValidator() { 
            RuleFor(x => x.MySpecificProperty1).NotEmpty().WithMessage("MySpecificProperty1 is required.");
            RuleFor(x => x.MySpecificProperty2).NotEmpty().WithMessage("MySpecificProperty2 is required.");
            RuleFor(x => x.MySpecificProperty3).NotEmpty().WithMessage("MySpecificProperty3 is required.");
        }
    }
}
