using Phones.Controllers;
using Phones.DataAccess;
using System;
using System.Windows;

namespace Phones.UI
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private Contacts _model = new Contacts();
        private readonly IContactController _controller = null;

        public AddWindow(IContactController contactsController, Contacts newContact)
        {
            InitializeComponent();

            _controller = contactsController;
            _model = newContact;

            contactFields.DataContext = _model;

            if (newContact.ID_Contact != Guid.Empty) this.Title1.Text = "Edit Contact";
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.AddContact(_model)) //пробуем добавить или отредактировать контакт
            {
                validateText.Visibility = Visibility.Collapsed;
                this.Close();
            }
            else validateText.Visibility = Visibility.Visible; //в случае неудачи показываем сообщение об ошибке
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
