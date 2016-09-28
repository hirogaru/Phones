using Ninject;
using Phones.Controllers;
using Phones.DataAccess;
using Phones.Models;
using System.Windows;

namespace Phones
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IKernel AppKernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            AppKernel = new StandardKernel();
            AppKernel.Bind<IRepository>().To<SqlRepository>();
            AppKernel.Bind<ContactsController>().ToSelf().WithConstructorArgument("contactModel", new ContactsModel());
        }
    }
}
