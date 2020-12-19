namespace HW_ATS
{
    public interface ITariffSuper
    {
        public double CallCost(Clients callCost); // стоимость звонка
        public void PhonePaymentPerMonth(Clients payPhone, ITariffSuper tariffSuper); // оплата звонка за месяц использования 
    }
}