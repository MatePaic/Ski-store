import { Grid2, Typography } from "@mui/material";
import OrderSummary from "../../app/shared/components/OrderSummary";
import CheckoutStepper from "./CheckoutStepper";
import { loadStripe, StripeElementsOptions } from "@stripe/stripe-js";
import { useFetchShoppingCartQuery } from "../shoppingCart/shoppingCartApi";
import { useEffect, useMemo, useRef } from "react";
import { Elements } from '@stripe/react-stripe-js';
import { useCreatePaymentIntentMutation } from "./checkoutApi";
import { useAppSelector } from "../../app/store/store";

const stripePromise = loadStripe(import.meta.env.VITE_STRIPE_PK);

export default function CheckoutPage() {
    const { data: shoppingCart } = useFetchShoppingCartQuery();
    const [createPaymentIntent, { isLoading }] = useCreatePaymentIntentMutation();
    const created = useRef(false);
    const {darkMode} = useAppSelector(state => state.ui);

    useEffect(() => {
      if (!created.current) createPaymentIntent();
      created.current = true;
    }, [createPaymentIntent])

    const options: StripeElementsOptions | undefined = useMemo(() => {
        if (!shoppingCart?.clientSecret) return undefined;
        return {
          clientSecret: shoppingCart.clientSecret,
          appearance: {
            labels: 'floating',
            theme: darkMode ? 'night' : 'stripe'
          }
        }
    }, [shoppingCart?.clientSecret, darkMode]);

    return (
        <Grid2 container spacing={2}>
            <Grid2 size={8}>
                {!stripePromise || !options || isLoading ? (
                  <Typography variant="h6">Loading checkout...</Typography>
                ) : (
                  <Elements stripe={stripePromise} options={options}>
                      <CheckoutStepper />
                  </Elements>
                )}
            </Grid2>
            <Grid2 size={4}>
                <OrderSummary />
            </Grid2>
        </Grid2>
    )
}