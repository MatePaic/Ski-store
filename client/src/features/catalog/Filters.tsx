import { Box, Button, Paper  } from "@mui/material";
import RadioButtonsGroup from "../../app/shared/components/RadioButtonsGroup";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { resetParams, setBrands, setSort, setTypes } from "./catalogSlice";
import CheckboxButtons from "../../app/shared/components/CheckboxButtons";
import Search from "./Search";

const sortOptions = [
    { value: 'name', label: 'Alphabetical' },
    { value: 'priceDesc', label: 'Price: High to low' },
    { value: 'priceAsc', label: 'Price: Low to high' },
]

type Props = {
    filtersData: {
        brands: string[];
        types: string[];
    }
}

export default function Filters({filtersData: data}: Props) {
    const {sort, brands, types} = useAppSelector(state => state.catalog);
    const dispatch = useAppDispatch();

    return (
        <Box display='flex' flexDirection='column' gap={3}>
            <Paper>
                <Search />
            </Paper>
            <Paper sx={{p: 3}}>
                <RadioButtonsGroup
                    selectedValue={sort}
                    options={sortOptions}
                    onChange={e => dispatch(setSort(e.target.value))}
                />
            </Paper>
            <Paper sx={{p: 3}}>
                <CheckboxButtons
                    items={data.brands}
                    checked={brands}
                    onChange={(items: string[]) => dispatch(setBrands(items))}
                />
            </Paper>
            <Paper sx={{p: 3}}>
                <CheckboxButtons
                    items={data.types}
                    checked={types}
                    onChange={(items: string[]) => dispatch(setTypes(items))}
                />
            </Paper>
            <Button onClick={() => dispatch(resetParams())}>
                Reset filter
            </Button>
        </Box>
    )
}