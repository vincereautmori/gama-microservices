﻿using Gama.Application.Contracts.Mappers;
using Gama.Application.UseCases.TrafficFineAgg.Commands;
using Gama.Application.UseCases.TrafficFineAgg.Interfaces;
using Gama.Application.UseCases.TrafficFineAgg.Responses;
using Gama.Application.UseCases.UserAgg.Interfaces;
using Gama.Domain.Entities.UsersAgg;
using Gama.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Gama.Domain.Entities.TrafficFinesAgg;

namespace Gama.Api.Controllers;

[ApiController]
[Route("v1/traffic-violations")]
public class TrafficViolationController : Controller
{
    private readonly ITrafficViolationService _trafficViolationService;
    private readonly IEntityMapper _entityMapper;

    public TrafficViolationController(
        ITrafficViolationService trafficViolationService, 
        IEntityMapper entityMapper,
        ICurrentUserAccessor currentUserAccessor
        )
    {
        _trafficViolationService = trafficViolationService;
        _entityMapper = entityMapper;
    }

    [Authorize(Roles = RolesName.Admin)]
    [HttpGet("{id:int}", Name = "GetTrafficViolation")]
    [ProducesResponseType(typeof(GetTrafficViolationResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _trafficViolationService.GetAsync(id);

        return result.ToOk((trafficViolation) => _entityMapper.Map<GetTrafficViolationResponse, TrafficViolation>(trafficViolation));
    }

    [Authorize]
    [HttpGet("", Name = "GetTrafficsViolations")]
    [ProducesResponseType(typeof(IEnumerable<GetTrafficViolationResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTrafficsViolations()
    {
        var result = await _trafficViolationService.GetTrafficsViolationsAsync();

        return result.ToOk((trafficViolation) => _entityMapper.Map<IEnumerable<GetTrafficViolationResponse>, IEnumerable<TrafficViolation>>(trafficViolation));
    }

    [HttpPost]
    [Authorize(Roles = RolesName.Admin)]
    [ProducesResponseType(typeof(GetTrafficViolationResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateTrafficViolationCommand createTrafficViolationCommand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var trafficViolation = _entityMapper.Map<TrafficViolation, CreateTrafficViolationCommand>(createTrafficViolationCommand);

        var result = await _trafficViolationService.CreateAsync(trafficViolation);

        return result.ToOk((trafficViolation) => _entityMapper.Map<GetTrafficViolationResponse, TrafficViolation>(trafficViolation));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = RolesName.Admin)]
    [ProducesResponseType(typeof(GetTrafficViolationResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTrafficViolationCommand updateTrafficViolationCommand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var trafficViolation = _entityMapper.Map<TrafficViolation, UpdateTrafficViolationCommand>(updateTrafficViolationCommand);

        trafficViolation.Id = id;

        var result = await _trafficViolationService.UpdateAsync(trafficViolation);

        return result.ToOk((result) => _entityMapper.Map<GetTrafficViolationResponse, TrafficViolation>(result));
    }

    [Authorize(Roles = RolesName.Admin)]
    [HttpDelete("{id:int}", Name = "DeleteTrafficViolation")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _trafficViolationService.DeleteAsync(id);
        return result.ToNoContent();
    }
}