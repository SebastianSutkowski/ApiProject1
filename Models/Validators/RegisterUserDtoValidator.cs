using FluentValidation;
using _ApiProject1_.Entities;
namespace _ApiProject1_.Models.Validators
{
    public class RegisterUserDtoValidator:AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .MinimumLength(6);
            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);
            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailExists = dbContext.Users.Any(u => u.Email == value);
                    if (emailExists)
                    {
                        context.AddFailure("Email", "That email is in use");
                    }
                });
        }
    }
}
