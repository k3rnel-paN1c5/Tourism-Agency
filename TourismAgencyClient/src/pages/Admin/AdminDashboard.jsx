import React from "react";
import DashboardHeader from "../../components/shared/DashboardHeader";
import DashboardBlock from "../../components/shared/DashboardBlock"; // Import the new component
import "./AdminDashboard.css";

/**
 * CarSupervisorDashboard component.
 * Displays a dashboard with navigation blocks for managing different aspects of Cars.
 */
const AdminDashboard = () => {
  return (
    <div className="dashboard">
      <DashboardHeader title="Admin Dashboard" />
      <main className="dashboard-main-content">
        <div className="dashboard-blocks">
          {/* Use the new DashboardBlock component */}
          <DashboardBlock
            title="Register New Employee"
            description="Regsiter new Car supervisors, Trip supervisors, and Managers"
            linkTo="/employee-register"
          />
          
          <DashboardBlock
            title="Future Use"
            description="This block is reserved for future features."
            isEmpty={true}
          />
        </div>
      </main>
    </div>
  );
};

export default AdminDashboard;
