// src/Components/SearchInput.js
import React, { useState, useEffect, useRef } from 'react';

function SearchInput({ onSearch, selectedLanguages }) {
  const [value, setValue] = useState('');
  const timeoutRef = useRef(null);

  useEffect(() => {
    if (!selectedLanguages || selectedLanguages.length === 0) return;

    if (timeoutRef.current) clearTimeout(timeoutRef.current);

    timeoutRef.current = setTimeout(() => {
      onSearch(value);
    }, 500);

    return () => clearTimeout(timeoutRef.current);
  }, [value, selectedLanguages, onSearch]);

  return (
    <input
      type="text"
      placeholder="Search by keyword..."
      value={value}
      onChange={(e) => setValue(e.target.value)}
      style={{ padding: '5px', flex: 1 }}
    />
  );
}

export default SearchInput;