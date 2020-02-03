<template>
  <div class="login-wrapper border border-light">
    <form class="form-signin" @submit.prevent="login">
      <h2 class="form-signin-heading">Please sign in</h2>
      <div class="alert alert-danger" v-if="error">{{ error }}</div>
      <label for="userName" class="sr-only">User Name</label>
      <input v-model="userName" type="text" id="userName" class="form-control" placeholder="User Name" autocomplete="off" required autofocus>
      <label for="inputPassword" class="sr-only">Password</label>
      <input v-model="password" type="password" id="inputPassword" class="form-control" placeholder="Password" autocomplete="off" required>
      <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
    </form>
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
      debugger;

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
      this.error = false;

      this.$router.replace(this.$route.query.redirect || '/')
    }

    , loginFailed() {
      this.error = 'Login failed!';
      delete localStorage.account;
      delete localStorage.token;
    }

  }
}
</script>

<style lang="css">
body {
  background: #605B56;
}

.login-wrapper {
  background: #fff;
  width: 70%;
  margin: 12% auto;
}

.form-signin {
  max-width: 330px;
  padding: 10% 15px;
  margin: 0 auto;
}
.form-signin .form-signin-heading,
.form-signin .checkbox {
  margin-bottom: 10px;
}
.form-signin .checkbox {
  font-weight: normal;
}
.form-signin .form-control {
  position: relative;
  height: auto;
  -webkit-box-sizing: border-box;
          box-sizing: border-box;
  padding: 10px;
  font-size: 16px;
}
.form-signin .form-control:focus {
  z-index: 2;
}
.form-signin input[type="userName"] {
  margin-bottom: -1px;
  border-bottom-right-radius: 0;
  border-bottom-left-radius: 0;
}
.form-signin input[type="password"] {
  margin-bottom: 10px;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
}
</style>