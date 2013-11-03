using System;

namespace MobileInspection.Core
{
    public class Inspection
    {
        public Inspection()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string FilePath { get; set; }
    }
}
