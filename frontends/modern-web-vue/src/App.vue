<template>
  <div id="app">
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <router-link class="navbar-brand" to="/">Modern Web</router-link>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <router-link class="nav-link" to="/simple">Simple</router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/table">Table</router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/login">Login</router-link>
                </li>
            </ul>
        </div>
    </nav>

    <router-view></router-view>

  </div>
</template>

<script>
export default {
  updated () {
    var anonymousRoutes = ['/', '/login', '/logout', '/terms', '/privacy'];
    if (!localStorage.token && !anonymousRoutes.includes(this.$route.path)) {
      this.$router.push('/login?redirect=' + this.$route.path);
    }
  }
}
</script>