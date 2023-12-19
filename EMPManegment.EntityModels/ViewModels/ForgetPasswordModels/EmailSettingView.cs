using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ForgetPasswordModels
{
    public class EmailSettingView
    {
        public string SecretKey { get; set; } = default!;
        public string From { get; set; } = default!;
        public int Port { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public bool EnableSSL { get; set; } = default!;
    }
    public class SendEmailModel
    {
        public string? Email { get; set; }
    }
}
