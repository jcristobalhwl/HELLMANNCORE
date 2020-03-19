using FluentValidation;
using Model.Request.Manifest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators.Manifest
{
    public class ManifestValidator : AbstractValidator<ManifestRequest>
    {
        public ManifestValidator()
        {
            //RuleFor(manifest => manifest.DAT_STARTDATE).NotNull().WithMessage("Debe elegir la Fecha de Inicio");
            //RuleFor(manifest => manifest.DAT_ENDDATE).NotNull().WithMessage("Debe elegir la Fecha Hasta");
        }
    }
}
