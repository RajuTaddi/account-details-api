using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using account.details.core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace account.details.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        // GET: api/Transactions/afbcf903-9784-400b-afd0-b497c01addc3
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _service.GetTransactionById(id);
            return Ok(response);
        }
    }
}
