import { Item } from "../../app/models/shoppingCart";
import { useClearShoppingCartMutation, useFetchShoppingCartQuery } from "../../features/shoppingCart/shoppingCartApi";

export const useShoppingCart = () => {
    const {data: shoppingCart} = useFetchShoppingCartQuery();
    const [clearShoppingCart] = useClearShoppingCartMutation();
    const subtotal = shoppingCart?.items.reduce((sum: number, item: Item) => sum + item.quantity * item.price, 0) ?? 0;
    const deliveryFee = subtotal > 10000 ? 0 : 500;
    const total = subtotal + deliveryFee;

    return {shoppingCart, subtotal, deliveryFee, total, clearShoppingCart}
}