using System.Collections.Generic;
using System.Linq;
using MobileInspection.Core.Dtos;

namespace MobileInspection.Core.Core
{
    public class InspectionTask
    {
        public List<InspectionListingDto> GetInspectionForListing()
        {
            return new InspectionRepository().All().Select(x => new InspectionListingDto{Title = x.Title,Id=x.Id}).ToList();
        }

        public void Save(string title, string location, string filePath)
        {
            new InspectionRepository().Save(new Inspection{Title = title, Location = location, FilePath = filePath});
        }
    }
}