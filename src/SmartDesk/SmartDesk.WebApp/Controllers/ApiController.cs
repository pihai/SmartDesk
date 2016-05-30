using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartDesk.WebApp.Queries;
using SmartDesk.WebApp.Queries.Dtos;

namespace SmartDesk.WebApp.Controllers {
  [Produces("application/json")]
  [Route("api")]
  public class ApiController : Controller {
    private readonly DayHistoryQuery _dayHistoryQuery;
    private readonly DayRatioQuery _dayRatioQuery;
    private readonly TotalStatsQuery _totalRatioQuery;
    public ApiController(DayHistoryQuery dayHistoryQuery, DayRatioQuery dayRatioQuery, TotalStatsQuery totalRatioQuery) {
      _dayHistoryQuery = dayHistoryQuery;
      _dayRatioQuery = dayRatioQuery;
      _totalRatioQuery = totalRatioQuery;
    }

    [HttpGet("{deviceId}/{year}/{month}/{day}/history")]
    public async Task<List<DayHistoryEntry>> GetDayHistory(string deviceId, int year, int month, int day) {
      return await _dayHistoryQuery.Query(deviceId, new DateTime(year, month, day));
    }

    [HttpGet("{deviceId}/{year}/{month}/{day}/ratio")]
    public async Task<DayRatio> GetDayRatio(string deviceId, int year, int month, int day) {
      return await _dayRatioQuery.Query(deviceId, new DateTime(year, month, day));
    }
    [HttpGet("{deviceId}/total")]
    public async Task<TotalStats> GetTotalStats(string deviceId) {
      return await _totalRatioQuery.Query(deviceId);
    }
    [HttpGet("{deviceId}/{year}/{month}/{day}/duration")]
    public double GetDurationOfDay(string deviceId, int year, int month, int day) {
      return 42.0;
    }
    [HttpGet("{deviceId}/{year}/{week}/ratio")]
    public async Task<List<DayRatio>> GetWeekRatio(string deviceId, int year, int week) {
      //TODO extract in separate query
      DateTime start = Functions.FirstDateOfWeek(year, week);
      var tasks =
        Enumerable.Range(0, 7)
          .Select(d => start.AddDays(d))
          .Select(d => _dayRatioQuery.Query(deviceId, d));
      return (await Task.WhenAll(tasks)).ToList();
    }
  }
}
