using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Phones.Controllers;
using Phones.DataAccess;
using Phones.Models;

namespace Phones.UI
{
    /// <summary>
    /// Представление получает обновления из модели и через контроллер передаёт данные обратно в модель
    /// </summary>
    public partial class ContactsWindow : Window
    {
        private readonly IContactsModel _model;
        private readonly IContactController _controller = null;

        public ContactsWindow(IContactController contactsController, IContactsModel contactsModel)
        {
            InitializeComponent();
            _controller = contactsController;
            _model = contactsModel;

            _model.ContactUpdated += model_ViewUpdated; //подписываемся на событие изменения модели
            gridContacts.ItemsSource = _model.Contacts.ToList();
        }

        void model_ViewUpdated(object sender, EventArgs e)
        {
            gridContacts.ItemsSource = _model.Contacts.ToList();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ShowSearchResult(searchString.Text);
        }

        private void addNew_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _controller.AddContact(this);
        }

        private void updateItem_Click(object sender, RoutedEventArgs e)
        {
            if (gridContacts.SelectedIndex == -1)
                return;
            _controller.AddContact(this, gridContacts.SelectedItem as Contacts);
        }

        private void deleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (gridContacts.SelectedIndex == -1)
                return;
            if (MessageBox.Show("Вы действительно хотите удалить этот контакт?", "Подтверждение удаления", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                _controller.DeleteContact((gridContacts.SelectedItem as Contacts).ID_Contact);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }
    }
}
