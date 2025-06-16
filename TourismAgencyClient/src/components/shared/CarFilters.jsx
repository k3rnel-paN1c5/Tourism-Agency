import { useState, useEffect } from 'react';
import categoryService from '../../services/Customer/categoryService';
import './CarFilters.css';

const CarFilters = ({ filters, onFilterChange }) => {
    const [categories, setCategories] = useState([]);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const data = await categoryService.getCategories();
                setCategories(data);
            } catch (error) {
                console.error("Failed to fetch categories:", error);
            }
        };
        fetchCategories();
    }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        onFilterChange(name, value);
    };

    return (
        <div className="car-filters">
            <h3 className="filters-title">Filters</h3>
            <div className="filter-group">
                <label>Model</label>
                <input type="text" name="model" value={filters.model} onChange={handleInputChange} placeholder="e.g., Camry" />
            </div>
            <div className="filter-group">
                <label>Color</label>
                <input type="text" name="color" value={filters.color} onChange={handleInputChange} placeholder="e.g., Blue" />
            </div>
            <div className="filter-group">
                <label>Category</label>
                <select name="categoryId" value={filters.categoryId} onChange={handleInputChange}>
                    <option value="">All</option>
                    {categories.map(category => (
                        <option key={category.id} value={category.id}>{category.title}</option>
                    ))}
                </select>
            </div>
            <div className="filter-group">
                <label>Min. Seats</label>
                <input type="number" name="seats" min="1" value={filters.seats} onChange={handleInputChange} />
            </div>
            <div className="filter-group">
                <label>Min. Price per Hour ($)</label>
                <input type="number" name="pph" min="0" value={filters.pph} onChange={handleInputChange} />
            </div>
            <div className="filter-group">
                <label>Min. Baggage Weight (kg)</label>
                <input type="number" name="mbw" min="0" value={filters.mbw} onChange={handleInputChange} />
            </div>
        </div>
    );
};

export default CarFilters;