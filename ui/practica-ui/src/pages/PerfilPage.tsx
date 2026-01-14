import React, { useState } from 'react';
import { useAuthStore } from '../store/auth.store';
import { updateProfile, uploadProfilePhoto } from '../services/usuario.service';
import { Toaster, toast } from 'sonner';

const PerfilPage: React.FC = () => {
  const { userProfile, setUserProfile } = useAuthStore();
  const API_URL = 'http://localhost:5012/';
  
  const [isEditing, setIsEditing] = useState(false);
  const [nombre, setNombre] = useState(userProfile?.nombre || '');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isUploading, setIsUploading] = useState(false);

  const handleUpdateProfile = async (e: React.FormEvent) => {
    e.preventDefault();
    if (password && password !== confirmPassword) {
      toast.error('Las contraseñas no coinciden.');
      return;
    }
    
    setIsLoading(true);
    toast.info('Actualizando perfil...');

    try {
      const updatedProfile = await updateProfile(nombre, password || undefined);
      setUserProfile(updatedProfile);
      toast.success('Perfil actualizado correctamente.');
      setIsEditing(false);
      setPassword('');
      setConfirmPassword('');
    } catch (error) {
      console.error('Error al actualizar perfil:', error);
      toast.error('No se pudo actualizar el perfil.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0];
      const formData = new FormData();
      formData.append('file', file);

      setIsUploading(true);
      toast.info('Subiendo foto de perfil...');

      try {
        const updatedProfile = await uploadProfilePhoto(formData);
        setUserProfile(updatedProfile);
        toast.success('Foto de perfil actualizada.');
      } catch (error: any) {
        console.error('Error al subir la foto:', error);
        const errorMessage = error.response?.data?.message || 'No se pudo actualizar la foto de perfil.';
        toast.error(errorMessage);
      } finally {
        setIsUploading(false);
      }
    }
  };

  return (
    <div className="container mx-auto p-4">
      <Toaster />
      <h1 className="text-3xl font-bold mb-6">Mi Perfil</h1>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Columna de la foto de perfil */}
        <div className="md:col-span-1 flex flex-col items-center">
          <div className="relative">
            <img
              className="h-40 w-40 rounded-full object-cover shadow-lg"
              src={userProfile?.fotoPerfil ? `${API_URL}${userProfile.fotoPerfil}?t=${new Date().getTime()}` : `https://ui-avatars.com/api/?name=${userProfile?.nombre}&size=160&background=random`}
              alt="Foto de perfil"
            />
            <label htmlFor="foto-perfil-input" className={`absolute bottom-0 right-0 bg-indigo-600 p-2 rounded-full cursor-pointer hover:bg-indigo-700 transition ${isUploading ? 'opacity-50' : ''}`}>
              {isUploading ? (
                <svg className="animate-spin h-6 w-6 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
              ) : (
                <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.5L14.732 3.732z"></path></svg>
              )}
              <input id="foto-perfil-input" type="file" className="hidden" accept="image/png, image/jpeg" onChange={handleFileChange} disabled={isUploading} />
            </label>
          </div>
          <h2 className="text-2xl font-semibold mt-4">{userProfile?.nombre}</h2>
          <p className="text-gray-600">{userProfile?.cargo}</p>
        </div>

        {/* Columna de información y formulario */}
        <div className="md:col-span-2 bg-white p-6 rounded-lg shadow-md">
          {!isEditing ? (
            <div>
              <h3 className="text-xl font-semibold mb-4">Información de la Cuenta</h3>
              <p className="mb-2"><strong>Email:</strong> {userProfile?.email}</p>
              <div className="mb-4">
                <strong>Permisos:</strong>
                <ul className="list-disc list-inside ml-4 mt-2">
                  {userProfile?.permisos.map(p => <li key={p} className="text-gray-700">{p}</li>)}
                </ul>
              </div>
              <button onClick={() => setIsEditing(true)} className="bg-indigo-500 text-white font-bold py-2 px-4 rounded hover:bg-indigo-600 transition">
                Editar Perfil
              </button>
            </div>
          ) : (
            <form onSubmit={handleUpdateProfile}>
              <h3 className="text-xl font-semibold mb-4">Editar Información</h3>
              <div className="mb-4">
                <label className="block text-gray-700 font-bold mb-2" htmlFor="nombre">Nombre</label>
                <input type="text" id="nombre" value={nombre} onChange={e => setNombre(e.target.value)} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" />
              </div>
              <div className="mb-4">
                <label className="block text-gray-700 font-bold mb-2" htmlFor="password">Nueva Contraseña</label>
                <input type="password" id="password" value={password} onChange={e => setPassword(e.target.value)} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Dejar en blanco para no cambiar" />
              </div>
              <div className="mb-4">
                <label className="block text-gray-700 font-bold mb-2" htmlFor="confirmPassword">Confirmar Contraseña</label>
                <input type="password" id="confirmPassword" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" />
              </div>
              <div className="flex gap-4">
                <button type="submit" disabled={isLoading} className={`bg-green-500 text-white font-bold py-2 px-4 rounded hover:bg-green-600 transition ${isLoading ? 'opacity-50' : ''}`}>
                  {isLoading ? 'Guardando...' : 'Guardar Cambios'}
                </button>
                <button type="button" onClick={() => setIsEditing(false)} className="bg-gray-500 text-white font-bold py-2 px-4 rounded hover:bg-gray-600 transition">
                  Cancelar
                </button>
              </div>
            </form>
          )}
        </div>
      </div>
    </div>
  );
};

export default PerfilPage;
