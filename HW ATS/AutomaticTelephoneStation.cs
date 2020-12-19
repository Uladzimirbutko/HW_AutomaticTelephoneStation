using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HW_ATS
{


    public class AutomaticTelephoneStation : PortForConnected, ITariffUnlimited, ITariffSuper
    {

        public event Action<ATSEventArgsMessage> Message;
        public long Number { get; set; }
        private double coefficient;

         public List<(int, DateTime)> InformationOfTheCall = new List<(int, DateTime)>();
         public List<double> CostCall = new List<double>();


        public void GetNumber() // генератор номеров
        {
            string PrefixCode = "8099";
            Random NumberGeneration = new Random();
            Number = long.Parse(PrefixCode + NumberGeneration.Next(1000000, 8000000));
        }

        public void ContractWithClient(Clients person) // Заключаем контракт
        {
            if (ClientsDictionary.All(i =>
                i.Key.FirstName != person.FirstName &
                i.Key.LastName != person.LastName &
                person.Age >= 18))
            {
                GetNumber();
                ClientsDictionary.Add(person, Number);
                Message?.Invoke(new ATSEventArgsMessage(
                    $"The contract with the {person.FirstName} {person.LastName} has been successfully. Phone number is {Number}"));
            }
            else if (person.Age < 18)
            {
                Message?.Invoke(new ATSEventArgsMessage(
                    $"The contract with the {person.FirstName} {person.LastName} has not been concluded. Client under 18 years old."));
            }
            else
            {
                Message?.Invoke(new ATSEventArgsMessage($"This client already exists."));
            }

        }

        public (long, bool ) GetClient(Clients person) // Ищeм клиента в сети
        {
            long number = 0;
            if (ClientsDictionary.Any(i => i.Key == person) && person != null)
            {
                number = ClientsDictionary[person];
                return (number, true);
            }
            else
            {
                Message?.Invoke(new ATSEventArgsMessage($"No contract with this client"));
                return (number, false);
            }
        }

        double ITariffUnlimited.CallCost(Clients callCost) //генерация 20 звонков по тарифу безлимит
        {
            double result = 50; //абон плата
            Console.WriteLine(
                $"Dear {callCost.FirstName} {callCost.LastName}, your 'UNLIMITED' tariff includes 400 free minutes per month within the network. " +
                "Over 400 minutes are charged at a double rate. Subscription fee of 50$ per month.");
            if (callCost != default)
            {
                int minutesPerMonth = 0;
                for (int i = 0; i < 20; i++)
                {
                    
                    InformationOfTheCall.Add(CallTimeAndDate());
                    minutesPerMonth += InformationOfTheCall[i].Item1;
                }
                
                if (minutesPerMonth > 400)
                {
                    coefficient = 2;
                    int minutesAboveTheNorm = minutesPerMonth - 400;
                    result = minutesAboveTheNorm * coefficient + result;
                    Message?.Invoke(new ATSEventArgsMessage($"You spoke for {minutesPerMonth} minutes per month." +
                                                            $"Your cost per month: {result}$"));
                }
                else
                {
                    Message?.Invoke(
                        new ATSEventArgsMessage(
                            $"You spoke for less than 400 minutes. Your cost per month {result}$ "));
                }
            }
            else
            {
                Message?.Invoke(new ATSEventArgsMessage($"The client does not exist."));
                result = 0;
            }

            return result;
        }



        void ITariffUnlimited.PhonePaymentPerMonth(Clients payPhone, ITariffUnlimited tariffUnlimited) // оплата за месяц тариф беспл. 400 минут
        {
            double cost = tariffUnlimited.CallCost(payPhone);
            Message?.Invoke(cost != 0
                ? new ATSEventArgsMessage($"You paid {cost}$.")
                : new ATSEventArgsMessage($"Payment is not possible."));
        }


        double ITariffSuper.CallCost(Clients callCost) // генерация звонков по тарифу супер
        {
            double result = 20; //абон плата
            Console.WriteLine(
                $"Dear {callCost.FirstName} {callCost.LastName}, your 'SUPER' tariff includes cheap calls within the network.Subscription fee of 50$ per month.");
            if (callCost != default)
            {
                double minutesPerMonth = 0;

                for (int i = 0; i < 20; i++)
                {
                    InformationOfTheCall.Add(CallTimeAndDate());
                    minutesPerMonth += InformationOfTheCall[i].Item1;
                }
                coefficient = 1.2;
                result = minutesPerMonth * coefficient + result;
                Message?.Invoke(new ATSEventArgsMessage($"You spoke for {minutesPerMonth} minutes per month." +
                                                        $"Your cost per month: {result}$"));
            }
            else
            {
                Message?.Invoke(new ATSEventArgsMessage($"The client does not exist."));
                result = 0;
            }

            return result;
        }

        void ITariffSuper.PhonePaymentPerMonth(Clients payPhone, ITariffSuper tariffSuper) //оплата тарифа супер
        {
            double cost = tariffSuper.CallCost(payPhone);
            Message?.Invoke(cost != 0
                ? new ATSEventArgsMessage($"You paid {cost}$.")
                : new ATSEventArgsMessage($"Payment is not possible."));

        }

    }
}

