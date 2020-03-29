<template>
  <div id="landingPage" class="container-fluid h-100">
    <div class="row">
      <br />
      <br />
      <br />
    </div>
    <div class="row mx-auto">
      <b-jumbotron header="Modern Web Vue" lead="Vue 3, Bootstrap 4, Brash 1">
        <p>
          An experiment using vue, bootstrap to display data from asp core 3, brash, and sqlite.
        </p>
      </b-jumbotron>
    
    </div>
  </div>
</template>

<script>
export default {
  name: 'landing',
}
</script>

<style lang="css">
  div#landingPage {
    background: url(/images/miami_skyline.jpg) no-repeat center center fixed;
    -webkit-background-size: cover;
    -moz-background-size: cover;
    -o-background-size: cover;
    background-size: cover;
  }
</style>