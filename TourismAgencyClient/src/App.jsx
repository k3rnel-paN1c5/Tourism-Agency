import { BrowserRouter } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';
import { AuthProvider } from './context/AuthContext'; 
import Navbar from './components/shared/NavBar';
import './App.css'

function App() {
  return (
     <BrowserRouter>
        <AuthProvider> 
            <Navbar /> 
            <AppRoutes />
        </AuthProvider>
     </BrowserRouter>
  );
}

export default App;