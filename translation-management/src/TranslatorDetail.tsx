import { Box, Grid, Typography, TextField, MenuItem, Button, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, } from '@mui/material';
import React, { useContext } from 'react';
import { useParams } from 'react-router-dom';
import { AppContext } from './App';

const statuses = [
    {
        value: 1,
        label: 'Applicant',
    },
    {
        value: 2,
        label: 'Certified',
    },
    {
        value: 3,
        label: 'Deleted',
    },
];

const TranslatorDetail = () => {
    const appContext = useContext(AppContext);
    let { id } = useParams();
    const [translatorName, setTranslatorName] = React.useState('');
    const [translatorCard, setTranslatorCard] = React.useState('');
    const [translatorRate, setTranslatorRate] = React.useState(0);
    const [translatorStatus, setTranslatorStatus] = React.useState(0);
    const [dialogOpen, setDialogOpen] = React.useState(false);
    const [availableJobs, setAvailableJobs] = React.useState([]);
    const [translationJobId, setTranslationJobId] = React.useState(0);

    React.useEffect(() => {
        async function fetchTranslator() {
            const res = await fetch(`${appContext.apiUrl}/Translator/${id}`);
            res
                .json()
                .then(res => {
                    setTranslatorName(res.name);
                    setTranslatorCard(res.creditCardNumber);
                    setTranslatorRate(res.hourlyRate);
                    setTranslatorStatus(res.status);
                })
        }
        fetchTranslator();
    }, []);

    React.useEffect(() => {
        async function fetchAvailableJobs() {
            const res = await fetch(`${appContext.apiUrl}/TranslationJob`);
            res
                .json()
                .then(res => {
                    setAvailableJobs(res.filter((j: any) => j.translatorId == null));
                })
        }
        fetchAvailableJobs();
    }, []);

    const handleAssign = () => {
        async function fetchAssign() {
            const requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    id: id,
                    name: translatorName,
                    hourlyRate: translatorRate,
                    creditCardNumber: translatorCard,
                    status: translatorStatus,
                })
            };
            fetch(`${appContext.apiUrl}/TranslationJob/AssignTranslator/?id=${translationJobId}&translatorId=${id}`, requestOptions)
                .then(res => {
                    window.location.reload();
                })
        }
        fetchAssign();
    };

    const handleClose = () => {
        setDialogOpen(false);
    };

    const handleClickSave = () => {
        async function fetchAdd() {
            const requestOptions = {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    id: id,
                    name: translatorName,
                    hourlyRate: translatorRate,
                    creditCardNumber: translatorCard,
                    status: translatorStatus,
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
                Translator Detail page:
            </Typography>
            <TextField value={translatorName} onChange={(ev) => setTranslatorName(ev.target.value)} required margin="dense" id="name" type="text" label="Name" fullWidth />
            <TextField value={translatorCard} onChange={(ev) => setTranslatorCard(ev.target.value)} required margin="dense" id="card" type="text" label="Credit Card Number" fullWidth />
            <TextField value={translatorRate} onChange={(ev) => setTranslatorRate(Number(ev.target.value))} required margin="dense" id="rate" type="text" label="Hourly Rate" fullWidth />
            <TextField value={translatorStatus} onChange={(ev) => setTranslatorStatus(Number(ev.target.value))} select margin="dense" label="Status" fullWidth>
                {statuses.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                        {option.label}
                    </MenuItem>
                ))}
            </TextField>
            <Box sx={{ mx: 0, my: 2 }} >
                <Button variant="contained" onClick={handleClickSave}>Save</Button>
                <Button variant="contained" onClick={() => setDialogOpen(true)}>Assign Job</Button>
            </Box>
            <Dialog open={dialogOpen} onClose={handleClose}>
                <DialogTitle>Jobs</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Assign job for translator
                    </DialogContentText>
                    <TextField value={translationJobId} onChange={(ev) => setTranslationJobId(Number(ev.target.value))} select margin="dense" label="Status" fullWidth>
                        {availableJobs.map((option: any) => (
                            <MenuItem key={option.id} value={option.id}>
                                {option.customerName}
                            </MenuItem>
                        ))}
                    </TextField>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button onClick={handleAssign} variant="contained">Assign</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}

export default TranslatorDetail;
