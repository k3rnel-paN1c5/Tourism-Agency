

// You might want to fetch car categories for a dropdown in the future
// import categoryService from '../services/categoryService';

const CarForm = ({ onSubmit, initialData, isLoading, error }) => {
  // 1. Initialize form state with fields matching the car data structure
  const [formData, setFormData] = useState({
    model: '',
    seats: '',
    color: '',
    image: '',
    pph: '', // Price Per Hour
    ppd: '', // Price Per Day
    mbw: '', // Max Baggage Weight
    categoryId: '',
  });

  // 2. Populate the form when initialData is provided (for editing)
  useEffect(() => {
    // If we have data, we are in "edit" mode
    if (initialData) {
      setFormData({
        model: initialData.model || '',
        seats: initialData.seats || '',
        color: initialData.color || '',
        image: initialData.image || '',
        pph: initialData.pph || '',
        ppd: initialData.ppd || '',
        mbw: initialData.mbw || '',
        categoryId: initialData.categoryId || '',
      });
    } else {
      // Otherwise, we are in "create" mode, so reset the form
      setFormData({
        model: '',
        seats: '',
        color: '',
        image: '',
        pph: '',
        ppd: '',
        mbw: '',
        categoryId: '',
      });
    }
  }, [initialData]);

  // 3. A generic handler to update state when any input changes
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  // 4. Handle form submission
  const handleSubmit = (e) => {
    e.preventDefault();

    // Create a new object for submission to avoid mutating state directly
    const dataToSubmit = {
      ...formData,
      // IMPORTANT: Convert string values from inputs back to numbers
      seats: parseFloat(formData.seats),
      pph: parseFloat(formData.pph),
      ppd: parseFloat(formData.ppd),
      mbw: parseFloat(formData.mbw),
      categoryId: parseInt(formData.categoryId, 10), // Category ID is likely an integer
    };
    
    // If we are editing, make sure to include the ID in the submission
    if (initialData && initialData.id) {
      dataToSubmit.id = initialData.id;
    }

    // Call the parent onSubmit function with the prepared data
    onSubmit(dataToSubmit);
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form">
      {error && <p className="error-message">{error}</p>}

      {/* Text Inputs */}
      <div className="input-group">
        <label htmlFor="carModel" className="input-label">Car Model</label>
        <input id="carModel" type="text" name="model" value={formData.model} onChange={handleChange} required className="auth-input" placeholder="e.g., Honda CR-V" />
      </div>
      <div className="input-group">
        <label htmlFor="carColor" className="input-label">Color</label>
        <input id="carColor" type="text" name="color" value={formData.color} onChange={handleChange} required className="auth-input" placeholder="e.g., White" />
      </div>
       <div className="input-group">
        <label htmlFor="carImage" className="input-label">Image URL</label>
        <input id="carImage" type="text" name="image" value={formData.image} onChange={handleChange} required className="auth-input" placeholder="e.g., honda-cr-v.jpg" />
      </div>

      {/* Numeric and Decimal Inputs */}
      <div className="form-row">
        <div className="input-group">
          <label htmlFor="carSeats" className="input-label">Seats</label>
          <input id="carSeats" type="number" name="seats" value={formData.seats} onChange={handleChange} required className="auth-input" />
        </div>
        <div className="input-group">
            <label htmlFor="carPph" className="input-label">Price Per Hour ($)</label>
            <input id="carPph" type="number" step="0.01" name="pph" value={formData.pph} onChange={handleChange} required className="auth-input" />
        </div>
         <div className="input-group">
            <label htmlFor="carPpd" className="input-label">Price Per Day ($)</label>
            <input id="carPpd" type="number" step="0.01" name="ppd" value={formData.ppd} onChange={handleChange} required className="auth-input" />
        </div>
      </div>
      
      <div className="form-row">
        <div className="input-group">
            <label htmlFor="carMbw" className="input-label">Max Baggage (kg)</label>
            <input id="carMbw" type="number" step="0.1" name="mbw" value={formData.mbw} onChange={handleChange} required className="auth-input" />
        </div>
        <div className="input-group">
            <label htmlFor="carCategoryId" className="input-label">Category ID</label>
            <input id="carCategoryId" type="number" name="categoryId" value={formData.categoryId} onChange={handleChange} required className="auth-input" />
            {/* In a real app, this would be a <select> dropdown */}
        </div>
      </div>

      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Car' : 'Create Car')}
      </button>
    </form>
  );
};

export default CarForm;