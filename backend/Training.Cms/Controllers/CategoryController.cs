using Microsoft.AspNetCore.Mvc;
using Training.BusinessLogic.Services;
using Training.Common.Helpers;
using System.IO;
using Training.BusinessLogic.Dtos.Admin;
using Training.Cms.Models;
using AutoMapper;

namespace Training.Cms.Controllers;

public class CategoryController(
    IAdminService adminService,
    IMapper mapper
) : Controller
{
    public async Task<IActionResult> Index()
    {
        var categoryPageStr = Request.Query["page"];
        var page = string.IsNullOrEmpty(categoryPageStr) ? 1 : int.Parse(categoryPageStr);

        var result = await adminService.GetCategory(page);
        var data = result.GetType().GetProperty("data").GetValue(result, null);
        
        ViewBag.Category = data;
        ViewBag.currentPage = page;

        // CommonHelper.Print(data);

        return View();
    }

    public async Task<IActionResult> EditCategory(int id)
    {
        return View();
    }

    public async Task<IActionResult> AddCategory() {
        return View();
    }

    public async Task<IActionResult> AddCategoryAction(PostCategoryReqModel reqModel)
    {
        CommonHelper.Print(reqModel);

        var fileName = await FileHelper.SaveImage(reqModel.Image);
        reqModel.FileName = fileName;

        try {
            var reqDTO = mapper.Map<AdminPostCategoryReqDTO>(reqModel);
            await adminService.PostCategory(reqDTO);
        } catch (Exception ex) {
            FileHelper.DeleteImage(fileName);
            throw ex;
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteCategoryAction(int id)
    {
        try {
            var cate = await adminService.DeleteCategory(id);
        } catch (Exception ex) {
            
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RestoreCategoryAction(int id)
    {
        await adminService.RestoreCategory(id);
        return RedirectToAction("Index");
    }
}