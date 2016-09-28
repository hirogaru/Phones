using System.Windows;
using Phones.Controllers;
using Ninject;

namespace Phones
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private IContactController _controller;

        public MainWindow()
        {
            InitializeComponent();
            //_controller = new ContactsController(new ContactsModel());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IContactController _controller = App.AppKernel.Get<ContactsController>();

            _controller.ShowContactsView(this);
            this.Hide();
        }
    }
}
