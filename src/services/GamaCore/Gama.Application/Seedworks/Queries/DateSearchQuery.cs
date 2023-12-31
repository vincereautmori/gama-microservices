﻿using Gama.Application.Seedworks.ValidationContracts;

namespace Gama.Application.Seedworks.Queries;

public class DateSearchQuery : PagedSearchQuery
{
    public DateTime CreatedSince { get; set; }

    public DateTime CreatedUntil { get; set; }

    public override void Validate()
    {
        base.Validate();
        AddNotifications(new PagedSearchQueryContract(this));
    }
}