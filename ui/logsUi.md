:5012/api/inspecciones:1 
 Failed to load resource: the server responded with a status of 403 (Forbidden)

InspeccionesPage.tsx:16 Error al cargar las inspecciones: 
AxiosError
:5012/api/inspecciones:1 
 Failed to load resource: the server responded with a status of 403 (Forbidden)
InspeccionesPage.tsx:16 Error al cargar las inspecciones: 
AxiosError
code
: 
"ERR_BAD_REQUEST"
config
: 
{transitional: {…}, adapter: Array(3), transformRequest: Array(1), transformResponse: Array(1), timeout: 0, …}
message
: 
"Request failed with status code 403"
name
: 
"AxiosError"
request
: 
XMLHttpRequest {onreadystatechange: null, readyState: 4, timeout: 0, withCredentials: false, upload: XMLHttpRequestUpload, …}
response
: 
config
: 
{transitional: {…}, adapter: Array(3), transformRequest: Array(1), transformResponse: Array(1), timeout: 0, …}
data
: 
""
headers
: 
AxiosHeaders {content-length: '0'}
request
: 
XMLHttpRequest {onreadystatechange: null, readyState: 4, timeout: 0, withCredentials: false, upload: XMLHttpRequestUpload, …}
status
: 
403
statusText
: 
"Forbidden"
[[Prototype]]
: 
Object
status
: 
403
stack
: 
"AxiosError: Request failed with status code 403\n    at settle (http://localhost:5173/node_modules/.vite/deps/axios.js?v=881b429c:1257:12)\n    at XMLHttpRequest.onloadend (http://localhost:5173/node_modules/.vite/deps/axios.js?v=881b429c:1606:7)\n    at Axios.request (http://localhost:5173/node_modules/.vite/deps/axios.js?v=881b429c:2223:41)\n    at async getAllInspecciones (http://localhost:5173/src/services/inspeccion.service.ts?t=1768430043600:48:22)\n    at async fetchInspecciones (http://localhost:5173/src/pages/InspeccionesPage.tsx?t=1768430062605:33:30)"
[[Prototype]]
: 
Error