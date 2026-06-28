using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using InterviewExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace InterviewExercise.Controllers;

public class HomeController(IWebHostEnvironment env, IItemCheckService itemCheckService) : Controller
{
    readonly IItemCheckService _itemCheckService = itemCheckService;
    readonly IWebHostEnvironment _env = env;

    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> CheckTheChanges([FromForm] string jsonModel)
    {
        if (!ModelState.IsValid)
        {
            // Returns the view back with error messages if validation fails
            return View("~/Views/Home/Index.cshtml", jsonModel);
        }

        UriModel model;
        try
        {
            model = JsonSerializer.Deserialize<UriModel>(jsonModel);
        }
        catch (JsonException)
        {
            return BadRequest("Invalid JSON data format.");
        }

        string filePath = model?.Uri ?? "";

        

        if (!Directory.Exists(filePath))
            return NotFound("Folder not found.");

        var rootPath = _env.ContentRootPath;

        var result = await _itemCheckService.CheckTheChanges(filePath, rootPath);


        return PartialView("_ItemListPartial", result);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
