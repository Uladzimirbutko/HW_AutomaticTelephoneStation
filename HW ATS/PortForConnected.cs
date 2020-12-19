using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace HW_ATS
{
    public class PortForConnected
    {

         // список клиентов

        public event Action<ATSEventArgsMessage> PortConnectingEvent;

        public int minutes;
        public string dateCall;

        public Dictionary<Clients, long> ClientsDictionary = new Dictionary<Clients, long>();
        public (int, string) CallTimeAndDate()   // дата и длительность звонка
        {
            Random getNumber = new Random();
            minutes = getNumber.Next(1, 60);
            var getDateCall = RandomDateTime.Next();
            dateCall = getDateCall.ToString();
            (int, string) result = (minutes, dateCall);
            return result;
        }
        public void ClientConnectionPort(Clients client1, Clients client2, Clients client3 = null) // порт соединения клиентов
        {

            long number1 = ClientsDictionary[client1];
            long number2 = ClientsDictionary[client2];


            if (client1 == null)
            {
                PortConnectingEvent?.Invoke (new ATSEventArgsMessage($"There is nothing to connect to"));
            }
            else if (client2 != null && client3 == null)
            {
                if (ClientsDictionary.Any(i => i.Value == number1) && 
                     client1.Port.IsTheSimCardConnected && 
                     ClientsDictionary.Any(i => i.Value == number2) && 
                     client2.Port.IsTheSimCardConnected) 
                {
                    client1.Port.Call1(client2);  //эмуляция 1
                    client2.Port.Call2(client1); //эмуляция 2

                }
                else
                {
                    PortConnectingEvent?.Invoke(new ATSEventArgsMessage($"The subscriber you are calling is temporarily unavailable"));
                }
                
            }
            else if (client3 != null)
            {
                var number3 = ClientsDictionary[client3];
                if (ClientsDictionary.Any(i => i.Value == number1) && 
                    client1.Port.IsTheSimCardConnected && 
                    ClientsDictionary.Any(i => i.Value == number2) && 
                    client2.Port.IsTheSimCardConnected && 
                    ClientsDictionary.Any(i => i.Value == number3) &&
                    client3.Port.IsTheSimCardConnected) 
                {
                    client1.Port.Call1(client2);
                    client3.Port.Call2(client2);
                    PortConnectingEvent?.Invoke(new ATSEventArgsMessage(
                        $"{client3.FirstName} {client3.LastName} trying to call {client1.FirstName} {client1.LastName} but the line is busy"));
                }
                else
                {
                    PortConnectingEvent?.Invoke( new ATSEventArgsMessage($"The subscriber you are calling is temporarily unavailable"));
                }

                Console.WriteLine($"Call ended, call duration { CallTimeAndDate().Item1}");
            }
        }
        public void MessagePort( ATSEventArgsMessage e)
        {
            Console.WriteLine(e.message);
        }

    }
}