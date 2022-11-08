import React from 'react';
import {
  BrowserRouter,
  Route,
  Routes,
} from 'react-router-dom';
import { Box } from '@mui/material';
import AppHeader from './AppHeader';
import MainOverview from './MainOverview';
import TranslatorDetail from './TranslatorDetail';
import TranslationJobDetail from './TranslationJobDetail';

const apiUrl = "http://localhost:7729/api";

export const AppContext = React.createContext<
  {
    apiUrl: string,
    tranlators: {
      id: number, name: string, hourlyRate: string, status: number, creditCardNumber: string
    }[],
    translationJobs: {
      id: number, customerName: string, status: number, originalContent: string, translatedContent: string, price: number
    }[]
  }>(
    {
      apiUrl: "",
      tranlators: [],
      translationJobs: []
    });

function App() {
  const [translators, setTranslators] = React.useState<{ id: number, name: string, hourlyRate: string, status: number, creditCardNumber: string }[]>([]);
  const [translationJobs, setTranslationJobs] = React.useState<{ id: number, customerName: string, status: number, originalContent: string, translatedContent: string, price: number }[]>([]);

  React.useEffect(() => {
    async function fetchTranslators() {
      const res = await fetch(`${apiUrl}/Translator`);
      res
        .json()
        .then(res => {
          setTranslators(res);
        })
    }
    fetchTranslators();
  }, []);

  React.useEffect(() => {
    async function fetchTranslators() {
      const res = await fetch(`${apiUrl}/TranslationJob`);
      res
        .json()
        .then(res => {
          setTranslationJobs(res);
        })
    }
    fetchTranslators();
  }, []);

  return (
    <AppContext.Provider value={{ apiUrl: apiUrl, tranlators: translators, translationJobs: translationJobs }}>
      <Box sx={{ width: '100%' }}>
        <AppHeader></AppHeader>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<MainOverview />} />
            <Route path="/translator/:id" element={<TranslatorDetail />} />
            <Route path="/translationjob/:id" element={<TranslationJobDetail />} />
          </Routes>
        </BrowserRouter>
      </Box >
    </AppContext.Provider>
  );
}

export default App;