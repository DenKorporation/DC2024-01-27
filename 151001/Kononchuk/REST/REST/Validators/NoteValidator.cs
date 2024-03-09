using FluentValidation;
using REST.Models.Entities;

namespace REST.Validators;

public class NoteValidator : AbstractValidator<Note>
{
    public NoteValidator()
    {
        RuleFor(editor => editor.Content).MinimumLength(2).MaximumLength(2048);
    }
}