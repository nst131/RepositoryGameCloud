import axios from 'axios';

const authApi = axios.create({
  //baseURL: 'https://localhost:5001/'
   baseURL: 'http://localhost:5001/'
});

export default authApi;