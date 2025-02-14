import { TableContainer, Table, TableBody, TableRow, TableCell, Box, Typography, TableHead } from "@mui/material";
import { currencyFormat } from "../../../lib/util";
import { Item } from "../../models/shoppingCart";
import { OrderItem } from "../../models/order";

type Props = {
    items: Item[] | OrderItem[]
}

export default function MappingItems({items}: Props) {
    
    return (
        <>
            <TableContainer>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell align="right" sx={{p: 4}}>Brand</TableCell>
                            <TableCell align="right" sx={{p: 4}}>Type</TableCell>
                            <TableCell align="right" sx={{p: 4}}>Quantity</TableCell>
                            <TableCell align="right" sx={{p: 4}}>Price</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {items.map((item) => (
                            <TableRow key={item.productId}
                                sx={{borderBottom: "1px solid rgba(224, 224, 224, 1)"}}>
                                <TableCell sx={{py: 4}}>
                                    <Box display="flex" gap={3} alignItems="center">
                                        <img
                                            src={item.pictureUrl}
                                            alt={item.name}
                                            style={{width: 40, height: 40}}
                                        />
                                        <Typography>
                                            {item.name}
                                        </Typography>
                                    </Box>
                                </TableCell>
                                <TableCell align="right" sx={{p: 4}}>
                                    {item.brand}
                                </TableCell>
                                <TableCell align="right" sx={{p: 4}}>
                                    {item.type}
                                </TableCell>
                                <TableCell align="right" sx={{p: 4}}>
                                    x {item.quantity}
                                </TableCell>
                                <TableCell align="right" sx={{p: 4}}>
                                    {currencyFormat(item.price)}
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    )
}