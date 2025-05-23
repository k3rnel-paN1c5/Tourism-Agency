import { Link, useLocation } from 'react-router-dom';
import Login from '../../components/Login';
import Register from '../../components/Register';

export default function AuthPage() {
  const location = useLocation();
  const isLogin = location.pathname.includes('login');

  return (
    <div style={{ maxWidth: '400px', margin: 'auto' }}>
      {isLogin ? <Login /> : <Register />}
      <p>
        {isLogin ? "Don't have an account?" : "Already have an account?"}{' '}
        <Link to={isLogin ? '/register' : '/login'}>
          {isLogin ? 'Register' : 'Login'}
        </Link>
      </p>
    </div>
  );
}