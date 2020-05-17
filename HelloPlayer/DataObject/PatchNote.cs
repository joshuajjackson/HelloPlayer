using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject
{
    public class PatchNote
    {
        public string PatchID { get; set; }
        public string PatchName { get; set; }
        public string PatchDescription { get; set; }
        public DateTime PatchDate { get; set; }
        public List<PatchLine> PatchLines { get; set; }
    }

    public class PatchLine
    {
        public int PatchLineID { get; set; }
        public string PatchLineName { get; set; }
        public string PatchLineDescription { get; set; }
    }
}
