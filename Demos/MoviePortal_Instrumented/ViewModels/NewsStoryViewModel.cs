using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.ViewModels
{
    public class NewsStoryViewModel
    {
        public DateTimeOffset Published { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
    }
}
