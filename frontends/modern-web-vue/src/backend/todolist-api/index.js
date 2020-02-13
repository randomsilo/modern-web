import Vue from 'vue'
import axios from 'axios';

const TODOLIST_API_URL = process.env.TODOLIST_API_URL || 'http://localhost:6200/api/';
const TODOLIST_API_USER = process.env.TODOLIST_API_USER || 'API_TODOLIST';
const TODOLIST_API_PASS = process.env.TODOLIST_API_PASS || 'API_THINGS_TO_DO';
const TODOLIST_API_CREDENTIALS = btoa(TODOLIST_API_USER + ':' + TODOLIST_API_PASS);

const TodoListApi = axios.create({
  baseURL: TODOLIST_API_URL
  , headers: {
    'Content-Type': 'application/json',
    'Authorization': 'Basic ' + TODOLIST_API_CREDENTIALS
  }
});

Vue.prototype.TodoListApi = TodoListApi;

export default {
};
