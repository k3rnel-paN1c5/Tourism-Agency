import React, { useState, useEffect } from "react";
import carService from "../../services/CarSupervisor/carService";


import DashboardHeader from "../../components/shared/DashboardHeader";
import DataTable from "../../components/shared/DataTable";
import Modal from "../../components/shared/Modal";
import CarForm from "../../components/carSupervisor/CarForm";
import "../shared/ManagementPage.css"; // Shared styles for management pages

/**
 * CarManagementPage component.
 * Provides functionality to manage cars (CRUD operations).
 */
const CarManagementPage = () => {
  const [cars, setCars] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCar, setSelectedCar] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Load both cars and categories when the component mounts
    const loadData = async () => {
      setIsLoading(true);
      try {
        const carsData = await carService.getCars();
        setCars(carsData);
      } catch (error) {
        console.error("Failed to load data:", error);
      }
      setIsLoading(false);
    };
    loadData();
  }, []);

  const loadCars = async () => {
    const data = await carService.getCars();
    setCars(data);
  };

  /**
   * Handles opening the modal for creating a new car.
   */
  const handleCreate = () => {
    setSelectedCar(null);
    setIsModalOpen(true);
  };

  /**
   * Handles opening the modal for editing an existing car.
   * @param {object} car - The car to edit.
   */
  const handleEdit = (car) => {
    setSelectedCar(car);
    setIsModalOpen(true);
  };

  /**
   * Handles deleting a car.
   * @param {string|number} id - The ID of the car to delete.
   */
  const handleDelete = async (id) => {
    if (window.confirm("Are you sure you want to delete this car?")) {
      try {
        await carService.deleteCar(id);
        loadCars(); // Refresh the list
      } catch (error) {
        console.error("Failed to delete car:", error);
      }
    }
  };

  /**
   * Handles saving a new or updated car.
   * @param {object} carData - The data of the car to save.
   */
  const handleSave = async (carData) => {
    try {
      if (selectedCar) {
        await carService.updateCar(selectedCar.id, carData);
      } else {
        await carService.createCar(carData);
      }
      loadCars(); // Refresh the list
      setIsModalOpen(false);
    } catch (error) {
      console.error("Failed to save car:", error);
    }
  };

  // Define columns for the DataTable
  const carColumns = [
    { header: "ID", key: "id" },
    { header: "Model", key: "model" },
    { header: "Seats", key: "seats" },
    { header: "Color", key: "color" },
    { header: "Image", key: "image" },
    { header: "Price Per Hour", key: "pph" },
    { header: "Price Per Day", key: "ppd" },
    { header: "Max Baggage Weight", key: "mbw" }
  ];

  return (
    <div className="management-page">
      <DashboardHeader title="Manage Cars" />
      <main className="management-content">
        <div className="actions-bar">
          <button onClick={handleCreate} className="btn-add-new">
            Add New Car
          </button>
        </div>
        {isLoading ? (
          <p>Loading cars...</p>
        ) : (
          <DataTable
            columns={carColumns}
            data={cars}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        )}
        {isModalOpen && (
          <Modal onClose={() => setIsModalOpen(false)}>
            <CarForm
              car={selectedCar}
              onSave={handleSave}
              onCancel={() => setIsModalOpen(false)}
            />
          </Modal>
        )}
      </main>
    </div>
  );
};

export default CarManagementPage;
