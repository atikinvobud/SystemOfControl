using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace BackEnd.Share;

public static class OrderCodeService
{
    public static Guid GetMessage(OrderStatuses? code) => code switch
    {
        OrderStatuses.InWork => Guid.Parse("9756cd25-f3b3-4e49-a3aa-b20e8072593e"),
        OrderStatuses.Waiting => Guid.Parse("b6407f50-ef6b-409d-8694-b7057c37da03"),
        OrderStatuses.WaitingApprove => Guid.Parse("821e9bd3-ebd1-44ab-b473-dec2a28b9038"),
        OrderStatuses.Ended => Guid.Parse("5f495a98-ed07-4f46-ada7-a96a9b8ee1dd"),
        OrderStatuses.Created => Guid.Parse("d12c497b-00ab-4a73-865f-cfe57db01aae"),
        _ => Guid.Parse("d12c497b-00ab-4a73-865f-cfe57db01aae")
    };
}