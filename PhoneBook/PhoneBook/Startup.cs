using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhoneBook.Startup))]
namespace PhoneBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
