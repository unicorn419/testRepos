using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlotAPI.Models;
using SlotAPI.Service;


namespace SlotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly SlotSpinService slotSpinService;
        public SlotController(SlotSpinService slotSpinService)
        {
            this.slotSpinService = slotSpinService;
        }

        [HttpGet]
        public void values([FromBody] string str)
        {
            
        }

        // POST api/Slot
        [HttpPost]
        [Authorize]
        public SlotSpinResponse Post(SlotSpinReq  slotSpinReq)
        {
            return slotSpinService.HandleRequest(slotSpinReq);
        }
    }
}
