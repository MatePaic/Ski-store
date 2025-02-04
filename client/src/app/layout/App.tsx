import { useEffect, useState } from "react"
import { Product } from "../models/product";
import { Box, Container, createTheme, CssBaseline, ThemeProvider } from "@mui/material";
import Catalog from "../../features/catalog/Catalog";
import Navbar from "./Navbar";

function App() {
  const [products, setProducts] = useState<Product[]>([]);
  const [darkMode, setdarkMode] = useState<boolean>(false);

  const palleteType = darkMode ? 'dark' : 'light';
  const theme = createTheme({
    palette: {
      mode: palleteType,
      background: {
        default: (palleteType === 'light') ? '#eaeaea' : '#121212'
      }
    }
  })

  const toggleDarkMode = () => {
    setdarkMode(!darkMode);
  }
  
  useEffect(() => {
    fetch('https://localhost:5001/api/products')
      .then(response => response.json())
      .then(data => setProducts(data.data))
  }, [])

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Navbar toggleDarkMode={toggleDarkMode} darkMode={darkMode} />
      <Box
        sx={{
          background: darkMode
            ? 'radial-gradient(circle, #1e3aBa, #111B27)'
            : 'radial-gradient(circle, #baecf9, #f0f9ff)',
          py: 6
        }}>
        <Container maxWidth='xl' sx={{mt: 8}}>
          <Catalog products={products} />
        </Container>
      </Box>
    </ThemeProvider>
  )
}

export default App
