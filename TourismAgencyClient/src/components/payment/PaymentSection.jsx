import { useState } from 'react';
import PaymentCard from './PaymentCard';
import PaymentModal from './PaymentModal';
import Modal from '../shared/Modal';
import './PaymentSection.css';

const PaymentSection = ({ payments, onPaymentUpdate, onPaymentAction }) => {
  const [selectedPayment, setSelectedPayment] = useState(null);
  const [isPaymentModalOpen, setIsPaymentModalOpen] = useState(false);

  const handlePaymentClick = (payment) => {
    setSelectedPayment(payment);
    setIsPaymentModalOpen(true);
  };

  const handleClosePaymentModal = () => {
    setIsPaymentModalOpen(false);
    setSelectedPayment(null);
  };

  const handlePaymentProcessed = async (updatedPayment) => {
    // Call the onPaymentUpdate prop to update the payments list in the parent component
    onPaymentUpdate(updatedPayment);
    handleClosePaymentModal();
  };

  return (
    <div className="payment-section">
      <div className="section-header">
        <h2>Payment Management</h2>
        <p>View and manage your payments</p>
      </div>

      <div className="payments-grid">
        {payments.length === 0 ? (
          <div className="no-payments">
            <p>No payments found</p>
          </div>
        ) : (
          payments.map(payment => (
            <PaymentCard
              key={payment.id}
              payment={payment}
              onClick={handlePaymentClick}
            />
          ))
        )}
      </div>
      {selectedPayment && (
        <PaymentModal
          payment={selectedPayment}
          type="process" // Set the type to 'process' for making a transaction
          onClose={handleClosePaymentModal}
          onSuccess={handlePaymentProcessed}
          onPaymentAction={onPaymentAction} // Pass the onPaymentAction prop from CustomerDashboard
        />
      )}
    </div>
  );
};

export default PaymentSection;