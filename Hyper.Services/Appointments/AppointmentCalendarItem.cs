using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Hyper.Services.Appointments
{
    public class AppointmentCalendarItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("hoverText")]
        public string HoverText { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("paymentTypeId")]
        public int PaymentTypeId { get; set; }

        [JsonProperty("paymentStatusId")]
        public int PaymentStatusId { get; set; }

        [JsonProperty("appointmentStatusId")]
        public int AppointmentStatusId { get; set; }

        [JsonProperty("color")]
        public string Color { get;  set; }
    }
}
