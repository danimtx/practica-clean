import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import type { Inspeccion } from '../services/inspeccion.service';
import { getInspeccionById, updateInspeccionEstado, uploadInspeccionFile } from '../services/inspeccion.service';
import { toast, Toaster } from 'sonner';
import { useAuthStore } from '../store/auth.store';

const DetalleInspeccionPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [inspeccion, setInspeccion] = useState<Inspeccion | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const { userProfile } = useAuthStore();

    const canChangeEstado = userProfile?.permisos.includes('inspeccion:estado');
    const canUploadFile = userProfile?.permisos.includes('inspeccion:archivo:subir');
    
    useEffect(() => {
        if (!id) return;
        setIsLoading(true);
        getInspeccionById(id)
            .then(setInspeccion)
            .catch(() => toast.error('No se pudo cargar la inspección.'))
            .finally(() => setIsLoading(false));
    }, [id]);

    const handleEstadoChange = async (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (!id) return;
        const newEstado = e.target.value;
        toast.info('Actualizando estado...');
        try {
            const updated = await updateInspeccionEstado(id, newEstado);
            setInspeccion(updated);
            toast.success('Estado actualizado.');
        } catch (error) {
            toast.error('No se pudo actualizar el estado.');
        }
    };
    
    const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    // Aseguramos que exista inspección antes de intentar actualizarla
    if (e.target.files && e.target.files[0] && id && inspeccion) {
        const file = e.target.files[0];
        const formData = new FormData();
        
       
        formData.append('archivo', file); 

        toast.info('Subiendo archivo...');
        try {
            
            const response: any = await uploadInspeccionFile(id, formData);
            
            
            setInspeccion({
                ...inspeccion,
                rutaArchivoPdf: response.rutaArchivo
            });
            
            toast.success('Archivo subido correctamente.');
        } catch (error) {
            console.error(error);
            toast.error('No se pudo subir el archivo.');
        }
    }
};

    if (isLoading) return <div>Cargando inspección...</div>;
    if (!inspeccion) return <div>Inspección no encontrada.</div>;

    const { nombreCliente, direccion, fechaRegistro, detallesTecnicos, observaciones, estado, rutaArchivoPdf, tecnico } = inspeccion;

    return (
        <div className="container mx-auto p-4">
            <Toaster />
            <h1 className="text-3xl font-bold mb-2">Inspección: {nombreCliente}</h1>
            <p className="text-gray-600 mb-6">{direccion}</p>
            
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                <div className="lg:col-span-2 bg-white p-6 rounded-lg shadow-md">
                    <h3 className="text-xl font-semibold mb-4">Detalles</h3>
                    <p><strong>Técnico Asignado:</strong> {tecnico?.nombre || 'No asignado'}</p>
                    <p><strong>Fecha de Registro:</strong> {new Date(fechaRegistro).toLocaleString()}</p>
                    <p className="mt-4"><strong>Detalles Técnicos:</strong></p>
                    <p className="whitespace-pre-wrap bg-gray-50 p-2 rounded">{detallesTecnicos}</p>
                    <p className="mt-4"><strong>Observaciones:</strong></p>
                    <p className="whitespace-pre-wrap bg-gray-50 p-2 rounded">{observaciones}</p>
                </div>

                <div className="lg:col-span-1 flex flex-col gap-6">
                    <div className="bg-white p-6 rounded-lg shadow-md">
                        <h3 className="text-xl font-semibold mb-4">Estado</h3>
                        {canChangeEstado ? (
                            <select value={estado} onChange={handleEstadoChange} className="shadow border rounded w-full py-2 px-3 text-gray-700">
                                <option>Pendiente</option>
                                <option>En Progreso</option>
                                <option>Completada</option>
                                <option>Cancelada</option>
                            </select>
                        ) : (
                            <p className="text-lg font-medium">{estado}</p>
                        )}
                    </div>
                    <div className="bg-white p-6 rounded-lg shadow-md">
                        <h3 className="text-xl font-semibold mb-4">Archivo PDF</h3>
                        {rutaArchivoPdf ? (
                            <a href={`http://localhost:5012/${rutaArchivoPdf}`} target="_blank" rel="noreferrer" className="text-indigo-600 hover:underline">
                                Descargar Informe
                            </a>
                        ) : (
                            <p>No se ha subido ningún archivo.</p>
                        )}
                        {canUploadFile && (
                            <div className="mt-4">
                                <label className="block text-sm font-medium text-gray-700">Subir nuevo informe (PDF)</label>
                                <input type="file" accept=".pdf" onChange={handleFileChange} className="mt-1 block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100"/>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default DetalleInspeccionPage;
