public class MaintenanceTicketViewModel
{
    public string assignedWorker {get;set;}
    public int priorityLevel {get;set;}
    public string maintenanceType {get;set;}
    public DateOnly plannedCompletion {get;set;}
    public string partsList {get;set;}
    public string description {get;set;}
}