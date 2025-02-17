import { configureStore } from "@reduxjs/toolkit";
import { counterSlice } from "../../features/contact/counterReducer";
import { useDispatch, useSelector } from "react-redux";
import { catalogApi } from "../../features/catalog/catalogApi";
import { uiSlice } from "../layout/uiSlice";
import { errorApi } from "../../features/about/errorApi";
import { shoppingCartApi } from "../../features/shoppingCart/shoppingCartApi";
import { catalogSlice } from "../../features/catalog/catalogSlice";
import { accountApi } from "../../features/account/accountApi";
import { checkoutApi } from "../../features/checkout/checkoutApi";
import { orderApi } from "../../features/orders/orderApi";
import { adminApi } from "../../features/admin/adminApi";

// export function configureTheStore() {
//     return legacy_createStore(counterReducer);
// }

export const store = configureStore({
    reducer: {
        [catalogApi.reducerPath]: catalogApi.reducer,
        [shoppingCartApi.reducerPath]: shoppingCartApi.reducer,
        [errorApi.reducerPath]: errorApi.reducer,
        [accountApi.reducerPath]: accountApi.reducer,
        [checkoutApi.reducerPath]: checkoutApi.reducer,
        [orderApi.reducerPath]: orderApi.reducer,
        [adminApi.reducerPath]: adminApi.reducer,
        counter: counterSlice.reducer,
        ui: uiSlice.reducer,
        catalog: catalogSlice.reducer
    },
    middleware: (getDefaultMiddlewere) => 
        getDefaultMiddlewere().concat(
            catalogApi.middleware,
            errorApi.middleware,
            shoppingCartApi.middleware,
            accountApi.middleware,
            checkoutApi.middleware,
            orderApi.middleware,
            adminApi.middleware
        )
});

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export const useAppDispatch = useDispatch.withTypes<AppDispatch>()
export const useAppSelector = useSelector.withTypes<RootState>()

