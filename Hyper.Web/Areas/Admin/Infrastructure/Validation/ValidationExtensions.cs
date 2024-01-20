using Hyper.Core.Extensions;

namespace Hyper.Web.Areas.Admin.Infrastructure.Validation
{
    public static class ValidationExtensions
    {
        public static bool ValidateTurkishIdentityNumber(string turkishIdentityNumber)
        {
            bool returnvalue = false;
            if (!turkishIdentityNumber.IsNullOrEmpty() && turkishIdentityNumber.Length == 11)
            {
                if (turkishIdentityNumber == "11111111111")
                    return true;

                long ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                TcNo = long.Parse(turkishIdentityNumber);

                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;

                C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                Q1 = (10 - ((C1 + C3 + C5 + C7 + C9) * 3 + C2 + C4 + C6 + C8) % 10) % 10;
                Q2 = (10 - ((C2 + C4 + C6 + C8 + Q1) * 3 + C1 + C3 + C5 + C7 + C9) % 10) % 10;

                returnvalue = BTCNO * 100 + Q1 * 10 + Q2 == TcNo;
            }
            return returnvalue;
        }
    }
}
