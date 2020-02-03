import Vue from 'vue'
import axios from 'axios';

const LOYAL_GUARD_API_URL = process.env.LOYAL_GUARD_API_URL || 'http://localhost:6101/api/';
const LOYAL_GUARD_API_USER = process.env.LOYAL_GUARD_API_USER || 'API_LOYALGUARD';
const LOYAL_GUARD_API_PASS = process.env.LOYAL_GUARD_API_PASS || 'API_TWO_IF_BY_SEA';
const LOYAL_GUARD_API_CREDENTIALS = btoa(LOYAL_GUARD_API_USER + ':' + LOYAL_GUARD_API_PASS);

const LoyalGuardApi = axios.create({
  baseURL: LOYAL_GUARD_API_URL
  , headers: {
    'Content-Type': 'application/json',
    'Authorization': 'Basic ' + LOYAL_GUARD_API_CREDENTIALS
  }
});

Vue.prototype.LoyalGuardApi = LoyalGuardApi;

export default {
};
