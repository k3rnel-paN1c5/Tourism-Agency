import { Route, createBrowserRouter, createRoutesFromElements } from 'react-router-dom';
import ProtectedRoutes from './ProtectedRoutes';
import Dashboard from './pages/Customer/Dashboard'
import Login from './components/Login';
import Register from './components/Register';
import NotFound from './pages/NotFound'

const router = createBrowserRouter(
    createRoutesFromElements(
        <Route path='/'>
            <Route element={<ProtectedRoutes />}>
                <Route path='/' element={<Dashboard />} />
                {/* <Route path='/admin' element={<Admin />} /> */}
            </Route>
            <Route path='/login' element={<Login />} />
            <Route path='/register' element={<Register />} />
            <Route path='*' element={NotFound}/>
        </Route>
    )
);

export default router