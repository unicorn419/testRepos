using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class SlotSpinResponse
    {
        public int Balance { get; set; }
        public int WonAmount { get; set; }
        public int[] Results { get; set; }
    }
}
