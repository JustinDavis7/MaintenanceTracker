using ProjectMaintenance.Models;
public class MaintenanceIndexViewModel
{
    public IEnumerable<MaintenanceTicket> Tickets { get; set; }
    public IEnumerable<Equipment> Equipment { get; set; }
}
