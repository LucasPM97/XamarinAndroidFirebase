using System;
using System.Collections.Generic;

namespace Entities
{
    public class DocumentItem
    {
        public List<string> Tags { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string documentImageUrl { get; set; }
    }
}