namespace HW_ATS
{
    public interface ITariffUnlimited   
    {
        public double CallCost(Clients callCost); // стоимость звонка
        public void PhonePaymentPerMonth(Clients payPhone, ITariffUnlimited tariffUnlimited); // оплата звонка за месяц использования
        
    }
}