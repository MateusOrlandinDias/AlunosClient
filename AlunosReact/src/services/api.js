import axios from 'axios';

//baseUrl -> Qual o local host do swagger?
const api = axios.create({
    baseURL: "https://localhost:44369",
});

export default api;