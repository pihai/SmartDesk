namespace SmartDesk.WebApp.Queries.Dtos {
  public class TotalStats {
    public double Active { get; set; }
    public double Sitting { get; set; }
    public double Standing { get; set; }

    public TotalStats(double active, double standing, double sitting) {
      Active = active;
      Standing = standing;
      Sitting = sitting;
    
    }
  }
}