using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf.Models.Reports
{
    public class Report
    {
        public string Reason { get; set; }
    }

    public class ClubReport : Report
    {
        public Club Club { get; set; }
    }
}
