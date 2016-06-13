using SmartDesk.Shared.Queries.Dtos;

namespace SmartDesk.WebApp.Models {
  public class CurrentStatusDTO {
    public bool Active { get; set; }
    public bool Online { get; set; }
    public double Height { get; set; }
    public string ActivityType { get; set; }

    public CurrentStatusDTO(CurrentStatus x) {
      Active = x.Active;
      Online = x.Online;
      ActivityType = x.ActivityType.ToString();
      Height = x.Height;
    }
  }
}