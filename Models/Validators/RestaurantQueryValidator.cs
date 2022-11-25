using FluentValidation;
using Microsoft.EntityFrameworkCore;
using _ApiProject1_.Entities;

namespace _ApiProject1_.Models.Validators
{
    public class RestaurantQueryValidator:AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new[] { 2, 5, 8 };
        private string[] allowedOrderBy = new[] { nameof(Restaurant.Name), nameof(Restaurant.Description), nameof(Restaurant.Category) };
        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in[{string.Join(", ", allowedPageSizes)}]");
                }
            });
            RuleFor(r => r.OrberBy).Custom((value, context) =>
            {
                if (!allowedOrderBy.Contains(value))
                {
                    context.AddFailure("OrberBy", $"OrberBy must be one of[{string.Join(", ", allowedOrderBy)}]");
                }
            });
        }
    }
}
