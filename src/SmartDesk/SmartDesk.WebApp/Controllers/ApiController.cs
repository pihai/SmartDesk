using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartDesk.Shared.Queries;
using SmartDesk.Shared.Queries.Dtos;
using SmartDesk.WebApp.Models;

namespace SmartDesk.WebApp.Controllers {
  [Produces("application/json")]
  [Route("api")]
  public class ApiController : Controller {
    private readonly DayHistoryQuery _dayHistoryQuery;
    private readonly DayRatioQuery _dayRatioQuery;
    private readonly TotalStatsQuery _totalRatioQuery;
    private readonly CurrentStatusQuery _currentStatusQuery;

    public ApiController(DayHistoryQuery dayHistoryQuery, DayRatioQuery dayRatioQuery, TotalStatsQuery totalRatioQuery, CurrentStatusQuery currentStatusQuery) {
      _dayHistoryQuery = dayHistoryQuery;
      _dayRatioQuery = dayRatioQuery;
      _totalRatioQuery = totalRatioQuery;
      _currentStatusQuery = currentStatusQuery;
    }

    [HttpGet("{deviceId}/{year}/{month}/{day}/history")]
    public async Task<List<DayHistoryEntryDTO>> GetDayHistory(string deviceId, int year, int month, int day) {
      var result = await _dayHistoryQuery.Query(deviceId, new DateTime(year, month, day));
      var dtos = result.Select(x => new DayHistoryEntryDTO(x)).ToList();
      return dtos;
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
    public async Task<double> GetDurationOfDay(string deviceId, int year, int month, int day) {
      return (await _dayRatioQuery.Query(deviceId, new DateTime(year, month, day))).Standing;
    }

    [HttpGet("{deviceId}/status")]
    public async Task<CurrentStatusDTO> GetCurrentStatus(string deviceId) {
      var result = await _currentStatusQuery.Query(deviceId);
      return new CurrentStatusDTO(result);
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
