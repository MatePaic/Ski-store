using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class DiscountService : IDiscountService
{
    public DiscountService(IConfiguration config)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
    }

    public async Task<AppCoupon?> GetCouponFromPromoCode(string code)
    {
        var promotionService = new PromotionCodeService();

        var options = new PromotionCodeListOptions
        {
            Code = code
        };

        var promotionCodes = await promotionService.ListAsync(options);

        var promotionCode = promotionCodes.FirstOrDefault();

        if (promotionCode != null && promotionCode.Coupon != null)
        {
            return new AppCoupon
            {
                Name = promotionCode.Coupon.Name,
                AmountOff = promotionCode.Coupon.AmountOff,
                PercentOff = promotionCode.Coupon.PercentOff,
                CouponId = promotionCode.Coupon.Id,
                PromotionCode = promotionCode.Code
            };
        }

        return null;
    }

    public async Task<decimal> CalculateDiscountFromAmount(AppCoupon appCoupon, decimal amount,
        bool removeDiscount = false)
    {
        var couponService = new CouponService();

        var coupon = await couponService.GetAsync(appCoupon.CouponId);

        if (coupon.AmountOff.HasValue && !removeDiscount)
        {
            return (long)coupon.AmountOff;
        }
        else if (coupon.PercentOff.HasValue && !removeDiscount)
        {
            var discount =  (long)Math.Round(amount * (coupon.PercentOff.Value / 100),
                MidpointRounding.AwayFromZero);

            return discount;
        }

        return 0;
    }
}