using System.Globalization;

namespace EMPManegment.Web.Helper
{
    public class Common
    {
        public Common(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string GetCommonDateFormat(DateTime date)
        {
            return date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
        }
    }
}
