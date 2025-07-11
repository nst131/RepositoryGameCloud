import React, { useState, useEffect, useRef } from 'react';

function SortInput({ onSort, selectedLanguages }) {
  const [value, setValue] = useState('');
  const timeoutRef = useRef(null);

  useEffect(() => {
    if (!selectedLanguages || selectedLanguages.length === 0) return;

    if (timeoutRef.current) clearTimeout(timeoutRef.current);

    timeoutRef.current = setTimeout(() => {
      onSort(value);
    }, 500);

    return () => clearTimeout(timeoutRef.current);
  }, [value, selectedLanguages, onSort]);

  return (
    <input
      type="text"
      placeholder="Sort By Keyword..."
      value={value}
      onChange={(e) => setValue(e.target.value)}
      style={{ padding: '5px', flex: 1 }}
    />
  );
}

export default SortInput;