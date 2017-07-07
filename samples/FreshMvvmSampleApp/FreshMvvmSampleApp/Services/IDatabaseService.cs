using System.Collections.Generic;
using FreshMvvmSampleApp.Models;

namespace FreshMvvmSampleApp.Services
{
    public interface IDatabaseService
    {
        List<Contact> GetContacts ();

        void UpdateContact (Contact contact);

        List<Quote> GetQuotes ();

        void UpdateQuote (Quote quote);
    }
}

