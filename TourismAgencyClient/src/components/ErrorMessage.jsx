import React from 'react';
import './ErrorMessage.css';

/**
 * A component to display an error message.
 * @param {object} props - The component props.
 * @param {string} props.message - The error message to display.
 * @param {Function} [props.onClear] - Optional function to clear the error.
 */
const ErrorMessage = ({ message, onClear }) => {
    if (!message) return null;

    return (
        <div className="error-message-container">
            <p>{message}</p>
            {onClear && (
                <button onClick={onClear} className="error-clear-button">
                    &times;
                </button>
            )}
        </div>
    );
};

export default ErrorMessage;
