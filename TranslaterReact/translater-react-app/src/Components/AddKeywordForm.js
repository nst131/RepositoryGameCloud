import React, { useState } from 'react';
import api from '../Api/Api';

function AddKeywordForm({ selectedLanguages, setResultData, fetchTotalPages }) {
  const [newKeyword, setNewKeyword] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [error, setError] = useState('');

  const handleAddKeyword = async () => {
    if (!newKeyword.trim()) return;

    try {
      const response = await api.post('/Keyword/InsertKeyword', { name: newKeyword });
      setNewKeyword('');
      setShowForm(false);
      setError('');

      fetchTotalPages();

      const newKeywordData = response.data;
      const filteredKeyword = {
        ...newKeywordData,
        outTranslationWithLanguages: newKeywordData.outTranslationWithLanguages.filter(t =>
          selectedLanguages.includes(t.languageId)
        )
      };

      setResultData(prev => [filteredKeyword, ...prev]);

    } catch (error) {
      if (error.response?.status === 401) {
        setError('Please Login');
      } else if (error.response?.status === 400) {
        setError(error.response.data.message || 'Mistake during addition');
      } else {
        console.error('Ошибка при добавлении keyword:', error);
      }
    }
  };

  return (
    <>
      <button style={{
        backgroundColor: '#007bff',
        color: 'white',
        border: 'none',
        padding: '5px 12px',
        borderRadius: '4px',
        cursor: 'pointer',
        fontWeight: 'bold'
      }}
        onClick={() => setShowForm(true)}> New keyword </button>

      {showForm && (
        <div style={{
          position: 'fixed',
          top: 0, left: 0,
          width: '100%',
          height: '100%',
          backgroundColor: 'rgba(0,0,0,0.5)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          zIndex: 999
        }}>
          <div style={{
            backgroundColor: 'white',
            padding: 20,
            borderRadius: 8,
            minWidth: 300
          }}>
            <h3>Add Keyword</h3>
            <input
              type="text"
              placeholder="New keyword"
              value={newKeyword}
              onChange={e => setNewKeyword(e.target.value)}
              style={{ marginBottom: 10, width: '100%' }}
            />
            {error && <div style={{ color: 'red', marginBottom: 10 }}>{error}</div>}
            <div style={{ display: 'flex', gap: 10 }}>
              <button onClick={handleAddKeyword}>Send</button>
              <button onClick={() => {
                setShowForm(false);
                setError('');
              }}>Cancel</button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}

export default AddKeywordForm;
