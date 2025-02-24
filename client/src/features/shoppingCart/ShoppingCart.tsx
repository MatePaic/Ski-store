import { Grid2, Typography } from "@mui/material";
import { useFetchShoppingCartQuery } from "./shoppingCartApi"
import ShoppingCartItem from "./ShoppingCartItem";
import OrderSummary from "../../app/shared/components/OrderSummary";

export default function ShoppingCart() {
    const {data, isLoading} = useFetchShoppingCartQuery();
    
    if (isLoading) return <Typography>Loading cart...</Typography>

    if (!data || data.items.length === 0) return <Typography variant="h3">Your cart is empty</Typography>
    
    return (
        <Grid2 container spacing={2}>
            <Grid2 size={8}>
                {data.items.map(item => (
                    <ShoppingCartItem item={item} key={item.productId} />
                ))}
            </Grid2>
            <Grid2 size={4}>
                <OrderSummary />
            </Grid2>
        </Grid2>
    )
}