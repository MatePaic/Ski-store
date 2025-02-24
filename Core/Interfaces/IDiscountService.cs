using Core.Entities;

namespace Core.Interfaces
{
    public interface IDiscountService
    {
        Task<AppCoupon?> GetCouponFromPromoCode(string code);
        Task<decimal> CalculateDiscountFromAmount(AppCoupon appCoupon, decimal amount, 
            bool removeDiscount=false);
    }
}
