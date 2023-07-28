﻿using System.Linq.Expressions;
using Gama.Domain.Interfaces.Repositories;

namespace Gama.Domain.Entities.OccurrencesAgg
{
    public interface IOccurrenceRepository : IRepository<Occurrence>
    {
        Task<IEnumerable<Occurrence>> GetAsync(Expression<Func<Occurrence, bool>> search, int offset, int size);
    }
}
