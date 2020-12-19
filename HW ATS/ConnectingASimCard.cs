using System;
using System.Collections.Generic;
using System.Linq;

namespace HW_ATS
{
    public class ConnectingASimCard <T> where T : AutomaticTelephoneStation
    {
            public event Action<ATSEventArgsMessage> Message;
            public bool IsTheSimCardConnected { get; set; }
            public T SimCard { get; set; }
            
            public ConnectingASimCard(T simCard, bool isTheSimCardConnected)
            {
                SimCard = simCard;
                IsTheSimCardConnected = isTheSimCardConnected;
            }

            Dictionary<Clients, long> ContactPhoneClients = new Dictionary<Clients, long>();

            public void AddContact(Clients client1, Clients client2)
            {
                var result = SimCard.GetClient(client2);

                if (result.Item2 & ContactPhoneClients.All(i => i.Value != result.Item1))
                {

                    ContactPhoneClients.Add(client2, result.Item1);
                    Message?.Invoke(new ATSEventArgsMessage($"{client2.FirstName} {client2.LastName} added to phone {client1.FirstName} {client1.LastName}"));
                }
                else
                {
                        Message?.Invoke(new ATSEventArgsMessage($"Contact { client2.FirstName} {client2.LastName} not added to your phone "));
                }
            }
            public void RemoveContacts(Clients client)
            {

                if (ContactPhoneClients.Any(i => i.Key.FirstName == client.FirstName & i.Key.LastName == client.LastName))
                {
                    ContactPhoneClients.Remove(client);
                    Message?.Invoke(new ATSEventArgsMessage($"Contact {client.FirstName} {client.LastName} successfully removed"));

                }
                else
                {
                    Message?.Invoke(new ATSEventArgsMessage("There is no such contact in your phone"));
                }
                ContactPhoneClients.OrderBy(i => i.Key.FirstName);
            }

            public void GetNumberAddContacts(Clients client) //получить одного клиента из списка контактов или все записанные в телефоне номера
            {
                if (ContactPhoneClients.Any(i => i.Key.LastName == client.LastName))
                {
                    var number = ContactPhoneClients[client];
                    Message?.Invoke( new ATSEventArgsMessage($"Name - {client.FirstName} {client.LastName} Phone - {number}"));
                }
                else
                {
                    Console.WriteLine($"There is no such contact. Check your entire contact list.");
                    foreach (var i in ContactPhoneClients)
                    {
                        Message?.Invoke(new ATSEventArgsMessage($"Name - {i.Key.FirstName} {i.Key.LastName} Phone - {i.Value}"));
                    }
                }
                ContactPhoneClients.OrderBy(i => i.Key.FirstName);
            }

            public void Call1(Clients contact) // эмуляция звонка 1
            {
                var result = SimCard.GetClient(contact); // проверяем есть ли клиент в сети

                if (result.Item2 && IsTheSimCardConnected) // если все в порядке совершаем звонок
                {
                    PortForConnected.CallTimeAndDate(); 
                    Message?.Invoke( new ATSEventArgsMessage($"Call in progress { PortForConnected.CallTimeAndDate().Item2} - " +
                                                             $"subscriber {contact.FirstName} {contact.LastName}"));
                }
                else 
                {
                    Message?.Invoke( new ATSEventArgsMessage($"Connection with {contact.FirstName} {contact.LastName} failed." +
                                                                        " The subscriber's device is turned off or is out of network range.")); 
                }
            }
            public void Call2(Clients contact) // эмуляция звонка 2
            {

                var result = SimCard.GetClient(contact);

                if (result.Item2 && IsTheSimCardConnected) // если всё в порядке совершаем звонок
                {
                    var callDateToday = PortForConnected.CallTimeAndDate().Item2;
                    if (callDateToday == DateTime.Today)// в рамках дня, что бы не рандомить до минуты. если совпадает - то занято
                    { 
                        Message?.Invoke(new ATSEventArgsMessage($"The subscriber {contact.FirstName} {contact.LastName} makes another call"));
                    }
                    else
                    {

                        Message?.Invoke(new ATSEventArgsMessage($"Call in progress {DateTime.Now} - " +
                                                                $"subscriber {contact.FirstName} {contact.LastName}"));
                    }
                }
                else
                {
                    Message?.Invoke(new ATSEventArgsMessage($"Connection with {contact.FirstName} {contact.LastName} failed." +
                                                        " The subscriber's device is turned off or is out of network range."));
                }
            }
            public void GetAllCalls(Clients client) 
            {
                var orderedOfDay = from i in SimCard.InformationOfTheCall
                    orderby i.Item2
                    select i;

                foreach (var get in orderedOfDay)
                {
                    Console.WriteLine($"{client.FirstName} {client.LastName} call date { get.Item2} - call duration {get.Item1} minutes ");
                }
                SimCard.InformationOfTheCall.Clear();
            }
            
    }
}
