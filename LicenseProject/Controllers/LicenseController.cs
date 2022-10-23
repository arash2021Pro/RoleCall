using CoreBussiness.BussinessEntity.Clients;
using CoreBussiness.BussinessEntity.Licenses;
using CoreBussiness.BussinessEntity.OTP;
using CoreBussiness.BussinessEntity.Serials;
using CoreBussiness.ManagementServices;
using CoreBussiness.UnitOfWork;
using DNTPersianUtils.Core;
using LicenseProject.Messaging;
using LicenseProject.Messaging.GhasedakProvider;
using LicenseProject.Messaging.KaveNegarProvider;
using LicenseProject.Models.Licences;
using LicenseProject.StartupModuleServices.CodeGenerators;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseProject.Controllers;

public class LicenseController:Controller
{
    private IUnitOfWork _work;
    private IManagerService _managerService;
    private ICodeGeneratorService _codeGeneratorService;
    private IMapper _mapper;
    public LicenseController(IUnitOfWork work, IManagerService managerService, ICodeGeneratorService codeGeneratorService, IMapper mapper)
    {
        _work = work;
        _managerService = managerService;
        _codeGeneratorService = codeGeneratorService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> RegisterLicense()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterLicense(LicenseViewModel? model)
    {
        if (ModelState.IsValid)
        {
            for (int x = 1; x <= model.AppSerialCount; x++)
            {
                var license = new License();
                license.IsActive = true;
                license.IsSmsPanelActive =  model.IsSmsPanelActive;
                license.IsMobileVersionActive = model.IsMobileVersionActive;
                _mapper.Map(model, license);
                license.LicenseCode = _codeGeneratorService.GenerateLicenseCode(1,8);
                await _managerService.LicenseService.AddNewLicenseAsync(license);
                for (int i = 1; i <= model.ClientCount; i++)
                {
                    var client = new Client();
                    client.License = license;
                    client.AppSerial = license.LicenseCode;
                    await _managerService.ClientService.AddNewClientAsync(client);
                }
            }
            var saveChanges = await _work.SaveChangesAsync();
            if (saveChanges > 0)
            {
                return RedirectToAction("LicenseList");    
            }
        }
        return View(model);
    }

    
    
    
    [HttpGet]
    public async Task<IActionResult> CompleteLicense(int id)
    {
        var license = await _managerService.LicenseService.GetLicenseAsync(id);
        var data = license.Adapt<OrderLicenseViewModel>();
        return View(data);
    }
    

    [HttpPost]
    public async Task<IActionResult> CompleteLicense(OrderLicenseViewModel model)
    {
        if (ModelState.IsValid)
        {
             var lisense = await _managerService.LicenseService.GetLicenseAsync(model.Id);
             _mapper.Map(model, lisense);
            var otp = new Otp() {Code = _codeGeneratorService.GenerateOtpCode(4), LicenseId = lisense.Id};
            await _managerService.OtpService.AddNewOtpAsync(otp);
            var saveChanges = await _work.SaveChangesAsync();
            if (saveChanges > 0)
            {
              //  var smsManager = new GhasedakSmsManager(new GhasedakOptions());
              //  await smsManager.SendOtpMessage("کد تایید شما", lisense.PhoneNumber!,new List<string>(){otp.Code});
       
                return RedirectToAction("VerifyPhone", "Auth",new {PhoneNumber=lisense.PhoneNumber});
            }
        }
        return View(model);
    }
    
    [Authorize]
    public async Task<IActionResult> LicenseList()
    {
        return View();
    }
    public async Task<IActionResult> LoadLicense(int start,int length)
    {
        var queryParameter = "search[value]";
        var search = Request.Query[queryParameter].ToString();
        var licenseData = await _managerService.LicenseService.ListLicenseDataAsync(start, length, search);
        
        var recordsTotal = await _managerService.LicenseService.GetLicenseCountAsync(search);
        var recordsFiltered = recordsTotal;
        if (!String.IsNullOrEmpty(search))
        {
            recordsFiltered = await _managerService.LicenseService.GetLicenseCountAsync(search);
        }
        var json = Json(new {data = licenseData, recordsTotal, recordsFiltered = recordsTotal});
        return json;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ModifyLicense(int id)
    {
        var license = await _managerService.LicenseService.GetLicenseAsync(id);
        var data = license!.Adapt<ModifyLicenseViewModel>();
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> ModifyLicense(ModifyLicenseViewModel model)
    {
        if (ModelState.IsValid)
        {
            var license = await _managerService.LicenseService.GetLicenseAsync(model.Id);
            license.ModificationTime = DateTime.Now.ToShortPersianDateTimeString();
            license.DateTimeModification = DateTime.Now;
            _mapper.Map(model, license);
            var saveChanges = await _work.SaveChangesAsync();
            if (saveChanges > 0)
            {
                return RedirectToAction("LicenseList","License");
            }
            return Problem();
        }
        return View(model);
    }


    






}