using System.Collections.Generic;
using System.Linq;

namespace MobileInspection.Core
{
    public class InspectionRepository
    {
        private static readonly List<Inspection> Inspections = new List<Inspection>();

        public IEnumerable<Inspection> All()
        {
            return Inspections.ToList();
        }

        public void Save(Inspection inspection)
        {
            Inspections.Add(inspection);
        }
    }
}