using Microsoft.AspNetCore.Mvc;
using Training.BusinessLogic.Services;
using Training.Common.Helpers;
namespace Training.Cms.Controllers;

public class ReportController(
    IAdminService adminService
) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> LowStockAsync()
    {
        var pageStr = Request.Query["page"];
        var page = string.IsNullOrEmpty(pageStr) ? 1 : int.Parse(pageStr);
        
        var res = await adminService.GetLowStockProducts(page);
        var data = res.GetType().GetProperty("data").GetValue(res, null);

        ViewBag.LowStockProducts = data;
        ViewBag.page = page;

        return View();
    }

    public async Task<IActionResult> TodayOrders()
    {
        var res = await adminService.GetTodayOrders();
        var data = res.GetType().GetProperty("data").GetValue(res, null);

        ViewBag.TodayOrders = data;

        return View();
    }

    public async Task<IActionResult> HighestOrders()
    {
        var pageStr = Request.Query["page"];
        var page = string.IsNullOrEmpty(pageStr) ? 1 : int.Parse(pageStr);

        var res = await adminService.GetHighestOrders(page);
        var data = res.GetType().GetProperty("data").GetValue(res, null);

        ViewBag.HighestOrders = data;

        CommonHelper.Print(data);

        return View();
    }
}
