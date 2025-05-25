import authService from '../../services/authService';
import { useNavigate } from 'react-router-dom';
import './Dashboard.css'
export default function Dashboard() {
  const navigate = useNavigate();

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
  };

  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h1>Welcome to Your Dashboard</h1>
        <p>You've successfully logged in!</p>
        <button onClick={handleLogout}>Logout</button>
      </div>
      <div className="stats-grid">
        <div className="stat-card">
          <h3>Total Users</h3>
          <p className="stat-number">1,234</p>
        </div>
        
        <div className="stat-card">
          <h3>Active Sessions</h3>
          <p className="stat-number">42</p>
        </div>
        
        <div className="stat-card">
          <h3>Daily Visits</h3>
          <p className="stat-number">890</p>
        </div>
      </div>
      
      <div className="recent-activity">
        <h2>Recent Activity</h2>
        <div className="activity-list">
          <div className="activity-item">
            <span className="activity-icon login"></span>
            <div className="activity-details">
              <p>Successfully logged in from Chrome on Windows</p>
              <small>Today, 10:30 AM</small>
            </div>
          </div>
          
          <div className="activity-item">
            <span className="activity-icon profile"></span>
            <div className="activity-details">
              <p>Updated profile information</p>
              <small>Yesterday, 3:45 PM</small>
            </div>
          </div>
          <div className="activity-item">
            <span className="activity-icon device"></span>
            <div className="activity-details">
              <p>New device registered: haha</p>
              <small>2 days ago, 9:15 AM</small>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}