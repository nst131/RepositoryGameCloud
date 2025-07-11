import React from 'react';

function Pagination({ totalPages, currentPage, onPageChange }) {
  if (totalPages <= 1) return null;

  
  const startPage = Math.min(
    Math.max(1, currentPage - 2),
    Math.max(1, totalPages - 4)
  );

  const pagesToShow = Array.from({ length: Math.min(5, totalPages) }, (_, i) => startPage + i);

  return (

    <div style={{ marginTop: 20, display: 'flex', gap: 5 }}>
      {pagesToShow.map(page => (
        <button
          key={page}
          onClick={() => onPageChange(page)}
          style={{
            padding: '5px 10px',
            backgroundColor: currentPage === page ? '#add8e6' : 'white',
            border: '1px solid #ccc',
            cursor: 'pointer',
          }}
        >
          {page}
        </button>
      ))}
    </div>
  );
}

export default Pagination;
