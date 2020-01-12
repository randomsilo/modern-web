<template>
  <div class="simple">
    <h1>Simple</h1>

    <div class="p-3 bg-secondary progress-bar-striped" style="min-height: 170px;">
        <b-button class="mb-2" variant="primary" @click="$bvToast.show('example-toast')">
            Show toast
        </b-button>
        <b-toast id="example-toast" title="BootstrapVue" static no-auto-hide>
        Hello, world! This is a toast message.
        </b-toast>
    </div>
  </div>
</template>

<script>
export default {
  name: 'simple',
}
</script>