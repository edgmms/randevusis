using System.ComponentModel;

namespace Hyper.Core.Domain.Appointments
{
    public enum PeriodType
    {
        [Description("Bir Defa")]
        OneTime = 0,

        //[Description("Günlük")]
        //Daily = 10,

        //[Description("Haftalık")]
        //Weekly = 20,

        //[Description("Her iki haftada bir")]
        //PerTwoWeek =21,

        //[Description("Her üç haftada bir")]
        //PerThreeWeek = 22,

        //[Description("Aylık")]
        //Monthly = 30,
    }
}