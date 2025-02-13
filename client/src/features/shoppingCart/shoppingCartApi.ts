import { createApi } from "@reduxjs/toolkit/query/react";
import { baseQueryWithErrorHandling } from "../../app/api/baseApi";
import { Item, ShoppingCart } from "../../app/models/shoppingCart";
import { Product } from "../../app/models/product";
import Cookies from 'js-cookie';

function isShoppingCartItem(product: Product | Item): product is Item {
    return (product as Item).quantity !== undefined;
}

export const shoppingCartApi = createApi({
    reducerPath: 'ShoppingCartApi',
    baseQuery: baseQueryWithErrorHandling,
    tagTypes: ['ShoppingCart'],
    endpoints: (builder) => ({
        fetchShoppingCart: builder.query<ShoppingCart, void>({
            query: () => 'shoppingCart',
            providesTags: ['ShoppingCart']
        }),
        addShoppingCartItem: builder.mutation<ShoppingCart, {product: Product | Item, quantity: number}>({
            query: ({ product, quantity }) => {
                const productId = isShoppingCartItem(product) ? product.productId : product.id;
                return {
                    url: `shoppingCart?productId=${productId}&quantity=${quantity}`,
                    method: 'POST'
                }
            },
            onQueryStarted: async ({ product, quantity }, { dispatch, queryFulfilled }) => {
                let isNewShoppingCart = false;
                const patchResult = dispatch(
                    shoppingCartApi.util.updateQueryData('fetchShoppingCart', undefined, (draft) => {
                        const productId = isShoppingCartItem(product) ? product.productId : product.id;

                        if (!draft?.shoppingCartId) isNewShoppingCart = true;

                        if (!isNewShoppingCart) {
                            const existingItem = draft.items.find(item => item.productId === productId);
                            if (existingItem) existingItem.quantity += quantity;
                            else draft.items.push(
                                isShoppingCartItem(product) ? 
                                    product :
                                    {...product, productId: product.id, quantity}
                            );
                        }
                    })
                );

                try {
                    await queryFulfilled;
                    if (isNewShoppingCart) dispatch(shoppingCartApi.util.invalidateTags(['ShoppingCart']));
                } catch (error) {
                    console.log(error);
                    patchResult.undo();
                }
            }
        }),
        removeShoppingCartItem: builder.mutation<void, {productId: number, quantity: number}>({
            query: ({productId, quantity}) => ({
                url: `shoppingCart?productId=${productId}&quantity=${quantity}`,
                method: 'DELETE'
            }),
            onQueryStarted: async ({productId, quantity}, {dispatch, queryFulfilled}) => {
                const patchResult = dispatch(
                    shoppingCartApi.util.updateQueryData('fetchShoppingCart', undefined, (draft) => {
                        const itemIndex = draft.items.findIndex(item => item.productId === productId);
                        if (itemIndex >= 0) {
                            draft.items[itemIndex].quantity -= quantity;
                            if (draft.items[itemIndex].quantity <= 0) {
                                draft.items.splice(itemIndex, 1);
                            }
                        }
                    })
                );

                try {
                    await queryFulfilled
                } catch (error) {
                    console.log(error);
                    patchResult.undo();
                }
            }
        }),
        clearShoppingCart: builder.mutation<void, void>({
            queryFn: () => ({ data: undefined }),
            onQueryStarted: async (_, { dispatch }) => {
                dispatch(
                    shoppingCartApi.util.updateQueryData('fetchShoppingCart', undefined, (draft) => {
                        draft.items = [];
                    })
                );
                Cookies.remove('shoppingCartId');
            }
        })
    })
});

export const {
    useFetchShoppingCartQuery,
    useAddShoppingCartItemMutation,
    useRemoveShoppingCartItemMutation,
    useClearShoppingCartMutation
} = shoppingCartApi;