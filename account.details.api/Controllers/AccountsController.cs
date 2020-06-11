using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using account.details.core.interfaces;
using account.details.core.services;
using account.details.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace account.details.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.GetAccounts();
            return Ok(response);
        }

        // GET api/values/134545
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult> Get(string accountNumber)
        {
            var response = await _service.GetAccountById(accountNumber);
            return Ok(response);
        }
    }
}
