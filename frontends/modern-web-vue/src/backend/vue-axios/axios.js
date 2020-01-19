import axios from 'axios';

const API_URL = process.env.API_URL || 'http://localhost:6101/api/';
const API_USER = process.env.API_USER || 'API_LOYALGUARD';
const API_PASS = process.env.API_PASS || 'API_TWO_IF_BY_SEA';
const API_CREDENTIALS = btoa(API_USER + ':' + API_PASS);

export default axios.create({
  baseURL: API_URL
  , headers: {
    'Content-Type': 'application/json',
    'Authorization': 'Basic ' + API_CREDENTIALS
  }
})