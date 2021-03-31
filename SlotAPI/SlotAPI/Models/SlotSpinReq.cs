using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class SlotSpinReq
    {
        public string Uid { get; set; }
        public int Bet { get; set; } = 0;
        public string token { get; set; }

    }
}
