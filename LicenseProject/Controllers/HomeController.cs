﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LicenseProject.Models;

namespace LicenseProject.Controllers;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }
    
}