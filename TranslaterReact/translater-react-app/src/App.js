import React, { useEffect, useState, useCallback } from 'react';
import api from './Api/Api';
import ResultTable from './Components/ResultTable';
import AddKeywordForm from './Components/AddKeywordForm';
import SearchInput from './Components/SearchInput';
import SortInput from './Components/SortInput';
import Pagination from './Components/Pagination';
import LogoutButton from './Components/LogoutButton';
import LoginButton from './Components/LoginButton';
import NewLanguageButton from './Components/NewLanguageButton';

function App() {
  const [languages, setLanguages] = useState([]);
  const [selectedLanguages, setSelectedLanguages] = useState([]);
  const [resultData, setResultData] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    const fetchLanguages = async () => {
      try {
        const response = await api.get('/Language/GetAll');
        setLanguages(response.data)
      } catch (error) {
        console.error('Ошибка при получении языков:', error);
      }
    };

    fetchLanguages();
  }, []);

  useEffect(() => {
    fetchTotalPages();
  }, []);

  const fetchTotalPages = async () => {
    try {
      const res = await api.get('/Keyword/GetCountPage');
      setTotalPages(res.data);
    } catch (e) {
      console.error('Ошибка при получении количества страниц');
    }
  };

  const fetchPage = async (page) => {
    try {
      const response = await api.post('/Keyword/GetKeywordByLanguages', {
        page,
        languagesId: selectedLanguages,
      });

      setResultData(response.data);
      setCurrentPage(page);
    } catch (error) {
      console.error('Ошибка при загрузке страницы:', error);
    }
  };

  const toggleLanguage = (id) => {
    setSelectedLanguages(prev =>
      prev.includes(id)
        ? prev.filter(x => x !== id)
        : [...prev, id]
    );
  };

  const handleSearch = useCallback(async (value) => {
    try {
      const response = await api.post('/Keyword/GetKeywordByValue', {
        page: 1,
        value,
        languagesId: selectedLanguages
      });
      setResultData(response.data);
      setCurrentPage(1);
    } catch (error) {
      console.error('Ошибка при поиске:', error);
    }
  }, [selectedLanguages]);

  const handleSort = useCallback(async (filterValue) => {
    try {
      const response = await api.post('/Keyword/GetFilteredKeywordByValue', {
        page: 1,
        filterValue,
        languagesId: selectedLanguages
      });
      setResultData(response.data);
      setCurrentPage(1);
    } catch (error) {
      console.error('Ошибка при сортировке:', error);
    }
  }, [selectedLanguages]);

  return (
    <>
      <div style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>

        <AddKeywordForm
          selectedLanguages={selectedLanguages}
          setResultData={setResultData}
          fetchTotalPages={fetchTotalPages}
        />

        <SearchInput
          selectedLanguages={selectedLanguages}
          onSearch={handleSearch}
        />

        <SortInput
          selectedLanguages={selectedLanguages}
          onSort={handleSort}
        />

        <LoginButton />
        <LogoutButton />
        <NewLanguageButton />

      </div>

      <div>
        <div style={{ border: '1px solid #ccc', padding: 20, marginTop: 20 }}>
          <h3>Select Languages:</h3>
          <ul>
            {languages.map(lang => (
              <li key={lang.id}>
                <label>
                  <input
                    type="checkbox"
                    checked={selectedLanguages.includes(lang.id)}
                    onChange={() => toggleLanguage(lang.id)}
                  />
                  {lang.name}
                </label>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <div div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        <h3>Pagination</h3>
        <div>
          <Pagination
            totalPages={totalPages}
            currentPage={currentPage}
            onPageChange={fetchPage}
          />
        </div>
        <div>
          {resultData.length > 0 && (
            <ResultTable
              translations={resultData}
              selectedLanguages={selectedLanguages}
              languageList={languages}
              setResultData={setResultData}
              fetchTotalPages={fetchTotalPages}
            />
          )}
        </div>
      </div>

    </>
  );
}

export default App;