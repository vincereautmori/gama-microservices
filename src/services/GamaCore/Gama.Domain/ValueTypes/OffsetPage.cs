﻿namespace Gama.Domain.ValueTypes;

public class OffsetPage<T>
{
    public int PageNumber { get; set; }

    public int Size { get; set; }

    public int Count { get; set; }

    public int Offset => (PageNumber - 1) * Size;

    public IEnumerable<T> Results { get; set; }    
}