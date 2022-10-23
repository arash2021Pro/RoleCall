using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CoreBussiness.BussinessEntity.Users;
using CoreBussiness.ManagementServices;
using CoreBussiness.UnitOfWork;
using LicenseProject.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using DNTPersianUtils.Core;
using LicenseProject.Models.Licences;
using Mapster;

namespace LicenseProject.Controllers;

public class AdminController:Controller
{
    private IManagerService _managerService;
    private IUnitOfWork _work;
    private IMapper _mapper;
    public AdminController(IManagerService managerService, IUnitOfWork work, IMapper mapper)
    {
        _managerService = managerService;
        _work = work;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        return RedirectToAction("LicenseList","License");
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(AdminViewModel? model)
    {
        if (ModelState.IsValid)
        {
            var result = await _managerService.UserService.IsAdminExistsAsync(model.UserName, model.Password);
            if (result)
            {
                var user = await _managerService.UserService.GetUserAsync(model.UserName, model.Password);
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("Dashboard", "Admin");
            }
            ModelState.AddModelError(nameof(model.UserName),"کاربر پیدا نشد");
            return View(model);
        }
        ModelState.AddModelError("global","عدم اعتبار سنجی");
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login","Admin");
    }
    
    [HttpGet]
    public async Task<IActionResult> ExportDataExcl()
    {
        var dataSource = await _managerService.LicenseService.ListLicenseCodeAsync();
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Codes");
        var currentRow = 1;

        worksheet.Cell(currentRow, 1).Value = "Id";
        worksheet.Cell(currentRow, 2).Value = "Serials";
        
        foreach (var Item in dataSource)
        {
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = Item.Id;
            worksheet.Cell(currentRow, 2).Value = Item.Code;
        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        return File(content,contentType, "Codes.xlsx");
    }

    [HttpGet]
    public async Task<IActionResult> DisableLicense(int Id)
    {
        var license = await _managerService.LicenseService.GetLicenseAsync(Id);
        license!.IsActive = false;
        var saveChanges = await _work.SaveChangesAsync();
        if (saveChanges > 0)
            return RedirectToAction("LicenseList", "License");
        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> EnableLicense(int Id)
    {
        var license = await _managerService.LicenseService.GetLicenseAsync(Id);
        license!.IsActive = true;
        var saveChanges = await _work.SaveChangesAsync();
        if (saveChanges > 0)
            return RedirectToAction("LicenseList", "License");
        return BadRequest();
    }

    
   
    [HttpGet]
    public async Task<IActionResult> SetExpiration(int Id)
    {
        var license = await _managerService.LicenseService.GetLicenseAsync(Id);
        var data = license.Adapt<LicenseCalandarViewModel>();
        data.Time = DateTime.Now.ToShortPersianDateString();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> SetExpiration(LicenseCalandarViewModel model)
    { 
        var license = await _managerService.LicenseService.GetLicenseAsync(model.Id); license!.Expiration = model.Time.ToGregorianDateTime();
        var saveChanges = await _work.SaveChangesAsync();
       if (saveChanges > 0)
           return RedirectToAction("LicenseList", "License");
       return BadRequest();
    }
    

}