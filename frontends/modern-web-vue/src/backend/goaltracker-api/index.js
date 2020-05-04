import Vue from 'vue';
import axios from 'axios';

const GOALTRACKER_API_URL = process.env.GOALTRACKER_API_URL || 'http://localhost:6300/api/';
const GOALTRACKER_API_USER = process.env.GOALTRACKER_API_USER || 'API_GOALTRACKER';
const GOALTRACKER_API_PASS = process.env.GOALTRACKER_API_PASS || 'HE_SHOOTS_HE_SCORES';
const GOALTRACKER_API_CREDENTIALS = btoa(GOALTRACKER_API_USER + ':' + GOALTRACKER_API_PASS);

const GoalTrackerApi = axios.create({
  baseURL: GOALTRACKER_API_URL
  , headers: {
    'Content-Type': 'application/json',
    'Authorization': 'Basic ' + GOALTRACKER_API_CREDENTIALS
  }
});

Vue.prototype.GoalTrackerApi = GoalTrackerApi;

export default {
};

