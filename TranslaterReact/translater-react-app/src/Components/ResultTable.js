import React, { useState } from 'react';
import api from '../Api/Api';

function ResultTable({ translations, selectedLanguages, languageList, setResultData, fetchTotalPages }) {
  const [editingCell, setEditingCell] = useState(null);
  const [tempValue, setTempValue] = useState('');

  const handleBlur = async (id, value) => {
    setEditingCell(null);
    if (!value.trim()) return;

    try {
      await api.post('/Translation/UpdateTranslationVlaueById', { id, value });

      setResultData(prev =>
        prev.map(row => ({
          ...row,
          outTranslationWithLanguages: row.outTranslationWithLanguages.map(trans =>
            trans.translationId === id
              ? { ...trans, translationValue: value }
              : trans
          )
        }))
      );

    } catch (error) {
      console.error('Ошибка при обновлении:', error);
    }
  };

  const handleDelete = async (name) => {
    try {
      await api.post('/Keyword/DeleteKeywordByValue', { name });
      fetchTotalPages()
      setResultData(prev => prev.filter(row => row.value !== name));
    } catch (error) {
      console.error('Ошибка при удалении:', error);
    }
  };

  return (
    <table border="1" cellPadding="8" style={{ marginTop: 20 }}>
      <thead>
        <tr>
          <th></th>
          <th>Keyword</th>
          {selectedLanguages.map(langId => {
            const lang = languageList.find(l => l.id === langId);
            return (
              <th key={langId}>{lang?.name}</th>
            );
          })}
        </tr>
      </thead>
      <tbody>
        {translations.map(row => (
          <tr key={row.id}>
            <td>
              <button
                onClick={() => handleDelete(row.value)}
                style={{
                  color: 'red',
                  border: 'none',
                  background: 'transparent',
                  cursor: 'pointer'
                }}
                title="Удалить keyword"
              >
                ❌
              </button>
            </td>
            <td>{row.value}</td>
            {selectedLanguages.map(langId => {
              const cell = row.outTranslationWithLanguages.find(t => t.languageId === langId);
              const isEditing = editingCell?.rowId === row.id && editingCell?.langId === langId;
              const cellId = cell?.translationId || '';
              const cellValue = cell?.translationValue || '';

              return (
                <td
                  key={`${row.id}-${langId}`}
                  onClick={() => {
                    setEditingCell({ rowId: row.id, langId });
                    setTempValue(cellValue);
                  }}
                >
                  {isEditing ? (
                    <input
                      autoFocus
                      value={tempValue}
                      onChange={e => setTempValue(e.target.value)}
                      onBlur={() => handleBlur(cellId, tempValue)}
                      style={{ width: '100%' }}
                    />
                  ) : (
                    cellValue
                  )}
                </td>
              );
            })}
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export default ResultTable;