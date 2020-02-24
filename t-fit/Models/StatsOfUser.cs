using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_fit.Models
{
    public class StatsOfUser
    {
        public string Name { get; set; }
        public int AverageSteps { get; set; }
        public int MinSteps { get; set; }
        public int MaxSteps { get; set; }
        public int Rank { get; set; }
        public string Status { get; set; }
        public bool IsResultStable { get; set; }
    }
}
