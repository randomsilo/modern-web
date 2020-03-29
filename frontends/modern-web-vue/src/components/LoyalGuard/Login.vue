<template>
  <div id="loyalGuardLoginPage" class="container-fluid h-100">

    <div class="row">
      <br />
      <br />
      <br />
    </div>

    <div class="row">
      <div class="col">
        
      </div>

      <div class="col-sm-6 col-md-5 col-lg-4 col-xl-3">
        <div class="card bg-light">
          <div class="card-header">Please Sign In</div>
          <div class="card-body">
            <form class="form-signin col-sm-11" @submit.prevent="login">
              <div class="alert alert-danger" v-if="error">{{ error }}</div>

              <div class="form-group row">
                <label for="userName" class="col">User Name</label>
                <input v-model="userName" type="text" id="userName" class="form-control form-control-sm col" autocomplete="off" required autofocus>
              </div>

              <div class="form-group row">
                <label for="inputPassword" class="col">Password</label>
                <input v-model="password" type="password" id="inputPassword" class="form-control form-control-sm col" autocomplete="off" required>
              </div>

              <div class="form-group row">
                <button class="btn btn-lg btn-primary ml-auto" type="submit">Sign in</button>
              </div>
            </form>
            <br />
          </div>
        </div>
      </div>
    </div>
    
  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: 'LoyalGuardLogin',
  data() {
    return {
      userName: '',
      password: '',
      error: false
    }
  },
  methods: {
    login() {
      this.LoyalGuardApi.post('/Auth'
        , { 
          userName: this.userName
          , password: this.password 
        })
        .then(request => this.loginSuccessful(request))
        .catch(() => this.loginFailed())
    }
    , loginSuccessful (req) {
      
      if (req.data.token == null) {
        this.loginFailed();
        return
      }

      if (req.data.token.expires < new Date()) {
        this.loginFailed();
        return
      }

      localStorage.account = JSON.stringify(req.data.account);
      localStorage.token = JSON.stringify(req.data.token);
      localStorage.privileges = JSON.stringify(req.data.privileges);
      this.error = false;

      this.$router.replace(this.$route.query.redirect || '/')
    }

    , loginFailed() {
      this.error = 'Login failed!';
      delete localStorage.account;
      delete localStorage.token;
      delete localStorage.privileges;
    }

  }
}
</script>

<style lang="css">
  div#loyalGuardLoginPage {
    background: url(/images/miami_skyline.jpg) no-repeat center center fixed;
    -webkit-background-size: cover;
    -moz-background-size: cover;
    -o-background-size: cover;
    background-size: cover;
  }

  div#login-wrapper {
    padding: 10px;
  }
  
</style>