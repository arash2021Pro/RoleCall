using System.Management;
using CoreBussiness.BussinessEntity.Licenses;
using CoreBussiness.BussinessEntity.OTP;
using CoreBussiness.ManagementServices;
using CoreBussiness.UnitOfWork;
using LicenseProject.Messaging.GhasedakProvider;
using LicenseProject.Messaging.IpPanelProvider;
using LicenseProject.Messaging.KaveNegarProvider;
using LicenseProject.Models.Client;
using LicenseProject.StartupModuleServices.CodeGenerators;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseProject.Controllers;

public class ClientController:Controller
{
    private IUnitOfWork _work;
    private IManagerService _managerService;
    private IMapper _mapper;
    private ICodeGeneratorService _codeGeneratorService;
    public ClientController(IUnitOfWork work, IManagerService managerService, IMapper mapper, ICodeGeneratorService codeGeneratorService)
    {
        _work = work;
        _managerService = managerService;
        _mapper = mapper;
        _codeGeneratorService = codeGeneratorService;
    }
    
    [HttpGet]
    public async Task<IActionResult> ValidateClient()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ValidateClient(ValidateClientViewModel model)
    {
        if (ModelState.IsValid)
        {
            var license = await _managerService.LicenseService.GetLicenseAsync(model.AppSerial);
            if (license == null)
            {
                ModelState.AddModelError(nameof(model.AppSerial),"سریال پیدا نشد");
                return View(model);
            }
            if (license!.CompanyAddress == null && license.CompanyName == null && license.ConstPhone == null &&
                license.LastName == null && license.Name == null && license.LegalCode == null &&
                license.PhoneNumber == null)
            {
                return RedirectToAction("CompleteLicense", "License",new{Id=license.Id});
            }
            var otp = new Otp() {Code = _codeGeneratorService.GenerateOtpCode(5), LicenseId = license.Id};
            await _managerService.OtpService.AddNewOtpAsync(otp);
            var saveChange = await _work.SaveChangesAsync();
            if (saveChange > 0)
            { 
          
               var smsManager = new IpPanelSmsManager(new IpPanelOptions());
               await smsManager.SendOtpMessage("zim8kjfxnfxpI7y",license.PhoneNumber,new List<string>{"verification-code",otp.Code});
               
               
              return RedirectToAction("VerifyPhone", "Auth",new {PhoneNumber=license.PhoneNumber});
            }
            ModelState.AddModelError(nameof(license.PhoneNumber),"خطا در پاسخگویی");
            return View(model);
        }
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> ClientList()
    {
        return View();
    }

    public async Task<IActionResult> LoadClients(int id,int start,int length)
    {
        var queryParameter = "search[value]";
        var search = Request.Query[queryParameter].ToString();
        var clientsData = await _managerService.ClientService.ListClientDataAsync(id,start,length,search);
        var recordsTotal = await _managerService.ClientService.GetClientCountAsync(search);
        var recordsFiltered = recordsTotal;
        if (!String.IsNullOrEmpty(search))
        {
            recordsFiltered = await _managerService.ClientService.GetClientCountAsync(search);
        }
        var json = Json(new {data = clientsData, recordsTotal, recordsFiltered = recordsTotal});
        return json;
    }




}