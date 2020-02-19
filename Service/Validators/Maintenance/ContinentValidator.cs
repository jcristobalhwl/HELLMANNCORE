using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class ContinentValidator : AbstractValidator<TBL_SLI_CONTINENT>
    {
        public ContinentValidator()
        {
            RuleFor(country => country.VCH_NAME).NotEmpty().WithMessage("Debes ingresar el nombre del Continente");
        }
    }
}
