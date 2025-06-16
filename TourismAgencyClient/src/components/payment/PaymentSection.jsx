import { useState } from 'react';
import PaymentCard from './PaymentCard';
import './PaymentSection.css';

const PaymentSection = ({ payments, onPaymentUpdate }) => {
  const [selectedPayment, setSelectedPayment] = useState(null);

  const handlePaymentClick = (payment) => {
    setSelectedPayment(payment);
    // You can add modal logic here similar to booking modal
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
    </div>
  );
};

export default PaymentSection;