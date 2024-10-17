using Microsoft.EntityFrameworkCore;
using Backoffice.Domain.Shared;

namespace Backoffice.Domain.Staffs
{
    [Owned]
    public class AvailabilitySlot
    {
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }

        private AvailabilitySlot()
        {

        }

        public AvailabilitySlot(string startTime, string endTime)
        {

            if (!DateTime.TryParse(startTime, out DateTime parsedStartTime))
            {
                throw new BusinessRuleValidationException("Error: Invalid start time format.");
            }

            if (!DateTime.TryParse(endTime, out DateTime parsedEndTime))
            {
                throw new BusinessRuleValidationException("Error: Invalid end time format.");
            }

            if (parsedEndTime <= parsedStartTime)
            {
                throw new BusinessRuleValidationException("Error: The end time must be after the start time!");
            }

            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public override string ToString()
        {
            return StartTime + " - " + EndTime;
        }
    }
}