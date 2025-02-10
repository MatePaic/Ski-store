import { createSlice } from "@reduxjs/toolkit";
import { ProductParams } from "../../app/models/productParams";

const initialState: ProductParams = {
    pageNumber: 1,
    pageSize: 8,
    types: [],
    brands: [],
    search: '',
    sort: ''
}

export const catalogSlice = createSlice({
    name: 'catalogSlice',
    initialState,
    reducers: {
        setPageNumber(state, action) {
            state.pageNumber = action.payload;
        },
        setPageSize(state, action) {
            state.pageSize = action.payload;
        },
        setSort(state, action) {
            state.sort = action.payload;
            state.pageNumber = 1;
        },
        setTypes(state, action) {
            state.types = action.payload;
            state.pageNumber = 1;
        },
        setBrands(state, action) {
            state.brands = action.payload;
            state.pageNumber = 1;
        },
        setSearch(state, action) {
            state.search = action.payload;
            state.pageNumber = 1;
        },
        resetParams() {
            return initialState;
        }
    }
});

export const { 
    setPageSize,
    setPageNumber,
    setSort,
    setTypes,
    setBrands,
    setSearch,
    resetParams
} = catalogSlice.actions;