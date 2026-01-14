import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useAuthStore } from './store/auth.store';
import LoginPage from './pages/LoginPage';
import ProtectedRoute from './components/ProtectedRoute';
import MainLayout from './components/layout/Layout';
import PerfilPage from './pages/PerfilPage';
import GestionUsuariosPage from './pages/GestionUsuariosPage';
import InspeccionesPage from './pages/InspeccionesPage';
import CrearInspeccionPage from './pages/CrearInspeccionPage';
import MisInspeccionesPage from './pages/MisInspeccionesPage';
import DetalleInspeccionPage from './pages/DetalleInspeccionPage';
import GestionCargosPage from './pages/GestionCargosPage';

// Componente para la página principal (Dashboard)
function HomePage() {
  const { userProfile } = useAuthStore();

  return (
    <div>
      <h1 className="text-3xl font-bold">Dashboard</h1>
      <p className="mt-2">Bienvenido de nuevo, {userProfile?.nombre}.</p>
      <div className="mt-6 p-6 bg-white rounded-lg shadow-md">
        <h2 className="text-xl font-semibold">Información del Perfil</h2>
        <p className="mt-2"><strong>Email:</strong> {userProfile?.email}</p>
        <p><strong>Cargo:</strong> {userProfile?.cargo}</p>
        <p className="mt-2"><strong>Permisos:</strong></p>
        <ul className="list-disc list-inside ml-4">
          {userProfile?.permisos?.map(p => <li key={p}>{p}</li>)}
        </ul>
      </div>
    </div>
  );
}

// Componente principal de la aplicación que define las rutas
function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        
        {/* Rutas Protegidas envueltas por el Layout Principal */}
        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/" element={<HomePage />} />
            <Route path="/perfil" element={<PerfilPage />} />
            <Route path="/gestion-usuarios" element={<GestionUsuariosPage />} />
            <Route path="/inspecciones" element={<InspeccionesPage />} />
            <Route path="/inspecciones/crear" element={<CrearInspeccionPage />} />
            <Route path="/mis-inspecciones" element={<MisInspeccionesPage />} />
            <Route path="/inspecciones/:id" element={<DetalleInspeccionPage />} />
            <Route path="/gestion-cargos" element={<GestionCargosPage />} />
          </Route>
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
