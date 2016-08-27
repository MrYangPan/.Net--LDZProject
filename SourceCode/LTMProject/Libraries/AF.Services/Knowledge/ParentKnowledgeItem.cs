using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Services.Knowledge
{

    public class ParentKnowledgeItem
    {
        public int KPID { get; set; }
        public string ParentId { get; set; }
        public int SubjectID { get; set; }
        public string Name { get; set; }
    }

    public class ParentKnowledgeInfoItem
    {
        public int KPID { get; set; }
        public int SubjectID { get; set; }
        public string Name { get; set; }
        public int TiMuCount { get; set; }
    }
}
