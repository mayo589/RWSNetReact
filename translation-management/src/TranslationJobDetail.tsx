import { Box, Grid, Typography, TextField, MenuItem, Button } from '@mui/material';
import React, { useContext } from 'react';
import { useParams } from 'react-router-dom';
import { AppContext } from './App';

const TranslationJobDetail = () => {
    const appContext = useContext(AppContext);
    let { id } = useParams();
    const [jobCustomerName, setJobCustomerName] = React.useState('');
    const [jobTranslatedContent, setJobTranslatedContent] = React.useState('');
    const [jobOriginalContent, setJobOriginalContent] = React.useState('');
    const [jobPrice, setJobPrice] = React.useState(0);
    const [jobStatus, setJobStatus] = React.useState('');

    React.useEffect(() => {
        async function fetchJob() {
            const res = await fetch(`${appContext.apiUrl}/TranslationJob/${id}`);
            res
                .json()
                .then(res => {
                    setJobCustomerName(res.customerName);
                    setJobTranslatedContent(res.translatedContent);
                    setJobOriginalContent(res.originalContent);
                    setJobPrice(res.price);

                    let status = '';
                    if (res.status === 1) status = 'New';
                    else if (res.status === 2) status = 'InProgress';
                    else if (res.status === 3) status = 'Completed';
                    setJobStatus(status);
                })
        }
        fetchJob();
    }, []);


    const handleClickSave = () => {
        async function fetchAdd() {
            const requestOptions = {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    id: id,
                    customerName: jobCustomerName,
                    translatedContent: jobTranslatedContent,
                })
            };
            fetch(`${appContext.apiUrl}/Translator/${id}`, requestOptions)
                .then(res => {
                    window.location.reload();
                })
        }
        fetchAdd();
    }

    return (
        <Box sx={{ width: '95%' }}>
            <Typography variant="h2" gutterBottom>
                Translation Job Detail page:
            </Typography>
            <TextField value={jobCustomerName} onChange={(ev) => setJobCustomerName(ev.target.value)} required margin="dense" id="name" type="text" label="Name" fullWidth />
            <TextField value={jobTranslatedContent} onChange={(ev) => setJobTranslatedContent(ev.target.value)} required margin="dense" id="rate" type="text" label="Translated Content" fullWidth />
            <TextField value={jobOriginalContent} InputProps={{ readOnly: true }} disabled margin="dense" id="card" type="text" label="Original Content" fullWidth />
            <TextField value={jobPrice} InputProps={{ readOnly: true }} disabled margin="dense" id="card" type="text" label="Price" fullWidth />
            <TextField value={jobStatus} InputProps={{ readOnly: true }} disabled margin="dense" id="card" type="text" label="Status" fullWidth />

            <Box sx={{ mx: 0, my: 2 }} >
                <Button variant="contained" onClick={handleClickSave}>Save</Button>
            </Box>
        </Box>
    );
}

export default TranslationJobDetail;
