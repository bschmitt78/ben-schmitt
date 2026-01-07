
import  { createBrowserRouter, RouterProvider} from 'react-router-dom'
import './App.css'
import Home from './pages/Home.jsx'
import Blog from './pages/Blog.jsx'
import Technologies from './pages/Technologies.jsx'
import Resume from './pages/Resume.jsx'
import Layout from './components/Layout.jsx';
import Login from './pages/Login.jsx';
import NotFoundPage from './pages/NotFoundPage.jsx';
import Account from './pages/Account.jsx';
import Hobbies from './pages/Hobbies.jsx';



const routes = [{
  path: '/',
  element: <Layout />,
  errorElement: <NotFoundPage />,
  children: [{
    path: '/',
    element: <Home/>
    },
    {
      path: '/home',
      element: <Home/>
    },
    {
      path: '/resume',
      element: <Resume/>
    },
    {
      path: '/technologies',
      element: <Technologies/>
    },
    {
      path: '/hobbies',
      element: <Hobbies/>
    },
    {
      path: '/blog', 
      element: <Blog/>
    },
    
  {
    path: '/login',
    element: <Login/>
  },
  {
    path: '/create-account',
    element: <Account/>
  }]

}]

const router = createBrowserRouter(routes);

function App() {
  return (
    <>
      <RouterProvider router={router} />
    </>
    
  );
}

export default App
