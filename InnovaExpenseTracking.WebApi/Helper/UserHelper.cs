using Azure.Core;
using InnovaExpenseTracking.WebApi.Context;
using InnovaExpenseTracking.WebApi.Dtos;
using InnovaExpenseTracking.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;

namespace InnovaExpenseTracking.WebApi.Helper
{
    public class UserHelper
    {
        private readonly ApplicationDbContext _context;

        public UserHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        
       
    }
}
