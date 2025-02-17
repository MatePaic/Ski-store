import { zodResolver } from "@hookform/resolvers/zod";
import { registerSchema, RegisterSchema } from "../../lib/schemas/registerSchema";
import { useRegisterMutation } from "./accountApi";
import { useForm } from "react-hook-form";
import { LockOutlined } from "@mui/icons-material";
import { Container, Paper, Box, Typography, TextField, Button } from "@mui/material";
import { Link } from "react-router-dom";
import { handleApiError } from "../../lib/util";

export default function RegisterForm() {
    const [registerUser] = useRegisterMutation();

    const {register, handleSubmit, setError, formState: {errors, isValid, isLoading}} = useForm<RegisterSchema>({
        mode: 'onTouched',
        resolver: zodResolver(registerSchema)
    });

    const onSubmit = async (data: RegisterSchema) => {
        try {
            await registerUser(data).unwrap();
        } catch (error) {
            handleApiError<RegisterSchema>(error, setError, 
                ['password', 'email', 'firstName', 'lastName']);
        }
    }
    
    return (
        <Container component={Paper} maxWidth='sm' sx={{borderRadius: 3}}>
            <Box display="flex" flexDirection="column" alignItems="center" marginTop="8">
                <LockOutlined sx={{ mt: 3, color: 'secondary.main', fontSize: 40 }} />
                <Typography variant="h5">
                    Register
                </Typography>
                <Box
                    component="form"
                    onSubmit={handleSubmit(onSubmit)}
                    width="100%"
                    display="flex"
                    flexDirection="column"
                    gap={3}
                    marginY={3}
                >
                    <TextField
                        fullWidth
                        label="FirstName"
                        autoFocus
                        {...register('firstName')}
                        error={!!errors.firstName}
                        helperText={errors.firstName?.message}
                    />
                    <TextField
                        fullWidth
                        label="LastName"
                        autoFocus
                        {...register('lastName')}
                        error={!!errors.lastName}
                        helperText={errors.lastName?.message}
                    />
                    <TextField
                        fullWidth
                        label="Email"
                        autoFocus
                        {...register('email')}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />
                    <TextField
                        fullWidth
                        label="Password"
                        type="password"
                        {...register('password')}
                        error={!!errors.password}
                        helperText={errors.password?.message}
                    />
                    <Button disabled={isLoading || !isValid} variant="contained" type="submit">
                        Register
                    </Button>
                    <Typography sx={{textAlign: 'center'}}>
                        Already have an account?
                        <Typography sx={{ml: 2}} component={Link} to='/login' color="primary">
                            Sign in
                        </Typography>
                    </Typography>
                </Box>
            </Box>
        </Container>
    )
}