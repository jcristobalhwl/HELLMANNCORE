using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class CityValidator : AbstractValidator<TBL_SLI_CITY>
    {
        public CityValidator()
        {
            RuleFor(city => city.VCH_NAME).NotEmpty().WithMessage("Debe ingresar el nombre de la Ciudad");
            RuleFor(city => city.INT_COUNTRYID).NotEmpty().WithMessage("Debe elegir el País al cual pertenece la Ciudad");
        }
    }
}
