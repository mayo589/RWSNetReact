import { Box, Paper, Table, TableBody, TableCell, Button, TableRow, TableContainer, TableFooter, TableHead, TablePagination, Typography, Link, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, TextField } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import React, { useContext } from 'react';
import { AppContext } from './App';

const TranslatorList = () => {
    const appContext = useContext(AppContext);
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);
    const [dialogOpen, setDialogOpen] = React.useState(false);
    const [translatorName, setTranslatorName] = React.useState('');
    const [translatorRate, setTranslatorRate] = React.useState(0);
    const [translatorCard, setTranslatorCard] = React.useState('');

    const handleChangePage = (
        event: React.MouseEvent<HTMLButtonElement> | null,
        newPage: number,
    ) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    ) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const handleClickAdd = () => {
        setDialogOpen(true);
    };

    const handleClose = () => {
        setDialogOpen(false);
    };

    const handleAdd = () => {
        async function fetchAdd() {
            const requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    name: translatorName,
                    hourlyRate: translatorRate,
                    creditCardNumber: translatorCard
                })
            };
            fetch(`${appContext.apiUrl}/Translator`, requestOptions)
                .then(res => {
                    window.location.reload();
                })
        }
        fetchAdd();
    }

    const handleDelete = (id: number) => {
        async function fetchDelete() {
            const requestOptions = {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' }
            };
            fetch(`${appContext.apiUrl}/Translator/${id}`, requestOptions)
                .then(res => {
                    window.location.reload();
                })
        }
        fetchDelete();
    }

    return (
        <Box sx={{ width: '100%' }}>
            <Typography variant="h2" gutterBottom>
                Translators ({appContext.tranlators.length}):
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
                    <TableHead>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>Hourly Rate</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Credit Card Number</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {(rowsPerPage > 0
                            ? appContext.tranlators.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                            : appContext.tranlators
                        ).map((tranlator: any) => {
                            let status = '';
                            if (tranlator.status == 1) status = 'Applicant'
                            else if (tranlator.status == 2) status = 'Certified'
                            else if (tranlator.status == 3) status = 'Deleted'

                            return (
                                <TableRow
                                    key={tranlator.id}
                                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                >
                                    <TableCell component="th" scope="row">
                                        {tranlator.id}
                                    </TableCell>
                                    <TableCell>
                                        <Link href={`/translator/${tranlator.id}`} color="inherit">
                                            {tranlator.name}
                                        </Link>
                                    </TableCell>
                                    <TableCell>{tranlator.hourlyRate}</TableCell>
                                    <TableCell>{status}</TableCell>
                                    <TableCell>{tranlator.creditCardNumber}</TableCell>
                                    <TableCell>
                                        <Button onClick={() => { handleDelete(tranlator.id) }} variant="outlined" startIcon={<DeleteIcon />}>
                                            Delete
                                        </Button>
                                    </TableCell>
                                </TableRow>
                            )
                        })}
                    </TableBody>
                    <TableFooter>
                        <TableRow>
                            <TablePagination
                                rowsPerPageOptions={[10, 25]}
                                component="div"
                                count={appContext.tranlators.length}
                                rowsPerPage={rowsPerPage}
                                page={page}
                                onPageChange={handleChangePage}
                                onRowsPerPageChange={handleChangeRowsPerPage}
                            />
                        </TableRow>
                    </TableFooter>
                </Table>
            </TableContainer>
            <Box sx={{ mx: 0, my: 2 }} >
                <Button variant="contained" onClick={handleClickAdd}>Add Translator</Button>
            </Box>
            <Dialog open={dialogOpen} onClose={handleClose}>
                <DialogTitle>Translators</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Add new Translator
                    </DialogContentText>
                    <TextField value={translatorName} onChange={(ev) => setTranslatorName(ev.target.value)} autoFocus required margin="dense" id="name" type="text" label="Name" fullWidth variant="standard" />
                    <TextField value={translatorRate} onChange={(ev) => setTranslatorRate(Number(ev.target.value))} required margin="dense" id="rate" type="number" label="Hourly rate" fullWidth variant="standard" />
                    <TextField value={translatorCard} onChange={(ev) => setTranslatorCard(ev.target.value)} required margin="dense" id="name" type="text" label="Credit Card Number" fullWidth variant="standard" />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button onClick={handleAdd} variant="contained">Create</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}

export default TranslatorList;