using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class CountryValidator : AbstractValidator<TBL_SLI_COUNTRY>
    {
        public CountryValidator()
        {
            RuleFor(country => country.VCH_NAME).NotEmpty().WithMessage("Debe ingresar el nombre del País");
            RuleFor(country => country.INT_CONTINENTID).NotEmpty().WithMessage("Debe elegir el continente del País");
            RuleFor(country => country.INT_CURRENCYID).NotEmpty().WithMessage("Debe elegir el tipo de moneda del País");
        }
    }
}
