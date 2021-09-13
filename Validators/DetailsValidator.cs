using FluentValidation;
using PracticeConcept.API.Models;
using PracticeConcept.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeConcept.API.Validators
{
    public class DetailsValidator : AbstractValidator<details>
    {
        public DetailsValidator()
        {
            RuleFor(x => x.Id)
                //Stop on first failure
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                //Stop on first failure
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty().WithMessage(Constants.NameIsEmpty)
                .Length(2, 50).WithMessage(Constants.NameExceedLimit)
                .Must(BeAValidName).WithMessage(Constants.NameIsInvalid);

            RuleFor(x => x.Address)
                //Stop on first failure
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty().Length(2, 200);
        }

        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "").Replace("-", "");
            return name.All(char.IsLetter);
        }
    }
}
