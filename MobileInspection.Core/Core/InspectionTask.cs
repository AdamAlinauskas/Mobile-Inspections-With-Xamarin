using System.Linq;

namespace MobileInspection.Core.Core
{
    public class InspectionTask
    {
        public string[] GetInspectionNames()
        {
            return new InspectionRepository().All().Select(x => x.Title).ToArray();
        }

        public void Save(string title, string location)
        {
            new InspectionRepository().Save(new Inspection{Title = title});
        }
    }
}