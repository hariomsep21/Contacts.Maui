
using Contacts.Maui.Models;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using Contact = Contacts.Maui.Models.Contact;

namespace Contacts.Maui.Views;
public partial class ContactsPage : ContentPage
{

    public ContactsPage()
    {
        InitializeComponent();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SearchBar.Text = string.Empty;
        LoadContacts();
    }

    private async void listContacts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (listContacts.ItemsSource != null)
        {
            //when we click on any label then the id move contact to edit page and show name of user those info selected;
            await Shell.Current.GoToAsync($"{nameof(EditContactsPage)}?Id={((Contact)listContacts.SelectedItem).ContactId}");

        }

    }

    private void listContacts_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        listContacts.ItemsSource = null;
    }

    private void btnAdd_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(AddContactPage));
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        var menuItem = sender as MenuItem;
        var contact = menuItem.CommandParameter as Contact;
        ContactRepository.DeleteContact(contact.ContactId);
        LoadContacts();
    }
    private void LoadContacts()
    {
        var contacts = new ObservableCollection<Contact>(ContactRepository.GetContacts());
        listContacts.ItemsSource = contacts;

    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var contacts = new ObservableCollection<Contact>(ContactRepository.SearchContacts(((SearchBar)sender).Text));
        listContacts.ItemsSource = contacts;
    }


    private async void Phone_Clicked(object sender, EventArgs e)
    {
        if (PhoneDialer.Default.IsSupported)
        {
            var button = (Button)sender;
            var contact = button.BindingContext as Contact;

            if (contact != null && !string.IsNullOrEmpty(contact.Phone))
            {
                PhoneDialer.Default.Open(contact.Phone);

                // Get the call duration
                int callDuration = ContactRepository.GetCallDuration(contact.Phone);
                string ReceiverName = contact.Name;
                string ReceiverNumber = contact.Phone;
                TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);

                // Display the call duration with a 10-second delay
                if (callDuration > 0)
                {
                    await Task.Delay(10000); // 10-second delay

                    await DisplayAlert("Call Duration", $"Last Call with {ReceiverName} on {ReceiverNumber}  \nCall End: {currentTime}  \nCall duration: {callDuration} seconds", "OK");
                }
                else
                {
                    await Task.Delay(10000); // 10-second delay

                   await DisplayAlert("Call Duration", "Call duration information not available", "OK");
                }
            }
        }
    }





    private void Email_Clicked_1(object sender, EventArgs e)
    {
        if (Email.Default.IsComposeSupported)
        {
            var button = (Button)sender;
            var contact = button.BindingContext as Contact;

            if (contact != null && !string.IsNullOrEmpty(contact.Email))
            {
                string subject = "Hello friends!";
                string body = "It was great to see you last weekend.";
                string[] recipients = new[] { contact.Email };

                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    BodyFormat = EmailBodyFormat.PlainText,
                    To = new List<string>(recipients)
                };

                Email.Default.ComposeAsync(message);
            }
        }
    }

    private void Message_Clicked(object sender, EventArgs e)
    {
        if (Sms.Default.IsComposeSupported)
        {
            var button = (Button)sender;
            var contact = button.BindingContext as Contact;

            if (contact != null && !string.IsNullOrEmpty(contact.Phone))
                PhoneDialer.Open(contact.Phone);

            string[] recipients = new[] { contact.Phone };
            string text = "Hello, I'm interested in buying your vase.";

            var message = new SmsMessage(text, recipients);

            Sms.ComposeAsync(message);
        }
    }
    //async void LoadLazyView_Clicked(object sender, EventArgs e)
    //{
    //    await LazyUserAction.LoadViewAsync();
    //}

}