using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.ViewModels
{
    public class SystemVM
    {
        public int TSSystemId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsPublished { get; set; }
        public string Description { get; set; }
    }

    public class PublishVM
    {
        public int SystemId { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }

}
