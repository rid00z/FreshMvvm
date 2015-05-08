using System;
using System.Collections.Generic;

namespace FreshMvvmSampleApp
{
    public interface IDatabaseService
    {
        List<Contact> GetContacts ();

        void UpdateContact (Contact contact);

        List<Quote> GetQuotes ();

        void UpdateQuote (Quote quote);
    }
}

