using Microsoft.EntityFrameworkCore;

namespace Backoffice.Domain.Shared
{
    [Owned]
    public class PhoneNumber
    {
        public string PhoneNum { get; private set; }

        private PhoneNumber()
        {
        }

        public PhoneNumber(string phoneNum)
        {
            if (phoneNum.Length != 9 || phoneNum.Substring(0,1) != "9")
            {
                throw new BusinessRuleValidationException("Error: The phone number is invalid!");
            }

            this.PhoneNum = phoneNum;
        }

        public override string ToString()
        {
            return this.PhoneNum;
        }
    }
}