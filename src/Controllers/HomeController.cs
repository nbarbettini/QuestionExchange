using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionExchange.Models;

namespace QuestionExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Tenant _currentTenant;
        
        public HomeController(AppDbContext context, Tenant tenant)
        {
            _context = context;
            _currentTenant = tenant;
        }

        public async Task<IActionResult> Index()
        {
            var topQuestions = await _context
                .Questions
                .Where(q => q.Tenant.Id == _currentTenant.Id)
                .OrderByDescending(q => q.UpdatedAt)
                .Take(5)
                .ToArrayAsync();
                
            var viewModel = new QuestionListViewModel
            {
                Questions = topQuestions
            };
            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
