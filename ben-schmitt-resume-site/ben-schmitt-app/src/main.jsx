import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'


import { initializeApp } from "firebase/app";

const firebaseConfig = {
  apiKey: "AIzaSyDvfHZ2b0JyMZCi7_6IMYj5ZTKoOlE6YVY",
  authDomain: "full-stack-react-726d8.firebaseapp.com",
  projectId: "full-stack-react-726d8",
  storageBucket: "full-stack-react-726d8.firebasestorage.app",
  messagingSenderId: "825298698398",
  appId: "1:825298698398:web:5fb7c5fcc76d4942bcbb8f"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
