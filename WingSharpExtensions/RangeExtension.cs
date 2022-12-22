namespace WingSharpExtensions;

using System;
using System.Collections.Generic;

public static class RangeExtension
{
    public static IEnumerator<int> GetEnumerator(this Range range)
    {
        var (start, end) = (range.Start.Value, range.End.Value);
        if (start < end)
        {
            for (var i = start; i < end; i++)
            {
                yield return i;
            }
        }
        else if (start > end)
        {
            for (var i = start; i > end; i++)
            {
                yield return i;
            }
        }
    }
}
