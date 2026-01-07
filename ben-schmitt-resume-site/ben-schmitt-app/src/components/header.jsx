import { Link } from "react-router-dom";
import {useNavigate} from "react-router-dom";
import {getAuth, signOut} from "firebase/auth";
import useUser from "../useUser";

export default function Header() {
  const { isLoading, user } = useUser();
  const navigate = useNavigate();
  const auth = getAuth();

    return (
        <header class="primary-header full-width">
          <div class="primary-header__layout">
            <nav className="primary-navigation">
              <ul>
                <li><Link to="/home">Home</Link></li>
                <li><Link to="/resume">Resume</Link></li>
                <li><Link to="/technologies">Technologies</Link></li>
                <li><Link to="/hobbies">Hobbies</Link></li>
                <li><Link to="/blog">Blog</Link></li>
                {isLoading ? <li>Loading...</li> : (
                  <>
                    {user && (
                      <li style={{ color: 'white' }}>
                        Logged in as {user.email}
                      </li>
                    )}
                    <li>
                      {user
                        ? <button onClick={() => signOut(getAuth())}>Log Out</button>
                        : <button onClick={() => navigate('/login')}>Log In</button>}
                    </li>
                  </>
                )}
              </ul>
            </nav>
      
            <div className="account-links">          
              <a href="#">Log In</a>
              <a href="#" className="create-account">Create Account</a>
            </div>
          </div>
        </header>
    )


}
