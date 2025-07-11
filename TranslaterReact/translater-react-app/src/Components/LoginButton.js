import React, { useState, useEffect } from 'react';
import AuthApi from '../Api/AuthApi';
import Cookies from 'js-cookie';

function LoginButton() {
  const [showModal, setShowModal] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    const token = Cookies.get('token');
    setIsLoggedIn(!!token);
  }, []);

  const handleLogin = async () => {
    try {
      const response = await AuthApi.post('/Auth/login', { email, password });
      Cookies.set('token', response.data.token);
      setIsLoggedIn(true);
      setShowModal(false);
      setError('');
      window.location.reload();
    } catch (err) {
      if (err.response?.status === 401) {
        setError('Incorrect email or passwrd');
      } else {
        setError('Incorrect email or password');
      }
    }
  };

  if (isLoggedIn) {
    return null;
  }

  return (
    <>
      <button
        onClick={() => setShowModal(true)}
        style={{
          backgroundColor: '#4CAF50',
          color: 'white',
          padding: '8px 16px',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer'
        }}
      >
        Login
      </button>

      {showModal && (
        <div style={{
          position: 'fixed',
          top: 0, left: 0,
          width: '100%', height: '100%',
          backgroundColor: 'rgba(0,0,0,0.5)',
          display: 'flex', justifyContent: 'center', alignItems: 'center'
        }}>
          <div style={{
            background: 'white',
            padding: 20,
            borderRadius: 8,
            minWidth: 300
          }}>
            <h3>Login</h3>
            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={e => setEmail(e.target.value)}
              style={{ width: '100%', marginBottom: 10 }}
            />
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              style={{ width: '100%', marginBottom: 10 }}
            />
            {error && <div style={{ color: 'red', marginBottom: 10 }}>{error}</div>}
            <button onClick={handleLogin}>Login</button>
            <button onClick={() => setShowModal(false)} style={{ marginLeft: 10 }}>
              Cancel
            </button>
          </div>
        </div>
      )}
    </>
  );
}

export default LoginButton;
