using FluentValidation;
using REST.Models.Entities;

namespace REST.Validators;

public class TagValidator : AbstractValidator<Tag>
{
    public TagValidator()
    {
        RuleFor(editor => editor.Name).NotEmpty().MinimumLength(2).MaximumLength(32);
    }
}