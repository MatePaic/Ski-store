import { Box, Divider, Typography } from "@mui/material";
import { ConfirmationToken } from "@stripe/stripe-js";
import { useShoppingCart } from "../../lib/hooks/useShoppingCart";
import MappingItems from "../../app/shared/components/MappingItems";

type Props = {
    confirmationToken: ConfirmationToken | null;
}

export default function Review({confirmationToken}: Props) {
    const { shoppingCart } = useShoppingCart();
    
    if (!shoppingCart) return <Typography variant="h5">There is no shopping cart.</Typography>

    const addressString = () => {
        if (!confirmationToken?.shipping) return '';
        
        const {name, address} = confirmationToken.shipping;
        
        return `${name}, ${address?.line1}, ${address?.city}, ${address?.state},
            ${address?.postal_code}, ${address?.country}`
    }

    const paymentString = () => {
        if (!confirmationToken?.payment_method_preview.card) return '';
        const { card } = confirmationToken.payment_method_preview;
        
        return `${card.brand.toUpperCase()}, **** **** **** ${card.last4},
            Exp: ${card.exp_month}/${card.exp_year}`
    }

    return (
        <div>
            <Box mt={4} width="100%">
                <Typography variant="h6" fontWeight="bold">
                    Billing and delivery information
                </Typography>
                <dl>
                    <Typography component="dt" fontWeight="medium">
                        Shipping address
                    </Typography>
                    <Typography component="dd" mt={1} color="textSecondary">
                        {addressString()}
                    </Typography>

                    <Typography component="dt" fontWeight="medium">
                        Payment details
                    </Typography>
                    <Typography component="dd" mt={1} color="textSecondary">
                        {paymentString()}
                    </Typography>
                </dl>
            </Box>

            <Box mt={6} mx="auto">
                <Divider />
                <MappingItems items={shoppingCart.items}/>
            </Box>
        </div>
    )
}