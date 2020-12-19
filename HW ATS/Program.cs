using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace HW_ATS
{
    class Program
    {
        static void Main(string[] args)
        {
            //читаем клиентов с файла
            var file = "ClientsList.txt";
            List<Clients> clientsLists = File.ReadAllLines(file)
                                             .Select(Clients.ParseFile)
                                             .ToList();


            var automaticTelephoneStation = new AutomaticTelephoneStation();
            //подписываемся на евенты
            automaticTelephoneStation.Message += automaticTelephoneStation.MessagePort; 
            automaticTelephoneStation.PortConnectingEvent += automaticTelephoneStation.MessagePort;

            //создаем клиентов из файла
            var client1 = clientsLists[0]; //   Золотухин|Шолох|Xiaomi|12.12.1995|25
            var client2 = clientsLists[1]; //   Каримбек|Шведов|Samsung|14.01.2000|20
            var client3 = clientsLists[2]; //   Лапин|Игнат|Huawei|18.06.1999|21

            //добавляем номера клиентам
            client1.Port = new ConnectingASimCard<AutomaticTelephoneStation>(automaticTelephoneStation, true);
            client2.Port = new ConnectingASimCard<AutomaticTelephoneStation>(automaticTelephoneStation, true);
            client3.Port = new ConnectingASimCard<AutomaticTelephoneStation>(automaticTelephoneStation, true);

            //подписываемся на евенты
            client1.Port.Message += automaticTelephoneStation.MessagePort; 
            client2.Port.Message += automaticTelephoneStation.MessagePort; 
            client3.Port.Message += automaticTelephoneStation.MessagePort; 

            //заключаем контракты
            automaticTelephoneStation.ContractWithClient(client1);
            automaticTelephoneStation.ContractWithClient(client2);
            automaticTelephoneStation.ContractWithClient(client3);
            Console.WriteLine();

            //добавляем контакты первый клиент это - кому добавляем второго клиента
            client1.Port.AddContact(client1, client2); 
            client1.Port.AddContact(client1, client3);
            client2.Port.AddContact(client2, client1);
            client2.Port.AddContact(client2, client3);
            client3.Port.AddContact(client3, client1);
            client3.Port.AddContact(client3, client2);
            Console.WriteLine();

            //оплачиваем счета и генерируем звонки
            ITariffUnlimited tariffUnlimitedClient1 = client1.Port.SimCard;
            tariffUnlimitedClient1.PhonePaymentPerMonth(client1,tariffUnlimitedClient1);
            Console.WriteLine();

            ITariffSuper tariffSuperClient2 = client2.Port.SimCard;
            tariffSuperClient2.PhonePaymentPerMonth(client2,tariffSuperClient2);
            Console.WriteLine();

            ITariffUnlimited tariffUnlimitedClient3 = client3.Port.SimCard;
            tariffUnlimitedClient3.PhonePaymentPerMonth(client3,tariffUnlimitedClient3);
            Console.WriteLine();

            //смотрим все звонки они почему то одинаковые хз как решить
            client1.Port.GetAllCalls(client1);
            Console.WriteLine();
            client2.Port.GetAllCalls(client2);
            Console.WriteLine();
            client3.Port.GetAllCalls(client3);

            //удаляем по одному контакту
            client1.Port.RemoveContacts(client2);
            client2.Port.RemoveContacts(client3);
            client3.Port.RemoveContacts(client1);
            Console.WriteLine();

            // проверяем есть ли такой контакт если нет - выводится весь список
            client1.Port.GetNumberAddContacts(client3);
            Console.WriteLine();
            client1.Port.GetNumberAddContacts(client2); //этого нет потому что удалили.
            Console.WriteLine();

            //эмулируем звонок c занятой линией
            automaticTelephoneStation.ClientConnectionPort(client1,client2,client3);

        }

    }
}
