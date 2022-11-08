import { Backdrop, Box, CircularProgress, Typography } from '@mui/material';
import React, { useContext } from 'react';
import { AppContext } from './App';

function AppHeader() {
    const appContext = useContext(AppContext);

    return (
        <Box sx={{ width: '100%' }}>
            <Typography variant="h1" gutterBottom>
                RWS React App
            </Typography>
            <Typography variant="body1" gutterBottom>
                This apps is used for Translators and Translator Job management
            </Typography>
        </Box >

    );
}

export default AppHeader;