import { Divider, Box, Button, Typography } from '@mui/material';
import React, { useContext } from 'react';
import { AppContext } from './App';
import TranslationJobList from './TranslationJobList';
import TranslatorList from './TranslatorList';

function MainOverview() {
    const appContext = useContext(AppContext);

    return (
        <Box sx={{ width: '100%' }}>
            <Box sx={{ m: 2 }} >
                <TranslatorList></TranslatorList>
            </Box>
            <Divider />
            <Box sx={{ m: 2 }} >
                <TranslationJobList></TranslationJobList>
            </Box>
        </Box >

    );
}

export default MainOverview;