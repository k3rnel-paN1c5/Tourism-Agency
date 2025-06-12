import React, { useState, useEffect, useMemo } from 'react';
import categoryService from '../../services/CarSupervisor/categoryService';
import DashboardHeader from '../../components/shared/DashboardHeader';
import DataTable from '../../components/shared/DataTable';
import Modal from '../../components/shared/Modal';
import ErrorMessage from '../../components/shared/ErrorMessage';
import CategoryForm from '../../components/carSupervisor/CategoryForm';
import SearchBar from '../../components/shared/SearchBar';
import "../shared/ManagementPage.css";// Shared styles for management pages


/**
 * CategoryManagementPage component.
 * Provides functionality to manage categories (CRUD operations).
 */
const CategoryManagementPage = () => {
    const [categories, setCategories] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [formError, setFormError] = useState(null);
     const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        loadCategories();
    }, []);

    /**
     * Fetches all regions from the service and updates the state.
     */
    const loadCategories = async () => {
        setIsLoading(true);
        setError(null);
        setFormError(null);
        try {
            const data = await categoryService.getCategories();
            setCategories(data);
        } catch (error) {
            setError("Failed to load Categories. Please try again later.");
            console.error("Failed to load Categories:", error);
            // Here you could set an error message in the state and display it
        }
        setIsLoading(false);
    };

    /**
     * Handles opening the modal for creating a new category.
     */
    const handleCreate = () => {
        setError(null);
        setFormError(null);
        setSelectedCategory(null);
        setIsModalOpen(true);
    };

    /**
     * Handles opening the modal for editing an existing category.
     * @param {object} category - The category to edit.
     */
    const handleEdit = (category) => {
        setError(null);
        setFormError(null);
        setSelectedCategory(category);
        setIsModalOpen(true);
    };

    /**
     * Handles deleting a category.
     * @param {string|number} id - The ID of the category to delete.
     */
    const handleDelete = async (id) => {
        // A simple confirmation dialog. For a better UX, consider a custom modal confirmation.
        setError(null);
        setFormError(null);
        try {
            await categoryService.deleteCategory(id);
            loadCategories(); // Refresh the list after deletion
        } catch (error) {
            setError("Failed to delete category, category may be used by a car")
            console.error("Failed to delete category:", error);
        }

    };

    /**
     * Handles saving a new or updated category.
     * @param {object} categoryData - The data of the category to save.
     */
    const handleSave = async (categoryData) => {
        setError(null);
        setFormError(null);
        try {
            if (selectedCategory) {
                await categoryService.updateCategory(selectedCategory.id, categoryData);
            } else {
                await categoryService.createCategory(categoryData);
            }
            loadCategories(); // Refresh the list after saving
            setIsModalOpen(false);
        } catch (error) {
            setFormError("Failed to save category. Name already exist");
            console.error("Failed to save category:", error);
        }
    };



    const filteredCategories = useMemo(() => {
        if (!searchQuery) {
            return categories;
        }
        return categories.filter(category =>
            category.name.toLowerCase().includes(searchQuery.toLowerCase())
        );
    }, [categories, searchQuery]);

    const categoryColumns = [
        { header: 'ID', key: 'id' },
        { header: 'Title', key: 'title' },
    ];
    
    return (
        <div className="management-page">

            <DashboardHeader title="Manage Categories" />
            <main className="management-content">
                <div className="actions-bar">
                   
                    <button onClick={handleCreate} className="btn-add-new">Add New Category</button>
                </div>

                <ErrorMessage message={error} onClear={() => setError(null)} />

                {isLoading ? (
                    <p>Loading Categories...</p>
                ) : (
                    <DataTable
                        columns={categoryColumns}
                        data={filteredCategories}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                    />
                )}
                {isModalOpen && (
                    <Modal
                        isOpen={isModalOpen}
                        onClose={() => setIsModalOpen(false)}
                        title="Creating a new category">
                            <ErrorMessage message={formError} onClear={() => setFormError(null)} />
                        <CategoryForm
                            onSubmit={handleSave}
                            initialData={selectedCategory}
                            onSave={handleSave}
                            onCancel={() => setIsModalOpen(false)}
                        />
                    </Modal>
                )}
            </main>
        </div>
    );
};

export default CategoryManagementPage;
