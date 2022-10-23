using CoreBussiness.BussinessEntity.OTP;
using CoreBussiness.ManagementServices;
using CoreBussiness.UnitOfWork;
using DNTPersianUtils.Core;
using LicenseProject.ApiModels;
using LicenseProject.ApiModels.Auth;
using LicenseProject.ApiModels.Clients;
using LicenseProject.ApiModels.License;
using LicenseProject.LicenseService;
using LicenseProject.Messaging.GhasedakProvider;
using LicenseProject.Messaging.IpPanelProvider;
using LicenseProject.StartupModuleServices.CodeGenerators;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace LicenseProject.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ApiServerController:ControllerBase
{
    private IManagerService _managerService;
    private ICodeGeneratorService _codeGeneratorService;
    private IUnitOfWork _work;
    private IMapper _mapper;

    public ApiServerController(IManagerService managerService, ICodeGeneratorService codeGeneratorService, IUnitOfWork work, IMapper mapper)
    {
        _managerService = managerService;
        _codeGeneratorService = codeGeneratorService;
        _work = work;
        _mapper = mapper;
    }
    // اعتبار سنجی تایپ اول .   
    [HttpPost]
    public async Task<IActionResult> ValidateClient([FromBody] ValidateClientModel? model)
    {
        if (ModelState.IsValid)
        {
            var license = await _managerService.LicenseService.GetLicenseAsync(model.AppSerial);
            
            if (license == null)
            {
                // دیتا نال خواهد بود .
                return Ok(new LicenseResponse(){IsExists = false,CompanyName = null,ExpireDate = null,IsMobile = false,IsSms = false,IsValid = false,LicenseKey = null,LicenseType = null,SerialNo = null,SystemCode = null,HasData = false,IsOtpConfirmed = license.IsOtpConfirmed});
            }
            
            if (license!.CompanyAddress == null && license.CompanyName == null && license.ConstPhone == null &&
                license.LastName == null && license.Name == null && license.LegalCode == null &&
                license.PhoneNumber == null)
            {
                // یعنی باید آی پی ای تکمیل اطلاعات لایسنس کال بشه .
                var client = await _managerService.ClientService.GetClientAsync(license.LicenseCode, license.Id);
                return Ok(new LicenseResponse(){IsExists = true,ExpireDate = license.Expiration.ToShortPersianDateTimeString(),CompanyName = null,IsMobile = license.IsMobileVersionActive,IsSms = license.IsSmsPanelActive,IsValid = license.IsActive,HasData = false,SerialNo = license.LicenseCode,LicenseKey = null,LicenseType = Convertor.GetLicenseType(license.SoftwareType),SystemCode = client.SystemSerial,IsOtpConfirmed = license.IsOtpConfirmed});
            }
            var otp = new Otp() {Code = _codeGeneratorService.GenerateOtpCode(5), LicenseId = license.Id};
            await _managerService.OtpService.AddNewOtpAsync(otp);
            var saveChange = await _work.SaveChangesAsync();
            if (saveChange > 0)
            {
                var smsManager = new IpPanelSmsManager(new IpPanelOptions());
                await smsManager.SendOtpMessage("zim8kjfxnfxpI7y",license.PhoneNumber,new List<string>{"verification-code",otp.Code});


                  var client = await _managerService.ClientService.GetClientAsync(license.LicenseCode, license.Id);
                  var Content = LicenseConvertor.ConvertLicenseKey(license,client);
                  //  .با موفقیت انجام شذه . ای پی ای وریفای موبایل کال باید بشه
                 return Ok(new LicenseResponse(){CompanyName = license.CompanyName,ExpireDate = license.Expiration.ToShortPersianDateTimeString(),HasData = true,IsExists = true,IsMobile = license.IsMobileVersionActive,IsSms = license.IsSmsPanelActive,IsValid = license.IsActive,LicenseKey = Content,LicenseType =Convertor.GetLicenseType(license.SoftwareType),SerialNo = license.LicenseCode,SystemCode = client.SystemSerial,IsOtpConfirmed = license.IsOtpConfirmed});
            }
            // خظای ثبت اطلاعات از طرف سرور رخ داد
            return Problem(statusCode:StatusCodes.Status500InternalServerError);
        }
        // خطای نامعتبر بودن دیتا رخ داد .
        return Problem(statusCode:StatusCodes.Status400BadRequest);
    }

    
    // اعتبار سنجی تایپ دوم
    [HttpPost]
    public async Task<IActionResult> CompleteLicense([FromBody] CompleteLicenseModel model)
    {
        if (ModelState.IsValid)
        {
            var lisense = await _managerService.LicenseService.GetLicenseAsync(model.LicenseCode);
         
            _mapper.Map(model, lisense);
            var otp = new Otp() {Code = _codeGeneratorService.GenerateOtpCode(5), LicenseId = lisense.Id};
            await _managerService.OtpService.AddNewOtpAsync(otp);
            var saveChanges = await _work.SaveChangesAsync();
            if (saveChanges > 0)
            {
                 // var smsManager = new GhasedakSmsManager(new GhasedakOptions());
                 // await smsManager.SendOtpMessage("کد تایید شما", lisense.PhoneNumber!,new List<string>(){otp.Code});
                 
                  // با موفیقیت انجام شد . ای پی ای وریفای موبایل باید کال بشه.
                  var License = await _managerService.LicenseService.GetLicenseAsync(model.LicenseCode);
                  var client = await _managerService.ClientService.GetClientAsync(lisense.LicenseCode, lisense.Id);
                  
                return Ok(new LicenseResponse(){IsExists = true, CompanyName = lisense.CompanyName,ExpireDate = lisense.Expiration.ToShortPersianDateTimeString(),HasData = true,IsMobile = lisense.IsMobileVersionActive,IsSms = lisense.IsSmsPanelActive,IsValid = lisense.IsActive,LicenseKey = null,LicenseType =Convertor.GetLicenseType(License.SoftwareType),SerialNo = lisense.LicenseCode,SystemCode = client.SystemSerial,IsOtpConfirmed = lisense.IsOtpConfirmed});
            }
            // خطای ثبت اطلاعات سرور
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
        // خطاای نامعتبر بودن دیتا
        return Problem(statusCode: StatusCodes.Status400BadRequest);
    }
    
    // اعتبار سنجی تایپ سوم
    [HttpPost]
    public async Task<IActionResult> VerifyPhone([FromBody]VerifyPhoneModel model)
    {
        if (ModelState.IsValid)
        {
            var otp = await _managerService.OtpService.GetOtpAsync(model!.Code);
            var license = await _managerService.LicenseService.GetLicenseAsync(otp.LicenseId);
            var client = await _managerService.ClientService.CatchClientAsync(license.Id);
            if (otp.IsAuthentic)
            {
                otp.IsUsed = true;
                license.IsOtpConfirmed = true;
                var saveChanges = await _work.SaveChangesAsync();
                if (saveChanges > 0)
                {
                    var Content = LicenseConvertor.ConvertLicenseKey(license,client);
                    return Ok(new LicenseResponse() {CompanyName = license.CompanyName,ExpireDate = license.Expiration.ToShortPersianDateTimeString(),HasData = true,IsExists = true,IsMobile = license.IsMobileVersionActive,IsSms = license.IsSmsPanelActive,IsValid = license.IsActive,LicenseKey = Content,LicenseType = Convertor.GetLicenseType(license.SoftwareType),SerialNo = license.LicenseCode,SystemCode = client.SystemSerial,IsOtpConfirmed = license.IsOtpConfirmed});
                }
                // خطای ثبت اطلاعات سرور
                return Problem(statusCode: StatusCodes.Status500InternalServerError);
            }
            // خطای منقضی بودن کد تایید
            return Problem(statusCode: StatusCodes.Status400BadRequest);
        }
        // خطای نامعتبر بودن اطلاعات
        return Problem(statusCode: StatusCodes.Status400BadRequest);
    }
    
    
}