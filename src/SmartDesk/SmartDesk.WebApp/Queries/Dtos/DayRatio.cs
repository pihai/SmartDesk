namespace SmartDesk.WebApp.Queries.Dtos {
  public class DayRatio {
    public double Standing { get; set; }
    public double Sitting { get; set; }
    public double Inactive { get; set; }

    public DayRatio(double standing, double sitting, double inactive) {
      Standing = standing;
      Sitting = sitting;
      Inactive = inactive;
    }
  }
}