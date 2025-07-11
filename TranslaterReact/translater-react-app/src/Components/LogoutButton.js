import React from 'react';
import Cookies from 'js-cookie';

function LogoutButton({ onLogout }) {
  const handleLogout = () => {
    Cookies.remove('token');
    if (onLogout) onLogout();
    window.location.reload();
  };

  return (
    <button onClick={handleLogout}
      style={{
        backgroundColor: '#f44336',
        color: 'white',
        padding: '8px 16px',
        border: 'none',
        borderRadius: '4px',
        cursor: 'pointer'
      }}>
      Logout
    </button>
  );
}

export default LogoutButton;