using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using account.details.core.interfaces;
using account.details.core.services;
using account.details.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace account.details.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountService service, ILogger<AccountsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.GetAccounts();
            _logger.LogInformation("GetAccounnts: Suucess");
            return Ok(response);
        }

        // GET api/values/134545
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult> Get(string accountNumber)
        {
            var response = await _service.GetAccountById(accountNumber);
            _logger.LogInformation("GetAccountById: Suucess");
            return Ok(response);
        }
    }
}
