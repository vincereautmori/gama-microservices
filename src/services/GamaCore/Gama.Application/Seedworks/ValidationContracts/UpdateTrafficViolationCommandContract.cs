﻿using Flunt.Validations;
using Gama.Application.UseCases.TrafficFineAgg.Commands;

namespace Gama.Application.Seedworks.ValidationContracts
{
    internal class UpdateTrafficViolationCommandContract : Contract<UpdateTrafficViolationCommand>
    {
        public UpdateTrafficViolationCommandContract(UpdateTrafficViolationCommand updateTrafficViolationCommand)
        {
            IsNotNullOrEmpty(updateTrafficViolationCommand.Name, nameof(updateTrafficViolationCommand.Name), "Você deve informar um nome.");
            IsNotNullOrEmpty(updateTrafficViolationCommand.Code, nameof(updateTrafficViolationCommand.Code), "Você deve informar um código.");
        }
    }
}
