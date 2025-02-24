import { Item } from "../../app/models/shoppingCart";
import { useClearShoppingCartMutation, useFetchShoppingCartQuery } from "../../features/shoppingCart/shoppingCartApi";

export const useShoppingCart = () => {
    const {data: shoppingCart} = useFetchShoppingCartQuery();
    const [clearShoppingCart] = useClearShoppingCartMutation();
    const subtotal = shoppingCart?.items.reduce((sum: number, item: Item) => sum + item.quantity * item.price, 0) ?? 0;
    const deliveryFee = subtotal > 10000 ? 0 : 500;
    let discount = 0;

    if (shoppingCart?.coupon) {
        if (shoppingCart.coupon.amountOff) {
            discount = shoppingCart.coupon.amountOff
        } else if (shoppingCart.coupon.percentOff) {
            discount = Math.round((subtotal * (shoppingCart.coupon.percentOff / 100)) * 100) / 100;
        }
    }

    const total = Math.round((subtotal - discount + deliveryFee) * 100) / 100;

    return {shoppingCart, subtotal, deliveryFee, discount, total, clearShoppingCart}
}