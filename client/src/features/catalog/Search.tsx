import { debounce, TextField } from "@mui/material";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { setSearch } from "./catalogSlice";
import { useEffect, useState } from "react";

export default function Search() {
    const {search} = useAppSelector(state => state.catalog);
    const dispatch = useAppDispatch();
    const [term, setTerm] = useState(search);

    useEffect(() => {
        setTerm(search);
    }, [search]);

    const debouncedSearch = debounce(event => {
        dispatch(setSearch(event.target.value));
    }, 500);

    return (
        <TextField
            label='Search products'
            variant="outlined"
            fullWidth
            type="search"
            value={term}
            onChange={e => {
                setTerm(e.target.value);
                debouncedSearch(e);
            }}
        />
  )
}