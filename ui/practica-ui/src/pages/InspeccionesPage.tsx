import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'; 
import { getAllInspecciones } from '../services/inspeccion.service';
import { toast, Toaster } from 'sonner';
import type { Inspeccion } from '../services/inspeccion.service';

const InspeccionesPage: React.FC = () => {
  const [inspecciones, setInspecciones] = useState<Inspeccion[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchInspecciones = async () => {
      try {
        setIsLoading(true);
        const data = await getAllInspecciones();
        setInspecciones(data);
      } catch (error) {
        console.error("Error al cargar las inspecciones:", error);
        toast.error("Error al cargar las inspecciones.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchInspecciones();
  }, []);

  if (isLoading) {
    return <div className="container mx-auto p-4 text-center">Cargando inspecciones...</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <Toaster position="top-right" />
      <h1 className="text-3xl font-bold mb-6">Todas las Inspecciones</h1>

      {inspecciones.length === 0 ? (
        <p className="text-center text-gray-500">No hay inspecciones registradas.</p>
      ) : (
        <div className="bg-white p-6 rounded-lg shadow-md overflow-hidden">
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Cliente</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Dirección</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Técnico Asignado</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Estado</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Fecha Registro</th>
                  {/* Nueva Columna para Acciones */}
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Acciones</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {inspecciones.map((inspeccion) => (
                  <tr key={inspeccion.id} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{inspeccion.nombreCliente}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{inspeccion.direccion}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {inspeccion.tecnico?.nombre || <span className="italic text-gray-400">Sin asignar</span>}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                        inspeccion.estado === 'Pendiente' ? 'bg-yellow-100 text-yellow-800' :
                        inspeccion.estado === 'Completada' ? 'bg-green-100 text-green-800' :
                        inspeccion.estado === 'En Progreso' ? 'bg-blue-100 text-blue-800' :
                        inspeccion.estado === 'Cancelada' ? 'bg-red-100 text-red-800' :
                        'bg-gray-100 text-gray-800'
                      }`}>
                        {inspeccion.estado}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {new Date(inspeccion.fechaRegistro).toLocaleDateString()}
                    </td>
                    {/* Botón Ver Detalles */}
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <Link
                        to={`/inspecciones/${inspeccion.id}`}
                        className="text-indigo-600 hover:text-indigo-900 font-bold hover:underline"
                      >
                        Ver Detalles
                      </Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default InspeccionesPage;