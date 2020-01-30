using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class CurrencyValidator : AbstractValidator<TBL_SLI_CURRENCY>
    {
        public CurrencyValidator()
        {
            RuleFor(currency => currency.VCH_NAME).NotEmpty().WithMessage("Debe ingresar el nombre de la Moneda");
            RuleFor(currency => currency.VCH_SYMBOL).NotEmpty().WithMessage("Debe ingresar el código de la Moneda ");
        }
    }
}
