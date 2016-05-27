using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.WebApp.Queries.Dtos;

namespace SmartDesk.WebApp.Queries {
  public static class Functions {
    public static async Task<List<T>> FetchRecords<T>(this CloudTable table, TableQuery<T> query, TableContinuationToken token = null)
      where T : ITableEntity, new() {
      var result = new List<T>();
      do {
        var segment = await table.ExecuteQuerySegmentedAsync(query, token);
        token = segment.ContinuationToken;
        result.AddRange(segment.Results);
      } while (token != null);
      return result;
    }

    public static IEnumerable<TOut> Pairwise<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TIn, TOut> selector) {
      var previous = default(TIn);
      using (var it = source.GetEnumerator()) {
        if (it.MoveNext())
          previous = it.Current;
        while (it.MoveNext())
          yield return selector(previous, previous = it.Current);
      }
      //return source.Zip(source.Skip(1), selector);
    } 

    public static ActivityType GetActivityType(bool isActive, bool isSitting) {
      return isActive ? isSitting ? ActivityType.Sitting : ActivityType.Standing : ActivityType.Inactive;
    }

    public static bool ToBool(this long x) {
      return x != 0;
    }

  }
}