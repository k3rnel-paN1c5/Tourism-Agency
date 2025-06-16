import { useState, useMemo } from 'react';
import DashboardHeader from '../../components/shared/DashboardHeader';
import Modal from '../../components/shared/Modal';
import ErrorMessage from '../../components/shared/ErrorMessage';
import CarFilters from '../../components/shared/CarFilters';
import CarBookingForm from '../../components/customer/CarBookingForm'; // Import the new form
import carService from '../../services/customer/carService';
import bookingService from '../../services/bookingService';
import './CarBookingPage.css';




// Reusable CarCard component (no changes)
const CarCard = ({ car, onBook }) => {
    const imageUrl = `/src/assets/images/${car.image}`;
    return (
        <div className="car-card">
            <img src={imageUrl} alt={`${car.model}`} className="car-card-image" />
            <div className="car-card-content">
                <h3 className="car-card-title">{car.model}</h3>
                <div className="car-attributes">
                    <span>Seats: {car.seats}</span>
                    <span>Color: {car.color}</span>
                    <span>Baggage: {car.mbw} kg</span>
                </div>
                <div className="car-pricing">
                    <span>${car.pph}/hour</span>
                    <span>${car.ppd}/day</span>
                </div>
                <button onClick={() => onBook(car)} className="book-now-button">Book Now</button>
            </div>
        </div>
    );
};


const CarBookingPage = () => {
    const [availableCars, setAvailableCars] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [selectedCar, setSelectedCar] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    
    const [filters, setFilters] = useState({
        model: '',
        color: '',
        categoryId: '',
        seats: '',
        pph: '',
        mbw: ''
    });

    const handleFilterChange = (name, value) => {
        setFilters(prev => ({ ...prev, [name]: value }));
    };

    const filteredCars = useMemo(() => {
        return availableCars.filter(car => {
            return (
                (filters.model ? car.model.toLowerCase().includes(filters.model.toLowerCase()) : true) &&
                (filters.color ? car.color.toLowerCase().includes(filters.color.toLowerCase()) : true) &&
                (filters.categoryId ? car.categoryId === parseInt(filters.categoryId) : true) &&
                (filters.seats ? car.seats >= parseInt(filters.seats) : true) &&
                (filters.pph ? car.pph >= parseInt(filters.pph) : true) &&
                (filters.mbw ? car.mbw >= parseInt(filters.mbw) : true)
            );
        });
    }, [availableCars, filters]);

    const handleSearch = async () => {
        if (!startDate || !endDate) {
            setError('Please select a start and end date.');
            return;
        }
        setIsLoading(true);
        setError(null);
        try {
            const cars = await carService.getAvailableCars(startDate, endDate);
            setAvailableCars(cars);
        } catch (err) {
            setError('Failed to load available cars. Please try again later.');
            setAvailableCars([]);
        } finally {
            setIsLoading(false);
        }
    };

    const handleBook = (car) => {
        setSelectedCar(car);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setSelectedCar(null);
    };

    const handleBookingSubmit = async (bookingData) => {
        // The booking service from bookingService.js should be used here
        await bookingService.createCarBooking(bookingData);
        handleCloseModal();
        // Re-fetch available cars to reflect the new booking
        handleSearch(); 
    };

    return (
        <div className="car-booking-page">
            <DashboardHeader
                title="Book a Car"
                subtitle="Find and book the perfect car for your trip."
            />

            <div className="date-picker-container">
                 <div className="form-group">
                     <label>Start Date</label>
                     <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
                 </div>
                 <div className="form-group">
                     <label>End Date</label>
                     <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
                 </div>
                 <button onClick={handleSearch} className="search-button">Search Cars</button>
             </div>

            {error && <ErrorMessage message={error} onClear={() => setError(null)} />}

            <div className="page-content">
                <div className="filters-container">
                    <CarFilters filters={filters} onFilterChange={handleFilterChange} />
                </div>

                <main className="results-container">
                    {isLoading ? (
                        <p className="loading-message">Loading cars...</p>
                    ) : (
                        <div className="cars-grid">
                            {filteredCars.length > 0 ? (
                                filteredCars.map(car => (
                                    <CarCard key={car.id} car={car} onBook={handleBook} />
                                ))
                            ) : (
                                <p>No cars available that match your criteria.</p>
                            )}
                        </div>
                    )}
                </main>
            </div>

            {selectedCar && (
                <Modal isOpen={isModalOpen} onClose={handleCloseModal} title={`Book: ${selectedCar.model}`}>
                    <CarBookingForm
                        car={selectedCar}
                        startDate={startDate}
                        endDate={endDate}
                        onSubmit={handleBookingSubmit}
                        onCancel={handleCloseModal}
                    />
                </Modal>
            )}
        </div>
    );
};

export default CarBookingPage;