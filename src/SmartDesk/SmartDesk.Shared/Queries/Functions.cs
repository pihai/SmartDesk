using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.Shared.Queries.Dtos;

namespace SmartDesk.Shared.Queries {
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

    public static ActivityType GetActivityType(bool isActive, bool isStanding, bool isonline) {
      return
        isonline
          ? isActive
            ? isStanding
              ? ActivityType.Standing
              : ActivityType.Sitting
            : ActivityType.Inactive
          : ActivityType.Offline;
    }

    public static bool ToBool(this long x) {
      return x != 0;
    }

    public static DateTime FirstDateOfWeek(int year, int weekOfYear) {
      DateTime jan1 = new DateTime(year, 1, 1);
      int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

      DateTime firstThursday = jan1.AddDays(daysOffset);
      var cal = CultureInfo.CurrentCulture.Calendar;
      int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

      var weekNum = weekOfYear;
      if (firstWeek <= 1) {
        weekNum -= 1;
      }
      var result = firstThursday.AddDays(weekNum * 7);
      return result.AddDays(-3);
    }

  }
}