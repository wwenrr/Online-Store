using Microsoft.AspNetCore.Mvc;
using Training.BusinessLogic.Dtos.Admin;
using Training.BusinessLogic.Services;
namespace Training.Cms.Controllers;


public class UserController(
    IAdminService adminService
) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userPageStr = Request.Query["page"];
        var userPage = string.IsNullOrEmpty(userPageStr) ? 1 : int.Parse(userPageStr);

        var result = await adminService.GetUser(userPage);
        var data = result.GetType().GetProperty("data").GetValue(result, null);

        ViewBag.User = data;
        ViewBag.currentPage = userPage;

        return View();
    }

    public async Task<IActionResult> UserEdit(int id, AdminPatchUserReqDTO addDTO)
    {
        ViewBag.Id = id;
        ViewBag.FirstName = Request.Query["FirstName"].ToString();
        ViewBag.LastName = Request.Query["LastName"].ToString();
        ViewBag.Email = Request.Query["Email"].ToString();
        ViewBag.Role = Request.Query["Role"].ToString();

        if(addDTO.Queryable == "true") {
            await adminService.PatchUser(addDTO);
            return RedirectToAction("Index");
        }

        return View();
    }

    public async Task<IActionResult> AddUser(AdminPostUserReqDTO addDTO)
    {
        ViewBag.FirstName = Request.Query["FirstName"].ToString();
        ViewBag.LastName = Request.Query["LastName"].ToString();
        ViewBag.Email = Request.Query["Email"].ToString();
        ViewBag.Role = Request.Query["Role"].ToString();

        if(addDTO.Queryable == "true") {
            await adminService.PostUser(addDTO);
            return RedirectToAction("Index");
        }

        return View();
    }

    public async Task<IActionResult> DeleteUser(int id)
    {
        await adminService.DeleteUser(id);
        return RedirectToAction("Index");
    }
}