import { useEffect, useState } from 'react';
import Login from '../../components/auth/Login';
import Register from '../../components/auth/Register';
import './AuthPage.css'

export default function AuthPage(login) {
  const [isLogin, setIsLogin] = useState(login['login']);

  useEffect(() => {
    setIsLogin(login['login']);
  }, [login])

  return (
    <div className="auth-container">
      <div className="auth-card">
        <div className="logo-container">
          {/* <div className="logo"></div> */}
          <h1>HIAST Tourism Agency</h1>
        </div>

        {isLogin ? <Login /> : <Register />}

        <div className="toggle-container">
          <p>
            {isLogin ? "Don't have an account?" : "Already have an account?"}
            <button
              className="toggle-button"
              onClick={() => setIsLogin(!isLogin)}
            >
              {isLogin ? "Sign Up" : "Sign In"}
            </button>
          </p>
        </div>
      </div>

      <div className="background-shapes">
        <div className="shape circle-1"></div>
        <div className="shape circle-2"></div>
        <div className="shape blob-1"></div>
        <div className="shape blob-2"></div>
      </div>
    </div>
  );
}