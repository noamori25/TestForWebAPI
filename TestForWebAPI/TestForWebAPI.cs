﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectManagmentSystem.POCO;
using UnitTestProject1;

namespace TestForWebAPI
{
    [TestClass]
    public class UnitTest1
    {
        TestCenerAPI test = new TestCenerAPI();
        HttpClient client = new HttpClient();

        [TestMethod]
        public void AnonymousGetAllFlights()
        {
            string url = "http://localhost:60341/api/anonymousFacade/GetAllFlights";
            List<Flight> flights = test.AnonymousFacade.GetAllFlights().ToList();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<Flight> flightsAPI = response.Content.ReadAsAsync<List<Flight>>().Result;
            if (flights != null && flightsAPI != null)
            {
                Assert.AreEqual(flights.Count, flightsAPI.Count);
                for (int i = 0; i < flights.Count; i ++)
                {
                    Assert.AreEqual(flights[i].Id, flightsAPI[i].Id);
                    Assert.AreEqual(flights[i].AirlineCompanyId, flightsAPI[i].AirlineCompanyId);
                    Assert.AreEqual(flights[i].RemainingTickets, flightsAPI[i].RemainingTickets);
                    Assert.AreEqual(flights[i].OriginCountryCode, flightsAPI[i].OriginCountryCode);
                    Assert.AreEqual(flights[i].DepartureTime, flightsAPI[i].DepartureTime);
                    Assert.AreEqual(flights[i].LandingTime, flightsAPI[i].LandingTime);
                    Assert.AreEqual(flights[i].DestinationCountryCode, flightsAPI[i].DestinationCountryCode);
                }
                
            }

        }
        [TestMethod]
        public void AdminstratorDeleteAirline ()
        {
            string url = "http://localhost:60341/api/administratorFacade/DeleteAirline/";
            List<AirlineCompany> airlines = test.AnonymousFacade.GetAllAirlineCompanies().ToList();
            url += $"{airlines[0].Id}";
            AirlineCompany airline = airlines[0];
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{test.AdminToken.User.UserName}:{test.AdminToken.User.Password}");
            string ss = Convert.ToBase64String(plainTextBytes);
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", ss);
            client.DefaultRequestHeaders.Authorization = ahv;
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            airlines = test.AnonymousFacade.GetAllAirlineCompanies().ToList();
            Assert.IsFalse(airlines.Contains(airline));
        }

        [TestMethod]
        public void CustomerGetFlights ()
        {
           // test.DeleteCustomerAndTicket();
            string url = "http://localhost:60341/api/customer/AllMyFLights";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //test.CustomerPurchaseTicketForTest();
            List<Flight> flights = test.CustomerFacade.GetAllMyFlights(test.CustomerToken).ToList();
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{test.CustomerToken.User.UserName}:{test.CustomerToken.User.Password}");
            string ss = Convert.ToBase64String(plainTextBytes);
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", ss);
            client.DefaultRequestHeaders.Authorization = ahv;
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<Flight> flightsAPI = response.Content.ReadAsAsync<List<Flight>>().Result;
            if (flights != null && flightsAPI != null)
            {
                Assert.AreEqual(flights.Count, flightsAPI.Count);
                for (int i = 0; i < flights.Count; i++)
                {
                    Assert.AreEqual(flights[i].Id, flightsAPI[i].Id);
                    Assert.AreEqual(flights[i].AirlineCompanyId, flightsAPI[i].AirlineCompanyId);
                    Assert.AreEqual(flights[i].RemainingTickets, flightsAPI[i].RemainingTickets);
                    Assert.AreEqual(flights[i].OriginCountryCode, flightsAPI[i].OriginCountryCode);
                    Assert.AreEqual(flights[i].DepartureTime, flightsAPI[i].DepartureTime);
                    Assert.AreEqual(flights[i].LandingTime, flightsAPI[i].LandingTime);
                    Assert.AreEqual(flights[i].DestinationCountryCode, flightsAPI[i].DestinationCountryCode);
                }

            }

        }

        [TestMethod]
        public void AirlineGetAllTickets()
        {
            string url = "http://localhost:60341/api/companyFacade/GetAllTickets";
            List<Ticket> tickets = test.AirlineFacade.GetAllMyTickets(test.AirlineToken).ToList();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{test.AirlineToken.User.UserName}:{test.AirlineToken.User.Password}");
            string ss = Convert.ToBase64String(plainTextBytes);
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", ss);
            client.DefaultRequestHeaders.Authorization = ahv;
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<Ticket> ticketsAPI = response.Content.ReadAsAsync<List<Ticket>>().Result;
            if (tickets != null && ticketsAPI != null)
            {
                Assert.AreEqual(tickets.Count, ticketsAPI.Count);
                for (int i = 0; i < tickets.Count; i++)
                {
                    Assert.AreEqual(tickets[i].Id, ticketsAPI[i].Id);
                    Assert.AreEqual(tickets[i].FlightId, ticketsAPI[i].FlightId);
                    Assert.AreEqual(tickets[i].CustomerId, ticketsAPI[i].CustomerId);
                }
            }
        }
    }
    
}
