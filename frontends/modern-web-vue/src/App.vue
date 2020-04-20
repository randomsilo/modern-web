<template>
  <div id="app" class="container-fluid d-flex h-100 flex-column">
    
    <nav class="row navbar navbar-expand-md navbar-dark bg-dark">
        <router-link class="navbar-brand" to="/">Modern Web</router-link>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <router-link class="nav-link" to="/Todolist" v-if="hasPermission('Feature1','View')">Todo List</router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/login" v-if="hasAccount() == false">Login</router-link>
                </li>
            </ul>
        </div>
        <div>
          <button 
            class="btn btn-outline-danger" 
            @click="logout()" 
            v-if="hasAccount()"><small>{{ account.userName }}</small> Logout
          </button>
        </div>
    </nav>
    

    <div id="main-content" class="row flex-fill d-flex justify-content-start">
      <router-view></router-view>
    </div>
    
    <div class="row">
      <div class="page-footer fixed-bottom border-top bg-dark">
        <div class="row">
          <div class="col col-md-3 col-sm-12">
          </div>
          <div class="col-md-3 col-sm-12">
            <small class="d-block text-muted">Daniel A. Dawson © 2020</small>
          </div>
          <div class=" col-md-3 col-sm-12">
            <router-link to="/TermsOfService">
              <small class="d-block text-muted">Terms of Service</small>
            </router-link>
          </div>
          <div class="col-md-3 col-sm-12">
            <router-link to="/PrivacyPolicy">
              <small class="d-block text-muted">Privacy Policy</small>
            </router-link>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
export default {
  updated() {
    var anonymousRoutes = ['/', '/login', '/logout', '/TermsOfService', '/PrivacyPolicy'];
    if (!localStorage.token && !anonymousRoutes.includes(this.$route.path)) {
      this.$router.push('/login?redirect=' + this.$route.path);
    }
  }

  , data() {
    return {
      account: null
      , token: null
      , privileges: null
    };
  }

  , methods: {
    
    hasAccount() {
      var found = false;
      if (this.account && this.account.userName && this.account.userName.length > 0) {
        found = true;
      } 
      return found;
    }

    , loadUserData: function() {
      if (!this.hasAccount() && localStorage.account) {
        this.account = localStorage.account ? JSON.parse(localStorage.account) : null;
        this.token = localStorage.token ? JSON.parse(localStorage.token) : null;
        this.privileges = localStorage.privileges ? JSON.parse(localStorage.privileges) : null;
      }
    }

    , hasPermission: function(feature, ability) {
      this.loadUserData();
      
      var permissionFound = false;

      if (this.account && this.token && this.privileges) {
        permissionFound = this.privileges[feature].includes(ability);
      }

      return permissionFound;
    }

    , getUserName() {
      var userName = "Anonymous";
      if (this.account && this.account.userName && this.account.userName.length > 0) {
        userName = this.account.userName;
      }

      return userName;
    }

    , logout: function() {
      delete localStorage.account;
      delete localStorage.token;
      delete localStorage.privileges;

      this.account = null;
      this.token = null;
      this.privileges = null;

      this.loadUserData();
    }
  }
}
</script>

<style lang="css">
  html, body {
    height: 100%;
  }

  div#main-content {
    background-color: whitesmoke;
  }

  .flex-fill {
    flex:1;
  }
</style>