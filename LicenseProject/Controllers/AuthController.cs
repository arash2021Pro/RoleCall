using System.Text;
using CoreBussiness.ManagementServices;
using CoreBussiness.UnitOfWork;
using LicenseProject.LicenseService;
using LicenseProject.Models.Auth;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace LicenseProject.Controllers;

public class AuthController:Controller
{
    private IUnitOfWork _work;
    private IManagerService _managerService;
    private IMapper _mapper;
    public AuthController(IUnitOfWork work, IManagerService managerService, IMapper mapper)
    {
        _work = work;
        _managerService = managerService;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> VerifyPhone(string?PhoneNumber)
    {
        var verifyModel = new VerifyPhoneViewModel() {PhoneNumber = PhoneNumber};
        return View(verifyModel);
    }
    [HttpPost]
    public async Task<IActionResult> VerifyPhone(VerifyPhoneViewModel? model)
    {
        if (ModelState.IsValid)
        {
            var otp = await _managerService.OtpService.GetOtpAsync(model!.Code);
            if (otp == null)
            {
                ModelState.AddModelError(nameof(model.Code),"اطاعاتی پیدا نشد");
                return View(model);
            }
            var license = await _managerService.LicenseService.GetLicenseAsync(otp.LicenseId);
            var client = await _managerService.ClientService.CatchClientAsync(license.Id);
            if (otp.IsAuthentic)
            {
                otp.IsUsed = true;
                license.IsOtpConfirmed = true;
                var saveChanges = await _work.SaveChangesAsync();
                if (saveChanges > 0)
                {
                    var Content = LicenseConvertor.ConvertLicense(license,client);
                    return File(Content, "application/xml", "License.Key.xml");
                }
                ModelState.AddModelError(nameof(model.Code),"خطا در پاسخگویی");
                return View(model);
            }
            ModelState.AddModelError(nameof(model.Code),"کد اعتبار ندارد");
            return View(model);
        }
        return View(model);
    }
    
}