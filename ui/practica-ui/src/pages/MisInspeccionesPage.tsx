import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import type { Inspeccion } from '../services/inspeccion.service';
import { getMisInspecciones } from '../services/inspeccion.service';
import { toast, Toaster } from 'sonner';

const MisInspeccionesPage: React.FC = () => {
    const [inspecciones, setInspecciones] = useState<Inspeccion[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        setIsLoading(true);
        getMisInspecciones()
            .then(data => {
                setInspecciones(data);
                toast.success('Inspecciones cargadas.');
            })
            .catch(error => {
                console.error(error);
                toast.error('No se pudieron cargar las inspecciones.');
            })
            .finally(() => setIsLoading(false));
    }, []);

    if (isLoading) {
        return <div>Cargando mis inspecciones...</div>;
    }

    return (
        <div className="container mx-auto p-4">
            <Toaster />
            <h1 className="text-3xl font-bold mb-6">Mis Inspecciones</h1>
            {inspecciones.length === 0 ? (
                <p>No tienes inspecciones asignadas.</p>
            ) : (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {inspecciones.map(ins => (
                        <div key={ins.id} className="bg-white p-6 rounded-lg shadow-md">
                            <h2 className="text-xl font-bold">{ins.nombreCliente}</h2>
                            <p className="text-gray-600">{ins.direccion}</p>
                            <div className="mt-4">
                                <span className={`px-3 py-1 inline-flex text-sm leading-5 font-semibold rounded-full ${
                                    ins.estado === 'Pendiente' ? 'bg-yellow-100 text-yellow-800' :
                                    ins.estado === 'En Progreso' ? 'bg-blue-100 text-blue-800' :
                                    'bg-green-100 text-green-800'
                                }`}>
                                    {ins.estado}
                                </span>
                            </div>
                            <p className="text-sm text-gray-500 mt-2">Registrado: {new Date(ins.fechaRegistro).toLocaleDateString()}</p>
                            <Link to={`/inspecciones/${ins.id}`} className="text-indigo-600 hover:text-indigo-900 mt-4 inline-block">
                                Ver Detalles
                            </Link>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default MisInspeccionesPage;
