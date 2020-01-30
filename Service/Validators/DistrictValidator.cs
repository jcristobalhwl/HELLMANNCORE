using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class DistrictValidator : AbstractValidator<TBL_SLI_DISTRICT>
    {
        public DistrictValidator()
        {
            RuleFor(city => city.VCH_NAME).NotEmpty().WithMessage("Debe ingresar el nombre del Distrito");
            RuleFor(city => city.INT_PROVINCEID).NotEmpty().WithMessage("Debe elegir la Provincia al cual pertenece el Distrito");
        }
    }
}
