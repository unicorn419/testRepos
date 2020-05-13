using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileProcessEntityLibrary.Data;
using FileProcessEntityLibrary.Data.Models.FileProcess;

namespace FileProcessorConfiguration
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientInfoesController : Controller
    {
        private readonly FileProcessContext _context;

        public ClientInfoesController(FileProcessContext context)
        {
            _context = context;
        }

        // GET: api/ClientInfoes
        [HttpGet]
        public IEnumerable<ClientInfo> GetClientInfos()
        {            
            return _context.ClientInfos;
        }

        // GET: api/ClientInfoes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientInfo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientInfo = await _context.ClientInfos.FindAsync(id);

            if (clientInfo == null)
            {
                return NotFound();
            }

            return Ok(clientInfo);
        }

  

        private bool ClientInfoExists(int id)
        {
            return _context.ClientInfos.Any(e => e.ClientInfoId == id);
        }
    }
}