import React from 'react';
import './SearchBar.css';

/**
 * A reusable search bar component.
 * @param {object} props - The component props.
 * @param {Function} props.onSearch - The function to call when the search query changes.
 * @param {string} [props.placeholder] - The placeholder text for the input field.
 */
const SearchBar = ({ onSearch, placeholder = "Search..." }) => {
    return (
        <div className="search-bar-container">
            <input
                type="text"
                className="search-input"
                placeholder={placeholder}
                onChange={(e) => onSearch(e.target.value)}
            />
        </div>
    );
};

export default SearchBar;
