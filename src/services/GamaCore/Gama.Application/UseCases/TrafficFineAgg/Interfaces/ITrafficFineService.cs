﻿using Gama.Application.UseCases.TrafficFineAgg.Queries;
using Gama.Domain.Entities.TrafficFinesAgg;
using Gama.Domain.ValueTypes;

namespace Gama.Application.UseCases.TrafficFineAgg.Interfaces;

public interface ITrafficFineService
{
    Task<Result<TrafficFine>> GetAsync(int id);
    Task<Result<OffsetPage<TrafficFine>>> GetByDateSearchAsync(TrafficFineListingQuery dateSearchQuery);
    Task<Result<TrafficFine>> CreateAsync(TrafficFine trafficFine);
    Task<Result<bool>> ComputeAsync(int id);
    Task<Result<bool>> DeleteAsync(int id);
}