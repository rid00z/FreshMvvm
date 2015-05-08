using System;
using System.Collections.Generic;

namespace FreshMvvmSampleApp
{
	public class DatabaseService : IDatabaseService 
    {
        public DatabaseService() 
        {
        }

		public void UpdateContact (Contact contact)
		{
			//todo
		}

		public void UpdateQuote (Quote quote)
		{
			//todo
		}

		public List<Contact> GetContacts ()
		{
			return new List<Contact> {
				new Contact { Name = "Xam Consulting", Phone = "0404 865 350" },
				new Contact { Name = "Michael Ridland", Phone = "0404 865 350" },
				new Contact { Name = "Thunder Apps", Phone = "0404 865 350" },
			};
		}

		public List<Quote> GetQuotes ()
		{
			return new List<Quote> {
				new Quote { CustomerName = "Xam Consulting", Total = "$350.00" },
				new Quote { CustomerName = "Michael Ridland", Total = "$3503.00" },
				new Quote { CustomerName = "Thunder Apps", Total = "$3504.00" },
			};
		}
    }
}

