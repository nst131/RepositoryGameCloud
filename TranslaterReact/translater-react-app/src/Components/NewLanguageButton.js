import React, { useState } from 'react';
import api from '../Api/Api';

function NewLanguageButton() {
  const [showModal, setShowModal] = useState(false);
  const [name, setName] = useState('');
  const [error, setError] = useState('');

 

const handleSubmit = async () => {
    if (!name.trim()) return;

     try {
      await api.post('/Language/InsertNewLanguage', { name });
      window.location.reload();
    } catch (error) {
      console.log(error.response.data)
      if (error.response?.status === 401) {
        setError('Please login for perform this operation');
      } else if (error.response?.status === 400) {
        const msg = error.response.data?.message || 'Incorrect query';
        setError(`${msg}`);
      } else {
        console.error('Ошибка при добавлении языка:', error);
        setError('Occured mistake, please try again later');
      }
    }
  };

  return (
    <>
      <button
        onClick={() => {
          setShowModal(true);
          setError('');
        }}
        style={{
          backgroundColor: '#007bff',
          color: 'white',
          padding: '6px 12px',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer',
        }}
      >
        New Language
      </button>

      {showModal && (
        <div style={{
          position: 'fixed',
          top: 0, left: 0,
          width: '100vw', height: '100vh',
          backgroundColor: 'rgba(0, 0, 0, 0.5)',
          display: 'flex', alignItems: 'center', justifyContent: 'center',
          zIndex: 1000
        }}>
          <div style={{
            backgroundColor: 'white',
            padding: 20,
            borderRadius: 8,
            minWidth: 300,
            display: 'flex',
            flexDirection: 'column',
            gap: 10
          }}>
            <h3>Add language</h3>
            <input
              type="text"
              placeholder="Enter name"
              value={name}
              onChange={e => setName(e.target.value)}
              style={{ padding: '8px' }}
            />
            {error && <div style={{ color: 'red' }}>{error}</div>}
            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
              <button onClick={handleSubmit} style={{ padding: '6px 10px' }}>
                Send
              </button>
              <button onClick={() => setShowModal(false)} style={{ padding: '6px 10px' }}>
                Close
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}

export default NewLanguageButton;