<template>
  <div id="landingPage" class="container-fluid h-100">
    <div class="row">
      <br />
      <br />
      <br />
    </div>
    <div class="row mx-auto">
      <div class="jumbotron jumbotron-fluid">
        <div class="container">
          <h1>Modern Web Vue</h1>
          <p>
            An experiment using vue, bootstrap to display data from asp core 3, brash, and sqlite.
          </p>
        </div>
      </div>   
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