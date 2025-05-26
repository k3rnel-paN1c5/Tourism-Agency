import './DashboardHeader.css'
const DashboardHeader = ({ title, subtitle }) => (
  <header className="dashboard-header">
    <h1 className="dashboard-title">{title}</h1>
    <p className="dashboard-subtitle">{subtitle}</p>
  </header>
);

export default DashboardHeader