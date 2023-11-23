#region

using NotificationService.Models.Pagination;

#endregion

namespace NotificationService.Models.QueryParameters.GetAll;

public class GetAllEmailsRequestQueryParameters
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
    public EQueryNotificationStatus? Status { get; set; } = null;
}