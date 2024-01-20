using Hyper.Core;
using Hyper.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hyper.Services.Messages
{
    public class SmsSender : ISmsSender
    {
        private readonly IStoreContext _storeContext;
        public SmsSender(IStoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task SendSmsAsync(string message, params string[] gsmNumbers)
        {
            if (!_storeContext.CurrentStore.SmsHeader.IsNullOrEmpty()
                || !_storeContext.CurrentStore.SmsUsername.IsNullOrEmpty()
                || !_storeContext.CurrentStore.SmsPassword.IsNullOrEmpty())
            {
                await Task.CompletedTask;
            }
            else
            {
                //var username = "5323517479";
                //var password = "1%D3CA5hll";
                //var msgHeader = "8503093351";

                var company = "Netgsm İletişim Hizmetleri";
                var filter = "";
                var appKey = "";
                var dil = "TR";


                #region Xml

                string content = "";
                content += "<?xml version='1.0' encoding='UTF-8'?>";
                content += "<mainbody>";
                content += "<header>";
                content += "<company dil='" + dil + "'>" + company + "</company>";
                content += "<usercode>" + _storeContext.CurrentStore.SmsUsername + "</usercode>";
                content += "<password>" + _storeContext.CurrentStore.SmsPassword + "</password>";
                content += "<type>1:n</type>";
                content += "<msgheader>" + _storeContext.CurrentStore.SmsHeader + "</msgheader>";
                content += "</header>";
                content += "<body>";
                content += "<msg>";
                content += "<![CDATA[" + message + "]]>";
                content += "</msg>";
                foreach (var gsmNumber in gsmNumbers)
                    content += "<no>" + gsmNumber.Replace("+90", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(",05", ",5") + "</no>";
                content += "</body>";
                content += "</mainbody>";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.netgsm.com.tr/sms/send/xml");
                var requestContent = new StringContent(content, null, "application/xml");
                request.Content = requestContent;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                #endregion

                //Kod Anlamı   kf242427
                //00  Görevinizin tarih formatinda bir hata olmadığını gösterir.
                //01  Mesaj gönderim baslangıç tarihinde hata var. Sistem tarihi ile değiştirilip işleme alındı.
                //02  Mesaj gönderim sonlandırılma tarihinde hata var.Sistem tarihi ile değiştirilip işleme alındı.Bitiş tarihi başlangıç tarihinden küçük girilmiş ise, sistem bitiş tarihine içinde bulunduğu tarihe 24 saat ekler.
                //347022009   Gönderdiğiniz SMS'inizin başarıyla sistemimize ulaştığını gösterir. Bu görevid niz ile mesajınızın durumunu sorguyabilirsiniz.
                //00 5Fxxxxxx - 2xxx - 4xxE - 8xxx - 8A5xxxxxxxxxxxx Gönderdiğiniz SMS'inizin başarıyla sistemimize ulaştığını gösterir. Bu görev(bulkid) sorgulanabilir, Raporlama servisinde bulkID bilgisi olarak 5Fxxxxxx-2xxx-4xxE-8xxx-8A5xxxxxxxxxxxx verilebilir. Bu outputu almanızın sebebi, 5 dakika boyunca ard arda gönderdiğiniz SMS'lerin sistemimiz tarafında çoklanarak(biriktirilerek) 1 dakika içerisinde gönderileceği anlamına gelir.

                //20  Mesaj metninde ki problemden dolayı gönderilemediğini veya standart maksimum mesaj karakter sayısını geçtiğini ifade eder. (Standart maksimum karakter sayısı 917 dir.Eğer mesajınız türkçe karakter içeriyorsa Türkçe Karakter Hesaplama menüsunden karakter sayılarının hesaplanış şeklini görebilirsiniz.)
                //30  Geçersiz kullanıcı adı , şifre veya kullanıcınızın API erişim izninin olmadığını gösterir.Ayrıca eğer API erişiminizde IP sınırlaması yaptıysanız ve sınırladığınız ip dışında gönderim sağlıyorsanız 30 hata kodunu alırsınız.API erişim izninizi veya IP sınırlamanızı, web arayüzden; sağ üst köşede bulunan ayarlar > API işlemleri menüsunden kontrol edebilirsiniz.
                //40  Mesaj başlığınızın(gönderici adınızın) sistemde tanımlı olmadığını ifade eder.Gönderici adlarınızı API ile sorgulayarak kontrol edebilirsiniz.
                //50  Abone hesabınız ile İYS kontrollü gönderimler yapılamamaktadır.
                //51  Aboneliğinize tanımlı İYS Marka bilgisi bulunamadığını ifade eder.
                //70  Hatalı sorgulama. Gönderdiğiniz parametrelerden birisi hatalı veya zorunlu alanlardan birinin eksik olduğunu ifade eder.
                //85  Mükerrer Gönderim sınır aşımı. Aynı numaraya 1 dakika içerisinde 20'den fazla görev oluşturulamaz.

                //if (responseHtml == "00")
                //    SuccessNotification("Sms gönderim işlemi başarıyla tamamlandı..");

                //if (responseHtml == "01")
                //    SuccessNotification("Mesaj gönderim baslangıç tarihinde hata var. Sistem tarihi ile değiştirilip işleme alındı..");


                //if (responseHtml == "02")
                //    SuccessNotification("Mesaj gönderim sonlandırılma tarihinde hata var.Sistem tarihi ile değiştirilip işleme alındı..");


                //if (html.Length > 3)
                //    SuccessNotification("SMS'iniz başarıyla  " + _topluSmsSettings.ProviderTitle + " sistemine ulaştı..");

                //// hata mesajları
                //if (responseHtml == "20")
                //    WarningNotification(" Mesaj metninde ki problemden dolayı gönderilemediğini veya standart maksimum mesaj karakter sayısını geçtiğini ifade eder. (Standart maksimum karakter sayısı 917 dir.Eğer mesajınız türkçe karakter içeriyorsa Türkçe Karakter Hesaplama menüsunden karakter sayılarının hesaplanış şeklini görebilirsiniz.)");

                //if (responseHtml == "30")
                //    WarningNotification("Geçersiz kullanıcı adı , şifre veya kullanıcınızın API erişim izninin olmadığını gösterir. Ayrıca eğer API erişiminizde IP sınırlaması yaptıysanız ve sınırladığınız ip dışında gönderim sağlıyorsanız 30 hata kodunu alırsınız.API erişim izninizi veya IP sınırlamanızı, web arayüzden; sağ üst köşede bulunan ayarlar > API işlemleri menüsunden kontrol edebilirsiniz.");

                //if (responseHtml == "40")
                //    WarningNotification("Mesaj başlığınızın(gönderici adınızın) sistemde tanımlı olmadığını ifade eder.Gönderici adlarınızı API ile sorgulayarak kontrol edebilirsiniz.");

                //if (responseHtml == "50")
                //    WarningNotification("Abone hesabınız ile İYS kontrollü gönderimler yapılamamaktadır.");

                //if (responseHtml == "51")
                //    WarningNotification("Aboneliğinize tanımlı İYS Marka bilgisi bulunamadığını ifade eder.");

                //if (responseHtml == "70")
                //    WarningNotification("Hatalı sorgulama. Gönderdiğiniz parametrelerden birisi hatalı veya zorunlu alanlardan birinin eksik olduğunu ifade eder.");

                //if (responseHtml == "85")
                //    WarningNotification("Mükerrer Gönderim sınır aşımı. Aynı numaraya 1 dakika içerisinde 20'den fazla görev oluşturulamaz.");
            }

        }
    }
}
