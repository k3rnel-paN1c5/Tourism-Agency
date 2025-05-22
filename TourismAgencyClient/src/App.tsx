import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { useState, useEffect, type JSX } from 'react'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import { authService } from './services/auth.service'
import './App.css'
import RegisterPage from './pages/RegisterPage'

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false)
  
  useEffect(() => {
    // Check if user is authenticated on component mount
    setIsAuthenticated(authService.isAuthenticated())
  }, [])

  // Protected route component
  const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
    if (!isAuthenticated) {
      return <Navigate to="/login" />
    }
    return children
  }

  return (
    <Router>
      <div className="app">
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route 
            path="/" 
            element={
              <ProtectedRoute>
                <HomePage />
              </ProtectedRoute>
            } 
          />
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </div>
    </Router>
  )
}

export default App
