import React from 'react';
import './DataTable.css';

const DataTable = ({ title, columns, data, onEdit, onDelete, onCreate, createLabel = "Create New" }) => {
  if (!Array.isArray(data)) {
    return <p>No data available or data is not in the expected format.</p>;
  }
  return (
    <div className="data-table-container">
      <div className="data-table-header">
        <h2>{title}</h2>
        {onCreate && (
          <button onClick={onCreate} className="create-button">
            {createLabel}
          </button>
        )}
      </div>
      {data.length === 0 ? (
        <p>No {title.toLowerCase()} found.</p>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              {columns.map((col) => (
                <th key={col.key}>{col.header}</th>
              ))}
              {(onEdit || onDelete) && <th>Actions</th>}
            </tr>
          </thead>
          <tbody>
            {data.map((item) => (
              <tr key={item.id}>
                {columns.map((col) => (
                  <td key={`${item.id}-${col.key}`}>{item[col.key]}</td>
                ))}
                {(onEdit || onDelete) && (
                  <td className="actions-cell">
                    {onEdit && (
                      <button onClick={() => onEdit(item)} className="action-button edit-button">
                        Edit
                      </button>
                    )}
                    {onDelete && (
                      <button onClick={() => onDelete(item.id)} className="action-button delete-button">
                        Delete
                      </button>
                    )}
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default DataTable;