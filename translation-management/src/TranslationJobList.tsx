import { Box, Paper, Table, TableBody, TableCell, TableRow, TableContainer, Button, TableFooter, TableHead, TablePagination, Typography, Link, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, TextField } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import React, { useContext } from 'react';
import { AppContext } from './App';

const TranslationJobList = () => {
    const appContext = useContext(AppContext);
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [dialogOpen, setDialogOpen] = React.useState(false);
    const [jobCustomerName, setJobCustomerName] = React.useState('');
    const [jobOriginalContent, setJobOriginalContent] = React.useState('');

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
                    customerName: jobCustomerName,
                    originalContent: jobOriginalContent
                })
            };
            fetch(`${appContext.apiUrl}/TranslationJob`, requestOptions)
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
            fetch(`${appContext.apiUrl}/TranslationJob/${id}`, requestOptions)
                .then(res => {
                    window.location.reload();
                })
        }
        fetchDelete();
    }

    return (
        <Box sx={{ width: '100%' }}>
            <Typography variant="h2" gutterBottom>
                Jobs ({appContext.translationJobs.length}):
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
                    <TableHead>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>Customer Name</TableCell>
                            <TableCell>Original Content</TableCell>
                            <TableCell>Translated Content</TableCell>
                            <TableCell>Translator</TableCell>
                            <TableCell>Price</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {(rowsPerPage > 0
                            ? appContext.translationJobs.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                            : appContext.translationJobs
                        ).map((job: any) => {
                            let status = '';
                            if (job.status == 1) status = 'New'
                            else if (job.status == 2) status = 'InProgress'
                            else if (job.status == 3) status = 'Completed'

                            return (
                                <TableRow
                                    key={job.id}
                                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                >
                                    <TableCell component="th" scope="row">
                                        {job.id}
                                    </TableCell>
                                    <TableCell>
                                        <Link href={`/translationjob/${job.id}`} color="inherit">
                                            {job.customerName}
                                        </Link>
                                    </TableCell>
                                    <TableCell>{job.originalContent}</TableCell>
                                    <TableCell>{job.translatedContent}</TableCell>
                                    <TableCell>{job.translator?.name}</TableCell>
                                    <TableCell>{job.price}</TableCell>
                                    <TableCell>{status}</TableCell>
                                    <TableCell>
                                        <Button onClick={() => { handleDelete(job.id) }} variant="outlined" startIcon={<DeleteIcon />}>
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
                                count={appContext.translationJobs.length}
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
                <Button variant="contained" onClick={handleClickAdd}>Add New Job</Button>
            </Box>
            <Dialog open={dialogOpen} onClose={handleClose}>
                <DialogTitle>Translation Jobs</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Add new Job
                    </DialogContentText>
                    <TextField value={jobCustomerName} onChange={(ev) => setJobCustomerName(ev.target.value)} autoFocus required margin="dense" id="name" type="text" label="Customer Name" fullWidth variant="standard" />
                    <TextField value={jobOriginalContent} onChange={(ev) => setJobOriginalContent(ev.target.value)} required margin="dense" id="rate" type="text" label="Original Content" fullWidth variant="standard" />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button onClick={handleAdd} variant="contained">Create</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}

export default TranslationJobList;