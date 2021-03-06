using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.MLBox.Foundation.Common.Models;
using Hackathon.MLBox.Foundation.Common.Models.DTO;
using Sitecore.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;

namespace Hackathon.MLBox.Foundation.Engine.Services
{
    public class XConnectService
    {
        public static string IdentificationSource => "demo";
        public static string IdentificationSourceEmail => "demo_email";

      
        public async Task<Contact> GetContact(string identifier)
        {
            using (XConnectClient client = SitecoreXConnectClientConfiguration.GetClient())
            {
                IdentifiedContactReference reference = new IdentifiedContactReference(IdentificationSource, identifier);
                var customer = await client.GetAsync(
                    reference,
                    new ContactExpandOptions(
                        PersonalInformation.DefaultFacetKey,
                        EmailAddressList.DefaultFacetKey,
                        ContactBehaviorProfile.DefaultFacetKey
                    )
                    {
                        Interactions = new RelatedInteractionsExpandOptions
                        {
                            StartDateTime = DateTime.MinValue,
                            EndDateTime = DateTime.MaxValue
                        }
                    }
                );

                return customer;
            }
        }

        // if addWebVisit=true, fake webvisit will be created for interaction 
        // it is needed if you want to populate interaction country (to use contacts country for ML data model)
        public async Task<bool> Add(Customer purchase, bool addWebVisit = false)
        {
            using (XConnectClient client = SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {

                    IdentifiedContactReference reference = new IdentifiedContactReference(IdentificationSource, purchase.CustomerId.ToString());
                    var customer = await client.GetAsync(
                        reference,
                        new ContactExpandOptions(
                            PersonalInformation.DefaultFacetKey,
                            EmailAddressList.DefaultFacetKey,
                            ContactBehaviorProfile.DefaultFacetKey
                        )
                        {
                            Interactions = new RelatedInteractionsExpandOptions
                            {
                                StartDateTime = DateTime.MinValue,
                                EndDateTime = DateTime.MaxValue
                            }
                        }
                    );

                    if (customer == null)
                    {
                        var email = "demo" + Guid.NewGuid().ToString("N") + "@gmail.com";

                        customer = new Contact(new ContactIdentifier(IdentificationSource, purchase.CustomerId.ToString(), ContactIdentifierType.Known));

                        var preferredEmail = new EmailAddress(email, true);
                        var emails = new EmailAddressList(preferredEmail, "Work");

                        client.AddContact(customer);
                        client.SetEmails(customer, emails);

                        var identifierEmail = new ContactIdentifier(IdentificationSourceEmail, email, ContactIdentifierType.Known);
                        
                        client.AddContactIdentifier(customer, identifierEmail);
                        client.Submit();

                        var channel = Guid.Parse("DF9900DE-61DD-47BF-9628-058E78EF05C6");
                        DeviceProfile newDeviceProfile = new DeviceProfile(Guid.NewGuid()) { LastKnownContact = customer };
                        client.AddDeviceProfile(newDeviceProfile);

                        var orders = purchase.Invoices.GroupBy(x => x.Number);
                        foreach (var order in orders)
                        {
                            var total = order.Sum(x => x.Price * x.Quantity);
                            var data = order.First().TimeStamp;
                            var currency = order.First().Currency;

                            var interaction = new Interaction(customer, InteractionInitiator.Contact, channel, "demo app");
                            var outcome = new Outcome(new Guid("{9016E456-95CB-42E9-AD58-997D6D77AE83}"), data, currency, total);

                            interaction.Events.Add(outcome);
                            if (addWebVisit)
                            {
                                //Add Device profile

                                interaction.DeviceProfile = newDeviceProfile;

                                //Add fake Ip info
                                IpInfo fakeIpInfo = new IpInfo("127.0.0.1") { BusinessName = "Home" };
                                client.SetFacet(interaction, IpInfo.DefaultFacetKey, fakeIpInfo);

                                // Add fake webvisit
                                // Create a new web visit facet model
                                var webVisitFacet = new WebVisit
                                {
                                    Browser =
                                        new BrowserData
                                        {
                                            BrowserMajorName = "Chrome",
                                            BrowserMinorName = "Desktop",
                                            BrowserVersion = "22.0"
                                        },
                                    Language = "en",
                                    OperatingSystem =
                                        new OperatingSystemData { Name = "Windows", MajorVersion = "10", MinorVersion = "4" },
                                    Referrer = "https://www.google.com",
                                    Screen = new ScreenData { ScreenHeight = 1080, ScreenWidth = 685 },
                                    SearchKeywords = "sitecore",
                                    SiteName = "website"
                                };

                                // Populate data about the web visit

                                var itemId = Guid.Parse("110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9");
                                var itemVersion = 1;

                                // First page view
                                var datetime = purchase.Invoices.FirstOrDefault() == null
                                    ? DateTime.Now
                                    : purchase.Invoices.First().TimeStamp.ToUniversalTime();
                                PageViewEvent pageView = new PageViewEvent(datetime,
                                    itemId, itemVersion, "en")
                                {
                                    ItemLanguage = "en",
                                    Duration = new TimeSpan(3000),
                                    Url = "/home"
                                };
                                // client.SetFacet(interaction, WebVisit.DefaultFacetKey, webVisitFacet);

                                interaction.Events.Add(pageView);
                                client.SetWebVisit(interaction, webVisitFacet);
                            }
                            client.AddInteraction(interaction);
                        }



                        await client.SubmitAsync();
                    }

                  

                    return true;
                }
                catch (XdbExecutionException ex)
                {
                    Log.Error(ex.Message, ex, this);
                    return false;
                }
            }
        }
    }
}